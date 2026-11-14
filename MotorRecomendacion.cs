using System;
using System.Collections.Generic;
using System.Linq;

namespace MotorRecomendacion
{
    public static class MotorRecomendacion
    {
        public static List<Recomendacion> CalcularRecomendaciones(
            IEnumerable<Recomendacion> contenidos,
            string tipoPreferido,
            List<string> categoriasPreferidas)
        {
            var categorias = new HashSet<string>(
                categoriasPreferidas ?? new List<string>(),
                StringComparer.OrdinalIgnoreCase
            );

            var resultado = contenidos

                // El tipo seleccionado sí filtra el catálogo
                .Where(c =>
                    c != null &&
                    string.Equals(
                        c.TipoContenido,
                        tipoPreferido,
                        StringComparison.OrdinalIgnoreCase
                    )
                )

                // Creamos objetos nuevos para no modificar
                // el PuntajeBase que viene de SQL
                .Select(c =>
                {
                    int puntajeBase =
                        Math.Clamp(
                            c.PuntajeSimilitud,
                            0,
                            100
                        );

                    bool coincideCategoria =
                        categorias.Contains(c.Categoria);

                    // 65 puntos por categoría elegida
                    // + hasta 35 por calidad/puntaje base
                    int puntaje =
                        (coincideCategoria ? 65 : 0)
                        +
                        (int)Math.Round(
                            puntajeBase * 0.35
                        );

                    return new Recomendacion(
                        c.Id,
                        c.Titulo,
                        c.TipoContenido,
                        c.Categoria,
                        c.AutorDirector,
                        c.Anio,
                        c.Sinopsis,
                        Math.Clamp(puntaje, 0, 100)
                    )
                    {
                        Estado = c.Estado
                    };
                })

                .OrderByDescending(
                    c => c.PuntajeSimilitud
                )
                .ThenBy(
                    c => c.Titulo
                )
                .ToList();

            return resultado;
        }
    }
}