using System;

namespace MotorRecomendacion
{
    public class Recomendacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string TipoContenido { get; set; }
        public string Categoria { get; set; }
        public string AutorDirector { get; set; }
        public int Anio { get; set; }
        public string Sinopsis { get; set; }
        public int PuntajeSimilitud { get; set; }
        public string Estado { get; set; } = "Pendiente";

        // Constructor vacío obligatorio para que System.Text.Json pueda leer bd_recomienda.json
        public Recomendacion() { }

        public Recomendacion(int id, string titulo, string tipo, string categoria, string autor, int anio, string sinopsis, int similitud)
        {
            Id = id;
            Titulo = titulo;
            TipoContenido = tipo;
            Categoria = categoria;
            AutorDirector = autor;
            Anio = anio;
            Sinopsis = sinopsis;
            PuntajeSimilitud = similitud;
            Estado = "Pendiente";
        }

        public override string ToString()
        {
            return $"[{TipoContenido} - {Categoria}] {Titulo} ({Anio}) | Dir/Autor: {AutorDirector} | Similitud: {PuntajeSimilitud}% | Estado: {Estado}";
        }
    }
}