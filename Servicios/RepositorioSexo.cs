using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioSexo
    {
        Task<int> DeleteSexoByClave(int clave);
        Task<IEnumerable<SexoModel>> GetAllSexos();
        Task<SexoModel> GetSexoByClave(int clave);
        Task<int> InsertSexo(SexoModel sexoModel);
        Task<int> UpdateSexo(SexoModel sexoModel);
    }
    public class RepositorioSexo : IRepositorioSexo
    {
        private readonly DapperContext _context;

        public RepositorioSexo(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SexoModel>> GetAllSexos()
        {
            var query = "SELECT * FROM Sexo";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<SexoModel>(query);
                return result;
            }
        }
        public async Task<SexoModel> GetSexoByClave(int clave)
        {
            var query = "SELECT * FROM Sexo WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SexoModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertSexo(SexoModel sexoModel)
        {
            var query = "INSERT INTO Sexo (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { sexoModel.Clave, sexoModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateSexo(SexoModel sexoModel)
        {
            var query = "UPDATE Sexo SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { sexoModel.Descripcion, sexoModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteSexoByClave(int clave)
        {
            var query = "DELETE FROM Sexo WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }

    
}
