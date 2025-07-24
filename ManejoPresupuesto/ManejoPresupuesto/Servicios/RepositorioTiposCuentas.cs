

using System.Data;
using System.Threading.Tasks;
using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);

        Task<bool> Existe(string nombre, int usuarioId);

        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);

        Task Actualizar(TipoCuenta tipoCuenta);

        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);

        Task Borrar(int id);

        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("TiposCuentas_Insertar",
                                                new
                                                {
                                                    usuarioId = tipoCuenta.UsuarioId,
                                                    nombre = tipoCuenta.Nombre
                                                },
                                                commandType: System.Data.CommandType.StoredProcedure);
            /*     ($@"INSERT INTO TiposCuentas (Nombre,UsuarioId,Orden) 
                    VALUES (@Nombre, @UsuarioId, 0);
                    SELECT SCOPE_IDENTITY()", tipoCuenta);==> SCOPE_IDENTITY() trae el nombre del registro recien creado  */
            tipoCuenta.Id = id;
        }


        //Verificar si existe un tipo de cuenta con el mismo nombre y usuario
        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            //QueryFirstOrDefaultAsync ==> devuelve el primer registro que cumpla con la condicion
            //QueryFirstOrDefaultAsync devuelve un int, si el nombre no existe devuelve 0, si existe devuelve 1
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM TiposCuentas
                                    WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                                    new { nombre, usuarioId });
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                            FROM TiposCuentas
                                                            WHERE UsuarioId = @UsuarioId
                                                            ORDER BY Orden", new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas
                                            SET Nombre = @Nombre
                                            WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"
                                                                SELECT Id, Nombre, Orden
                                                                FROM TiposCuentas
                                                                WHERE Id = @Id AND UsuarioId = @UsuarioId",
                                                                new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            var query = "UPDATE TiposCuentas SET Orden = @Orden Where Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tipoCuentasOrdenados);
        }
    }
}