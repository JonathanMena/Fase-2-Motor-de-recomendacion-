namespace MotorRecomendacion.Models
{
    public class AfinidadCategoria
    {
        public string Categoria { get; set; } = string.Empty;
        public int Puntaje { get; set; }
    }

    public class PerfilUsuario
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<AfinidadCategoria> Afinidades { get; set; }
            = new List<AfinidadCategoria>();
    }
}