using Microsoft.Data.SqlClient;
using MotorRecomendacion.Models;

namespace MotorRecomendacion.Data
{
    public class InteraccionRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public InteraccionRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task RegistrarAsync(
            int idUsuario,
            int idContenido,
            string tipoInteraccion)
        {
            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();

            const string sql = @"
                INSERT INTO Interacciones
                (
                    IdUsuario,
                    IdContenido,
                    TipoInteraccion
                )
                VALUES
                (
                    @IdUsuario,
                    @IdContenido,
                    @TipoInteraccion
                )";

            await using var comando =
                new SqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@IdUsuario",
                idUsuario);

            comando.Parameters.AddWithValue(
                "@IdContenido",
                idContenido);

            comando.Parameters.AddWithValue(
                "@TipoInteraccion",
                tipoInteraccion);

            await comando.ExecuteNonQueryAsync();
        }


        public async Task<List<HistorialInteraccion>>
            ObtenerHistorialAsync(int idUsuario)
        {
            var historial =
                new List<HistorialInteraccion>();

            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();

           const string sql = @"
                WITH UltimasInteracciones AS
                (
                    SELECT
                        i.IdInteraccion,
                        i.IdUsuario,
                        i.IdContenido,
                        i.TipoInteraccion,
                        i.Fecha,

                        ROW_NUMBER() OVER
                        (
                            PARTITION BY i.IdContenido
                            ORDER BY
                                i.Fecha DESC,
                                i.IdInteraccion DESC
                        ) AS NumeroFila

                    FROM Interacciones i

                    WHERE i.IdUsuario = @IdUsuario
                )

                SELECT
                    i.IdInteraccion,
                    c.IdContenido,
                    c.Titulo,
                    c.TipoContenido,
                    c.Categoria,
                    c.AutorDirector,
                    c.Anio,
                    i.TipoInteraccion,
                    i.Fecha

                FROM UltimasInteracciones i

                INNER JOIN Contenidos c
                    ON c.IdContenido = i.IdContenido

                WHERE i.NumeroFila = 1

                ORDER BY i.Fecha DESC";

            await using var comando =
                new SqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@IdUsuario",
                idUsuario);

            await using var lector =
                await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                historial.Add(
                    new HistorialInteraccion
                    {
                        IdInteraccion =
                            lector.GetInt32(0),

                        IdContenido =
                            lector.GetInt32(1),

                        Titulo =
                            lector.GetString(2),

                        TipoContenido =
                            lector.GetString(3),

                        Categoria =
                            lector.GetString(4),

                        AutorDirector =
                            lector.GetString(5),

                        Anio =
                            lector.GetInt32(6),

                        TipoInteraccion =
                            lector.GetString(7),

                        Fecha =
                            lector.GetDateTime(8)
                    }
                );
            }

            return historial;
        }
    }
}