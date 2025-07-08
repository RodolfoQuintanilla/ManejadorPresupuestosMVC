

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

    }
}