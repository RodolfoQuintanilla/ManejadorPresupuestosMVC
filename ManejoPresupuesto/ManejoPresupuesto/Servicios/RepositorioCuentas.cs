

using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuentaEditar);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Craer(Cuenta cuenta);
        Task<Cuenta> ObtenetPorId(int id, int usuarioId);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Craer(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                    @"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
                    VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                    SELECT SCOPE_IDENTITY();", cuenta);
            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"
                                    SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
                                    FROM Cuentas
                                    INNER JOIN TiposCuentas tc
                                    ON tc.Id = Cuentas.TipoCuentaId
                                    WHERE tc.UsuarioId = @UsuarioId
                                    ORDER BY tc.Orden", new { usuarioId });
        }

        public async Task<Cuenta> ObtenetPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"
        SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, tc.Id AS TipoCuentaId
        FROM Cuentas
        INNER JOIN TiposCuentas tc
            ON tc.Id = Cuentas.TipoCuentaId
        WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id", new { id, usuarioId });
        }


        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Cuentas
                                    SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                    TipoCuentaId = @TipoCuentaId
                                    WHERE Id = @Id;", cuenta);
        }



    }
}