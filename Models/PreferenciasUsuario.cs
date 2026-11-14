namespace MotorRecomendacion.Models
{
    public class PreferenciasUsuario
    {
        public string TipoPreferido { get; set; }

        public List<string> CategoriasPreferidas { get; set; }
            = new List<string>();
    }
}