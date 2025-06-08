using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioEstadoCivil
    {
        Task<int> DeleteEstadoCivilByClave(int clave);
        Task<IEnumerable<EstadoCivilModel>> GetAllEstadosCivil();
        Task<EstadoCivilModel> GetEstadoCivilByClave(int clave);
        Task<int> InsertEstadoCivil(EstadoCivilModel estadoCivilModel);
        Task<int> UpdateEstadoCivil(EstadoCivilModel estadoCivilModel);
    }
    public class RepositorioEstadoCivil : IRepositorioEstadoCivil
    {
        private readonly DapperContext _context;

        public RepositorioEstadoCivil(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EstadoCivilModel>> GetAllEstadosCivil()
        {
            var query = "SELECT * FROM EstadoCivil";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<EstadoCivilModel>(query);
                return result;
            }
        }
        public async Task<EstadoCivilModel> GetEstadoCivilByClave(int clave)
        {
            var query = "SELECT * FROM EstadoCivil WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<EstadoCivilModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertEstadoCivil(EstadoCivilModel estadoCivilModel)
        {
            var query = "INSERT INTO EstadoCivil (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { estadoCivilModel.Clave, estadoCivilModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateEstadoCivil(EstadoCivilModel estadoCivilModel)
        {
            var query = "UPDATE EstadoCivil SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { estadoCivilModel.Descripcion, estadoCivilModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteEstadoCivilByClave(int clave)
        {
            var query = "DELETE FROM EstadoCivil WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
