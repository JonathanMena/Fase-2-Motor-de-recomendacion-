using Microsoft.Data.SqlClient;

namespace MotorRecomendacion.Data
{
    public class ContenidoRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ContenidoRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


  public async Task<List<Recomendacion>> ObtenerTodosAsync(
    int idUsuario)
{
    var contenidos = new List<Recomendacion>();

    await using var conexion =
        _connectionFactory.CrearConexion();

    await conexion.OpenAsync();

    const string sql = @"
        SELECT
            c.IdContenido,
            c.Titulo,
            c.TipoContenido,
            c.Categoria,
            c.AutorDirector,
            c.Anio,
            c.Sinopsis,
            c.PuntajeBase
        FROM Contenidos c
        WHERE c.Activo = 1
        AND NOT EXISTS
        (
            SELECT 1
            FROM Interacciones i
            WHERE i.IdContenido = c.IdContenido
              AND i.IdUsuario = @IdUsuario
              AND i.TipoInteraccion = 'Descartado'
        )
        ORDER BY c.IdContenido";

    await using var comando =
        new SqlCommand(sql, conexion);

    comando.Parameters.AddWithValue(
        "@IdUsuario",
        idUsuario);

    await using var lector =
        await comando.ExecuteReaderAsync();

    while (await lector.ReadAsync())
    {
        contenidos.Add(
            new Recomendacion
            {
                Id = lector.GetInt32(0),
                Titulo = lector.GetString(1),
                TipoContenido = lector.GetString(2),
                Categoria = lector.GetString(3),
                AutorDirector = lector.GetString(4),
                Anio = lector.GetInt32(5),

                Sinopsis = lector.IsDBNull(6)
                    ? string.Empty
                    : lector.GetString(6),

                PuntajeSimilitud =
                    lector.GetInt32(7),

                Estado = "Pendiente"
            }
        );
    }

    return contenidos;
}
        public async Task<int> InsertarAsync(
            Recomendacion contenido)
        {
            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();

            const string sql = @"
                INSERT INTO Contenidos
                (
                    Titulo,
                    TipoContenido,
                    Categoria,
                    AutorDirector,
                    Anio,
                    Sinopsis,
                    PuntajeBase
                )
                OUTPUT INSERTED.IdContenido
                VALUES
                (
                    @Titulo,
                    @TipoContenido,
                    @Categoria,
                    @AutorDirector,
                    @Anio,
                    @Sinopsis,
                    @PuntajeBase
                )";

            await using var comando =
                new SqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@Titulo",
                contenido.Titulo);

            comando.Parameters.AddWithValue(
                "@TipoContenido",
                contenido.TipoContenido);

            comando.Parameters.AddWithValue(
                "@Categoria",
                contenido.Categoria);

            comando.Parameters.AddWithValue(
                "@AutorDirector",
                contenido.AutorDirector);

            comando.Parameters.AddWithValue(
                "@Anio",
                contenido.Anio);

            comando.Parameters.AddWithValue(
                "@Sinopsis",
                contenido.Sinopsis ?? string.Empty);

            comando.Parameters.AddWithValue(
                "@PuntajeBase",
                contenido.PuntajeSimilitud);

            object resultado =
                await comando.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }
    }
}