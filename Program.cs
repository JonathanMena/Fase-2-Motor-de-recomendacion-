using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MotorRecomendacion;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Instancia única en memoria dinámica de nuestra Lista Doblemente Enlazada
ListaDoble listaRecomendaciones = BaseDeDatos.InicializarYCargarLista();

// Función auxiliar para armar la respuesta con el estado de los punteros en memoria
object ObtenerEstadoMemoria()
{
    Nodo actual = listaRecomendaciones.ObtenerNodoActual();
    var historial = new System.Collections.Generic.List<object>();

    for (int i = 1; i <= listaRecomendaciones.Longitud(); i++)
    {
        var item = listaRecomendaciones.Obtener(i);
        historial.Add(new
        {
            posicion = i,
            esActual = (i == listaRecomendaciones.Actual()),
            dato = item
        });
    }

    return new
    {
        posicionActual = listaRecomendaciones.Actual(),
        longitud = listaRecomendaciones.Longitud(),
        nodoAnterior = actual?.Anterior?.Dato,
        nodoActual = actual?.Dato,
        nodoSiguiente = actual?.Siguiente?.Dato,
        listaCompleta = historial
    };
}

// 1. Obtener estado actual de la ListaDoble
app.MapGet("/api/lista", () => Results.Ok(ObtenerEstadoMemoria()));

// 2. Operación Inicio() -> Ir a primerNodo (Cabeza)
app.MapPost("/api/lista/inicio", () =>
{
    listaRecomendaciones.Inicio();
    return Results.Ok(ObtenerEstadoMemoria());
});

// 3. Operación Siguiente() -> Avanzar punteroActual.Siguiente
app.MapPost("/api/lista/siguiente", () =>
{
    listaRecomendaciones.Siguiente();
    return Results.Ok(ObtenerEstadoMemoria());
});

// 4. Operación Anterior() -> Retroceder punteroActual.Anterior
app.MapPost("/api/lista/anterior", () =>
{
    listaRecomendaciones.Anterior();
    return Results.Ok(ObtenerEstadoMemoria());
});

// 5. Cambiar estado ("Me gusta" / "Guardado") en el Dato del Nodo actual
app.MapPost("/api/lista/estado/{nuevoEstado}", (string nuevoEstado) =>
{
    Nodo actual = listaRecomendaciones.ObtenerNodoActual();
    if (actual != null)
    {
        actual.Dato.Estado = nuevoEstado;
        BaseDeDatos.GuardarDesdeLista(listaRecomendaciones);
    }
    return Results.Ok(ObtenerEstadoMemoria());
});

// 6. Operación Eliminar(posición) -> Descartar nodo y reenlazar punteros Ant y Sig
app.MapDelete("/api/lista/eliminar/{posicion:int}", (int posicion) =>
{
    listaRecomendaciones.Eliminar(posicion);
    BaseDeDatos.GuardarDesdeLista(listaRecomendaciones);
    return Results.Ok(ObtenerEstadoMemoria());
});

// 7. Operación Insertar(elemento, posición) o Añadir(elemento)
app.MapPost("/api/lista/insertar", (InsercionRequest req) =>
{
    int nuevoId = listaRecomendaciones.Longitud() + 10;
    var nuevaRec = new Recomendacion(
        nuevoId, req.Titulo, req.TipoContenido, req.Categoria,
        req.AutorDirector, req.Anio, req.Sinopsis, req.PuntajeSimilitud
    );

    if (req.Posicion <= 0 || req.Posicion > listaRecomendaciones.Longitud())
        listaRecomendaciones.Añadir(nuevaRec);
    else
        listaRecomendaciones.Insertar(nuevaRec, req.Posicion);

    BaseDeDatos.GuardarDesdeLista(listaRecomendaciones);
    return Results.Ok(ObtenerEstadoMemoria());
});


app.MapPost("/api/recomendaciones/calcular", (
    PreferenciasRequest preferencias) =>
{
    var contenidos = new List<Recomendacion>();

    for (int i = 1; i <= listaRecomendaciones.Longitud(); i++)
    {
        contenidos.Add(listaRecomendaciones.Obtener(i));
    }

    var resultado = global::MotorRecomendacion.MotorRecomendacion.CalcularRecomendaciones(
        contenidos,
        preferencias.TipoPreferido,
        preferencias.CategoriasPreferidas ?? new List<string>()
    );

    return Results.Ok(resultado);
});



app.Run("http://localhost:5150");

public record InsercionRequest(
    string Titulo, string TipoContenido, string Categoria,
    string AutorDirector, int Anio, string Sinopsis,
    int PuntajeSimilitud, int Posicion
);


public record PreferenciasRequest(
    string TipoPreferido,
    List<string>? CategoriasPreferidas
);
