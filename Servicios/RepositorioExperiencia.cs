using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioExperiencia
    {
        Task<int> DeleteExperienciaByClave(int clave);
        Task<IEnumerable<ExperienciaModel>> GetAllExperiencias();
        Task<ExperienciaModel> GetExperienciaByClave(int clave);
        Task<int> InsertExperiencia(ExperienciaModel experienciaModel);
        Task<int> UpdateExperiencia(ExperienciaModel experiencialModel);
    }
    public class RepositorioExperiencia : IRepositorioExperiencia
    {
        private readonly DapperContext _context;

        public RepositorioExperiencia(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ExperienciaModel>> GetAllExperiencias()
        {
            var query = "SELECT * FROM Experiencia";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<ExperienciaModel>(query);
                return result;
            }
        }
        public async Task<ExperienciaModel> GetExperienciaByClave(int clave)
        {
            var query = "SELECT * FROM Experiencia WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<ExperienciaModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertExperiencia(ExperienciaModel experienciaModel)
        {
            var query = "INSERT INTO Experiencia (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { experienciaModel.Clave, experienciaModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateExperiencia(ExperienciaModel experiencialModel)
        {
            var query = "UPDATE Experiencia SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { experiencialModel.Descripcion, experiencialModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteExperienciaByClave(int clave)
        {
            var query = "DELETE FROM Experiencia WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
