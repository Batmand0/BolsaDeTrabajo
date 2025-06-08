using BolsaDeTrabajo.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioNacionalidad
    {
        Task<int> DeleteNacionalidadByClave(int clave);
        Task<IEnumerable<NacionalidadModel>> GetAllNacionalidades();
        Task<NacionalidadModel> GetNacionalidadByClave(int clave);
        Task<int> InsertNacionalidad(NacionalidadModel nacionalidadModel);
        Task<int> UpdateNacionalidad(NacionalidadModel nacionalidadModel);
    }
    public class RepositorioNacionalidad : IRepositorioNacionalidad
    {
        private readonly DapperContext _context;

        public RepositorioNacionalidad(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<NacionalidadModel>> GetAllNacionalidades()
        {
            var query = "SELECT * FROM Nacionalidad";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<NacionalidadModel>(query);
                return result;
            }
        }

        public async Task<NacionalidadModel> GetNacionalidadByClave(int clave)
        {
            var query = "SELECT * FROM Nacionalidad WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<NacionalidadModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertNacionalidad(NacionalidadModel nacionalidadModel)
        {
            var query = "INSERT INTO Nacionalidad (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { nacionalidadModel.Clave, nacionalidadModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateNacionalidad(NacionalidadModel nacionalidadModel)
        {
            var query = "UPDATE Nacionalidad SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { nacionalidadModel.Descripcion, nacionalidadModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteNacionalidadByClave(int clave)
        {
            var query = "DELETE FROM Nacionalidad WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
