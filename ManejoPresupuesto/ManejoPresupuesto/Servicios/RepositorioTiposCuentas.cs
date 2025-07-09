

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
            var id = await connection.QuerySingleAsync<int>
                                        ($@"INSERT INTO TiposCuentas (Nombre,UsuarioId,Orden) 
                                            VALUES (@Nombre, @UsuarioId, 0);
                                            SELECT SCOPE_IDENTITY()", tipoCuenta);
            tipoCuenta.Id = id;
        }


        //Verificar si existe un tipo de cuenta con el mismo nombre y usuario
        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                @"select 1
                from TiposCuentas
                where Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                new { nombre, usuarioId });

            return existe == 1;
        }


        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>
            ($@"select * from TiposCuentas where UsuarioId = @UsuarioId order by Orden asc", new { usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            //permite ejecutar un query que no devuelva nada
            await connection.ExecuteAsync(@"Update TiposCuentas SET Nombre = @Nombre
                                            where Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>
            ($@"select Id, Nombre, Orden
                    from TiposCuentas 
                    where Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id = @Id", new { id });
        }

    }
}