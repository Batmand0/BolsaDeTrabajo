using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioPerfil
    {
        Task<int> DeletePerfilByClave(int clave);
        Task<IEnumerable<PerfilModel>> GetAllPerfiles();
        Task<PerfilModel> GetPerfilByClave(int clave);
        Task<int> InsertPerfil(PerfilModel perfilModel);
        Task<int> UpdatePerfil(PerfilModel perfilModel);
    }
    public class RepositorioPerfil : IRepositorioPerfil
    {
        private readonly DapperContext _context;

        public RepositorioPerfil(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PerfilModel>> GetAllPerfiles()
        {
            var query = "SELECT * FROM Perfil";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<PerfilModel>(query);
            }
        }
        public async Task<PerfilModel> GetPerfilByClave(int clave)
        {
            var query = "SELECT * FROM Perfil WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<PerfilModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertPerfil(PerfilModel perfilModel)
        {
            var query = "INSERT INTO Perfil (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { perfilModel.Clave, perfilModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdatePerfil(PerfilModel perfilModel)
        {
            var query = "UPDATE Perfil SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { perfilModel.Descripcion, perfilModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeletePerfilByClave(int clave)
        {
            var query = "DELETE FROM Perfil WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
