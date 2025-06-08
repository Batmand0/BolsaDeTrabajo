using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioDepartamento
    {
        Task<int> DeleteDepartamentoByClave(int clave);
        Task<IEnumerable<DepartamentoModel>> GetAllDepartamentos();
        Task<DepartamentoModel> GetDepartamentoByClave(int clave);
        Task<int> InsertDepartamento(DepartamentoModel departamentoModel);
        Task<int> UpdateDepartamento(DepartamentoModel departamentoModel);
    }
    public class RepositorioDepartamento : IRepositorioDepartamento
    {
        private readonly DapperContext _context;

        public RepositorioDepartamento(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DepartamentoModel>> GetAllDepartamentos()
        {
            var query = "SELECT * FROM Departamento";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<DepartamentoModel>(query);
            }
        }
        public async Task<DepartamentoModel> GetDepartamentoByClave(int clave)
        {
            var query = "SELECT * FROM Departamento WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<DepartamentoModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertDepartamento(DepartamentoModel departamentoModel)
        {
            var query = "INSERT INTO Departamento (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { departamentoModel.Clave, departamentoModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateDepartamento(DepartamentoModel departamentoModel)
        {
            var query = "UPDATE Departamento SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { departamentoModel.Descripcion, departamentoModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteDepartamentoByClave(int clave)
        {
            var query = "DELETE FROM Departamento WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
