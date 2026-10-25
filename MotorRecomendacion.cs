
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
            var resultado = contenidos.ToList();

            foreach (var contenido in resultado)
            {
                int puntaje = 0;

                if (!string.IsNullOrWhiteSpace(tipoPreferido) &&
                    string.Equals(
                        contenido.TipoContenido,
                        tipoPreferido,
                        StringComparison.OrdinalIgnoreCase))
                {
                    puntaje += 30;
                }

                if (categoriasPreferidas != null &&
                    categoriasPreferidas.Any(c =>
                        string.Equals(
                            c,
                            contenido.Categoria,
                            StringComparison.OrdinalIgnoreCase)))
                {
                    puntaje += 40;
                }

                int puntajeOriginal = Math.Clamp(
                    contenido.PuntajeSimilitud, 0, 100);

                puntaje += (int)Math.Round(puntajeOriginal * 0.30);

                contenido.PuntajeSimilitud = Math.Clamp(puntaje, 0, 100);
            }

            return resultado
                .OrderByDescending(c => c.PuntajeSimilitud)
                .ToList();
        }
    }
}
