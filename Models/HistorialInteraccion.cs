namespace MotorRecomendacion.Models
{
    public class HistorialInteraccion
    {
        public int IdInteraccion { get; set; }
        public int IdContenido { get; set; }
        public string Titulo { get; set; }
        public string TipoContenido { get; set; }
        public string Categoria { get; set; }
        public string AutorDirector { get; set; }
        public int Anio { get; set; }
        public string TipoInteraccion { get; set; }
        public DateTime Fecha { get; set; }
    }
}