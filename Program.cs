using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using MotorRecomendacion;
using MotorRecomendacion.Data;
using MotorRecomendacion.Models;

using System.Collections.Generic;


var builder = WebApplication.CreateBuilder(args);


// ======================================================
// CONFIGURACIÓN SQL SERVER
// ======================================================

string connectionString =
    builder.Configuration
        .GetConnectionString("MotorRecomendacion");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión " +
        "'MotorRecomendacion' en appsettings.json"
    );
}


var connectionFactory =
    new SqlConnectionFactory(connectionString);

var contenidoRepository =
    new ContenidoRepository(connectionFactory);

var preferenciaRepository =
    new PreferenciaRepository(connectionFactory);

var interaccionRepository =
    new InteraccionRepository(connectionFactory);


// Por ahora el sistema trabaja con un usuario de demostración.
// Más adelante podría venir de autenticación.
const int UsuarioDemoId = 1;


var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();


// ======================================================
// CARGAR SQL → LISTA DOBLEMENTE ENLAZADA
// ======================================================

ListaDoble listaRecomendaciones =
    new ListaDoble();


var contenidosIniciales =
    await contenidoRepository
        .ObtenerTodosAsync(UsuarioDemoId);


foreach (var contenido in contenidosIniciales)
{
    listaRecomendaciones.Añadir(contenido);
}


listaRecomendaciones.Inicio();


Console.WriteLine(
    $"Lista cargada desde SQL Server: " +
    $"{listaRecomendaciones.Longitud()} nodos"
);


// ======================================================
// FUNCIÓN AUXILIAR PARA DEVOLVER ESTADO DE LA LISTA
// ======================================================

object ObtenerEstadoMemoria()
{
    Nodo actual =
        listaRecomendaciones.ObtenerNodoActual();

    var historial =
        new List<object>();


    for (
        int i = 1;
        i <= listaRecomendaciones.Longitud();
        i++)
    {
        var item =
            listaRecomendaciones.Obtener(i);

        historial.Add(
            new
            {
                posicion = i,

                esActual =
                    i == listaRecomendaciones.Actual(),

                dato = item
            }
        );
    }


    return new
    {
        posicionActual =
            listaRecomendaciones.Actual(),

        longitud =
            listaRecomendaciones.Longitud(),

        nodoAnterior =
            actual?.Anterior?.Dato,

        nodoActual =
            actual?.Dato,

        nodoSiguiente =
            actual?.Siguiente?.Dato,

        listaCompleta =
            historial
    };
}


// ======================================================
// LISTA DOBLEMENTE ENLAZADA
// ======================================================


// Obtener estado actual
app.MapGet(
    "/api/lista",
    () =>
    {
        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// Ir a cabeza
app.MapPost(
    "/api/lista/inicio",
    () =>
    {
        listaRecomendaciones.Inicio();

        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// Siguiente
app.MapPost(
    "/api/lista/siguiente",
    () =>
    {
        listaRecomendaciones.Siguiente();

        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// Anterior
app.MapPost(
    "/api/lista/anterior",
    () =>
    {
        listaRecomendaciones.Anterior();

        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// ======================================================
// ME GUSTA / GUARDADO
// ======================================================

app.MapPost(
    "/api/lista/estado/{nuevoEstado}",
    async (string nuevoEstado) =>
    {
        Nodo actual =
            listaRecomendaciones.ObtenerNodoActual();


        if (actual == null)
        {
            return Results.BadRequest(
                new
                {
                    mensaje =
                        "No existe un nodo actual"
                }
            );
        }


        string tipoInteraccion;


        if (
            nuevoEstado.Contains(
                "Me gusta",
                StringComparison.OrdinalIgnoreCase))
        {
            tipoInteraccion = "MeGusta";
        }
        else if (
            nuevoEstado.Contains(
                "Guardado",
                StringComparison.OrdinalIgnoreCase))
        {
            tipoInteraccion = "Guardado";
        }
        else
        {
            return Results.BadRequest(
                new
                {
                    mensaje =
                        "Estado no válido"
                }
            );
        }


        // Cambia el estado visual en memoria.
        actual.Dato.Estado =
            nuevoEstado;


        // Persiste la interacción en SQL Server.
        await interaccionRepository
            .RegistrarAsync(
                UsuarioDemoId,
                actual.Dato.Id,
                tipoInteraccion
            );


        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// ======================================================
// DESCARTAR / ELIMINAR NODO
// ======================================================

app.MapDelete(
    "/api/lista/eliminar/{posicion:int}",
    async (int posicion) =>
    {
        var recomendacion =
            listaRecomendaciones.Obtener(posicion);


        if (recomendacion == null)
        {
            return Results.NotFound(
                new
                {
                    mensaje =
                        "La posición indicada no existe"
                }
            );
        }


        // Guardamos la interacción ANTES
        // de eliminar el nodo de memoria.
        await interaccionRepository
            .RegistrarAsync(
                UsuarioDemoId,
                recomendacion.Id,
                "Descartado"
            );


        bool eliminado =
            listaRecomendaciones
                .Eliminar(posicion);


        if (!eliminado)
        {
            return Results.BadRequest(
                new
                {
                    mensaje =
                        "No fue posible eliminar el nodo"
                }
            );
        }


        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// ======================================================
// INSERTAR NUEVO CONTENIDO
// ======================================================

app.MapPost(
    "/api/lista/insertar",
    async (InsercionRequest req) =>
    {
        var nuevaRec =
            new Recomendacion(
                0,
                req.Titulo,
                req.TipoContenido,
                req.Categoria,
                req.AutorDirector,
                req.Anio,
                req.Sinopsis,
                req.PuntajeSimilitud
            );


        // Primero guardamos en SQL.
        int nuevoId =
            await contenidoRepository
                .InsertarAsync(nuevaRec);


        // SQL nos devuelve el IDENTITY.
        nuevaRec.Id =
            nuevoId;


        // Luego agregamos a la estructura dinámica.
        if (
            req.Posicion <= 0 ||
            req.Posicion >
                listaRecomendaciones.Longitud())
        {
            listaRecomendaciones
                .Añadir(nuevaRec);
        }
        else
        {
            listaRecomendaciones
                .Insertar(
                    nuevaRec,
                    req.Posicion
                );
        }


        return Results.Ok(
            ObtenerEstadoMemoria()
        );
    }
);


// ======================================================
// PREFERENCIAS DEL USUARIO
// ======================================================


// Guardar preferencias
app.MapPost(
    "/api/preferencias/{idUsuario:int}",
    async (
        int idUsuario,
        PreferenciasRequest preferencias) =>
    {
        if (
            string.IsNullOrWhiteSpace(
                preferencias.TipoPreferido) ||
            preferencias.CategoriasPreferidas == null ||
            preferencias.CategoriasPreferidas.Count == 0)
        {
            return Results.BadRequest(
                new
                {
                    mensaje =
                        "Debe seleccionar un tipo " +
                        "y al menos una categoría"
                }
            );
        }


        await preferenciaRepository
            .GuardarAsync(
                idUsuario,
                preferencias.TipoPreferido,
                preferencias.CategoriasPreferidas
            );


        return Results.Ok(
            new
            {
                mensaje =
                    "Preferencias guardadas correctamente"
            }
        );
    }
);


// Consultar preferencias
app.MapGet(
    "/api/preferencias/{idUsuario:int}",
    async (int idUsuario) =>
    {
        var preferencias =
            await preferenciaRepository
                .ObtenerAsync(idUsuario);


        if (preferencias == null)
        {
            return Results.NotFound(
                new
                {
                    mensaje =
                        "El usuario todavía no tiene preferencias"
                }
            );
        }


        return Results.Ok(preferencias);
    }
);


// ======================================================
// HISTORIAL DESDE SQL SERVER
// ======================================================

app.MapGet(
    "/api/historial/{idUsuario:int}",
    async (int idUsuario) =>
    {
        var historial =
            await interaccionRepository
                .ObtenerHistorialAsync(idUsuario);


        return Results.Ok(historial);
    }
);


// ======================================================
// MOTOR DE RECOMENDACIONES
// ======================================================

app.MapPost(
    "/api/recomendaciones/calcular",
    (
        PreferenciasRequest preferencias) =>
    {
        var contenidos =
            new List<Recomendacion>();


        for (
            int i = 1;
            i <= listaRecomendaciones.Longitud();
            i++)
        {
            contenidos.Add(
                listaRecomendaciones.Obtener(i)
            );
        }


        var resultado =
            global::MotorRecomendacion
                .MotorRecomendacion
                .CalcularRecomendaciones(
                    contenidos,
                    preferencias.TipoPreferido,
                    preferencias.CategoriasPreferidas
                        ?? new List<string>()
                );


        return Results.Ok(resultado);
    }
);


// ======================================================
// EJECUCIÓN
// ======================================================

app.Run(
    "http://localhost:5150"
);


// ======================================================
// REQUEST MODELS
// ======================================================

public record InsercionRequest(
    string Titulo,
    string TipoContenido,
    string Categoria,
    string AutorDirector,
    int Anio,
    string Sinopsis,
    int PuntajeSimilitud,
    int Posicion
);


public record PreferenciasRequest(
    string TipoPreferido,
    List<string>? CategoriasPreferidas
);