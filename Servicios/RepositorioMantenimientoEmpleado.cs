using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioMantenimientoEmpleado
    {
        Task<int> DeleteEmpleadoByClave(int clave);
        Task<IEnumerable<MantenimientoEmpleadoModel>> GetAllEmpleado();
        Task<MantenimientoEmpleadoModel> GetEmpleadoByClave(int clave);
        Task<int> InsertEmpleado(MantenimientoEmpleadoModel mantenimientoEmpleadoModel);
        Task<int> UpdateEmpleado(MantenimientoEmpleadoModel mantenimientoEmpleadoModel);
    }
    public class RepositorioMantenimientoEmpleado : IRepositorioMantenimientoEmpleado
    {
        private readonly DapperContext _context;

        public RepositorioMantenimientoEmpleado(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MantenimientoEmpleadoModel>> GetAllEmpleado()
        {
            var query = "SELECT * FROM MantenimientoEmpleado";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<MantenimientoEmpleadoModel>(query);
            }
        }
        public async Task<MantenimientoEmpleadoModel> GetEmpleadoByClave(int clave)
        {
            var query = "SELECT * FROM MantenimientoEmpleado WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<MantenimientoEmpleadoModel>(query, new { Clave = clave });
            }
        }
        public async Task<int> InsertEmpleado(MantenimientoEmpleadoModel mantenimientoEmpleadoModel)
        {
            var query = "INSERT INTO MantenimientoEmpleado " +
                "(Clave, Nombre, APaterno, AMaterno, FIngreso, Departamento, Puesto, Curso, ClaveVacante, Correo) " +
                "OUTPUT INSERTED.Clave " +  // Aquí capturamos la clave insertada
                "VALUES (@Clave, @Nombre, @APaterno, @AMaterno, @FIngreso, @Departamento, @Puesto, @Curso, @ClaveVacante, @Correo);";

            using (var connection = _context.CreateConnection())
            {
                var claveInsertada = await connection.QuerySingleAsync<int>(query, new
                {
                    mantenimientoEmpleadoModel.AMaterno,
                    mantenimientoEmpleadoModel.Nombre,
                    mantenimientoEmpleadoModel.APaterno,
                    mantenimientoEmpleadoModel.Correo,
                    mantenimientoEmpleadoModel.Clave,
                    mantenimientoEmpleadoModel.FIngreso,
                    mantenimientoEmpleadoModel.Departamento,
                    mantenimientoEmpleadoModel.Puesto,
                    mantenimientoEmpleadoModel.Curso,
                    mantenimientoEmpleadoModel.ClaveVacante
                });

                return claveInsertada;  // Retorna la clave insertada
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateEmpleado(MantenimientoEmpleadoModel mantenimientoEmpleadoModel)
        {
            var query = "UPDATE MantenimientoEmpleado " +
                        "SET FIngreso = @FIngreso, Departamento = @Departamento, Puesto = @Puesto, Curso = @Curso " +
                        "WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    mantenimientoEmpleadoModel.FIngreso,
                    mantenimientoEmpleadoModel.Clave,
                    mantenimientoEmpleadoModel.Departamento,
                    mantenimientoEmpleadoModel.Puesto,
                    mantenimientoEmpleadoModel.Curso

                });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteEmpleadoByClave(int clave)
        {
            var query = "DELETE FROM MantenimientoEmpleado WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
