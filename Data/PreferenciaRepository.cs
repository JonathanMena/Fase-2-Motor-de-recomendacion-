using Microsoft.Data.SqlClient;
using MotorRecomendacion.Models;

namespace MotorRecomendacion.Data
{
    public class PreferenciaRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public PreferenciaRepository(
            SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public async Task GuardarAsync(
            int idUsuario,
            string tipoContenido,
            IEnumerable<string> categorias)
        {
            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();

            await using var transaccion =
                await conexion.BeginTransactionAsync();

            try
            {
                const string eliminarSql = @"
                    DELETE FROM PreferenciasUsuario
                    WHERE IdUsuario = @IdUsuario";

                await using (
                    var eliminar = new SqlCommand(
                        eliminarSql,
                        conexion,
                        (SqlTransaction)transaccion))
                {
                    eliminar.Parameters.AddWithValue(
                        "@IdUsuario",
                        idUsuario);

                    await eliminar.ExecuteNonQueryAsync();
                }


                const string insertarSql = @"
                    INSERT INTO PreferenciasUsuario
                    (
                        IdUsuario,
                        TipoContenido,
                        Categoria,
                        Peso
                    )
                    VALUES
                    (
                        @IdUsuario,
                        @TipoContenido,
                        @Categoria,
                        100
                    )";

Console.WriteLine(
    $"Guardando preferencias para usuario {idUsuario}"
);

Console.WriteLine(
    $"Tipo: {tipoContenido}"
);
                foreach (string categoria in categorias)
                {
                    await using var insertar =
                        new SqlCommand(
                            insertarSql,
                            conexion,
                            (SqlTransaction)transaccion);
Console.WriteLine(
    $"Insertando categoría: {categoria}"
);
                    insertar.Parameters.AddWithValue(
                        "@IdUsuario",
                        idUsuario);

                    insertar.Parameters.AddWithValue(
                        "@TipoContenido",
                        tipoContenido);

                    insertar.Parameters.AddWithValue(
                        "@Categoria",
                        categoria);

                    await insertar.ExecuteNonQueryAsync();
                }

                await transaccion.CommitAsync();
            }
       catch (Exception ex)
{
    Console.WriteLine(
        "ERROR GUARDANDO PREFERENCIAS: " + ex.Message
    );

    Console.WriteLine(ex.ToString());

    await transaccion.RollbackAsync();

    throw;
}
        }


        public async Task<PreferenciasUsuario?>
            ObtenerAsync(int idUsuario)
        {
            await using var conexion =
                _connectionFactory.CrearConexion();

            await conexion.OpenAsync();

            const string sql = @"
                SELECT
                    TipoContenido,
                    Categoria
                FROM PreferenciasUsuario
                WHERE IdUsuario = @IdUsuario
                ORDER BY IdPreferencia";

            await using var comando =
                new SqlCommand(sql, conexion);

            comando.Parameters.AddWithValue(
                "@IdUsuario",
                idUsuario);

            await using var lector =
                await comando.ExecuteReaderAsync();

            string tipo = null;
            var categorias = new List<string>();

            while (await lector.ReadAsync())
            {
                tipo ??= lector.GetString(0);

                categorias.Add(
                    lector.GetString(1)
                );
            }

            if (tipo == null)
                return null;

            return new PreferenciasUsuario
            {
                TipoPreferido = tipo,
                CategoriasPreferidas = categorias
            };
        }
    }
}