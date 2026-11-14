using Microsoft.Data.SqlClient;
using MotorRecomendacion.Models;

namespace MotorRecomendacion.Data
{
    public class PerfilRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public PerfilRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task<PerfilUsuario?> ObtenerAsync(
            int idUsuario)
        {
            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();


            // ==========================================
            // DATOS DEL USUARIO
            // ==========================================

            const string usuarioSql = @"
                SELECT
                    IdUsuario,
                    Nombre,
                    Email
                FROM Usuarios
                WHERE IdUsuario = @IdUsuario";

            var perfil = new PerfilUsuario();


            await using (
                var comandoUsuario =
                    new SqlCommand(
                        usuarioSql,
                        conexion))
            {
                comandoUsuario.Parameters.AddWithValue(
                    "@IdUsuario",
                    idUsuario);

                await using var lectorUsuario =
                    await comandoUsuario.ExecuteReaderAsync();

                if (!await lectorUsuario.ReadAsync())
                {
                    return null;
                }

                perfil.IdUsuario =
                    lectorUsuario.GetInt32(0);

                perfil.Nombre =
                    lectorUsuario.GetString(1);

                perfil.Email =
                    lectorUsuario.GetString(2);
            }


            // ==========================================
            // CALCULAR AFINIDADES
            // ==========================================

            const string afinidadSql = @"
                WITH CategoriasUsuario AS
                (
                    SELECT DISTINCT
                        Categoria
                    FROM PreferenciasUsuario
                    WHERE IdUsuario = @IdUsuario

                    UNION

                    SELECT DISTINCT
                        c.Categoria
                    FROM Interacciones i

                    INNER JOIN Contenidos c
                        ON c.IdContenido = i.IdContenido

                    WHERE i.IdUsuario = @IdUsuario
                ),

                DatosAfinidad AS
                (
                    SELECT
                        cu.Categoria,

                        CASE
                            WHEN EXISTS
                            (
                                SELECT 1
                                FROM PreferenciasUsuario p
                                WHERE p.IdUsuario = @IdUsuario
                                AND p.Categoria = cu.Categoria
                            )
                            THEN 60
                            ELSE 20
                        END AS BasePreferencia,

                        ISNULL
                        (
                            SUM
                            (
                                CASE
                                    WHEN i.TipoInteraccion = 'MeGusta'
                                        THEN 15

                                    WHEN i.TipoInteraccion = 'Guardado'
                                        THEN 10

                                    WHEN i.TipoInteraccion = 'Descartado'
                                        THEN -20

                                    ELSE 0
                                END
                            ),
                            0
                        ) AS PuntosInteraccion

                    FROM CategoriasUsuario cu

                    LEFT JOIN Contenidos c
                        ON c.Categoria = cu.Categoria

                    LEFT JOIN Interacciones i
                        ON i.IdContenido = c.IdContenido
                        AND i.IdUsuario = @IdUsuario

                    GROUP BY cu.Categoria
                )

                SELECT
                    Categoria,

                    CASE
                        WHEN BasePreferencia + PuntosInteraccion > 100
                            THEN 100

                        WHEN BasePreferencia + PuntosInteraccion < 0
                            THEN 0

                        ELSE BasePreferencia + PuntosInteraccion
                    END AS Puntaje

                FROM DatosAfinidad

                ORDER BY Puntaje DESC";

            await using var comandoAfinidad =
                new SqlCommand(
                    afinidadSql,
                    conexion);

            comandoAfinidad.Parameters.AddWithValue(
                "@IdUsuario",
                idUsuario);

            await using var lectorAfinidad =
                await comandoAfinidad.ExecuteReaderAsync();

            while (await lectorAfinidad.ReadAsync())
            {
                perfil.Afinidades.Add(
                    new AfinidadCategoria
                    {
                        Categoria =
                            lectorAfinidad.GetString(0),

                        Puntaje =
                            lectorAfinidad.GetInt32(1)
                    }
                );
            }


            return perfil;
        }
    }
}