using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioPuestos
    {
        Task<int> DeletePuestoByClave(int clave);
        Task<IEnumerable<PuestoModel>> GetAllPuestos();
        Task<PuestoModel> GetPuestoByClave(int clave);
        Task<int> InsertPuesto(PuestoModel puestoModel);
        Task<int> UpdatePuesto(PuestoModel puestoModel);
    }
    public class RepositorioPuestos : IRepositorioPuestos
    {
        private readonly DapperContext _context;

        public RepositorioPuestos(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PuestoModel>> GetAllPuestos()
        {
            var query = "SELECT * FROM Puesto";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<PuestoModel>(query);
            }
        }
        public async Task<PuestoModel> GetPuestoByClave(int clave)
        {
            var query = "SELECT * FROM Puesto WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<PuestoModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertPuesto(PuestoModel puestoModel)
        {
            var query = "INSERT INTO Puesto (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { puestoModel.Clave, puestoModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdatePuesto(PuestoModel puestoModel)
        {
            var query = "UPDATE Puesto SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { puestoModel.Descripcion, puestoModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeletePuestoByClave(int clave)
        {
            var query = "DELETE FROM Puesto WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
