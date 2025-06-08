using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioMantenimientoRestringido
    {
        Task<IEnumerable<MantenimientoRestringidoModel>> GetAllRestringido();
        Task<MantenimientoRestringidoModel> GetRestringidoByClave(int clave);
        Task<int> InsertRestringido(MantenimientoRestringidoModel mantenimientoRestringidoModel);
        Task<int> UpdateRestringido(MantenimientoRestringidoModel mantenimientoRestringidoModel);
    }
    public class RepositorioMantenimientoRestringido : IRepositorioMantenimientoRestringido
    {
        private readonly DapperContext _context;

        public RepositorioMantenimientoRestringido(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MantenimientoRestringidoModel>> GetAllRestringido()
        {
            var query = "SELECT * FROM MantenimientoRestringido";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<MantenimientoRestringidoModel>(query);
            }
        }
        public async Task<MantenimientoRestringidoModel> GetRestringidoByClave(int clave)
        {
            var query = "SELECT * FROM MantenimientoRestringido WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<MantenimientoRestringidoModel>(query, new { Clave = clave });
            }
        }
        public async Task<int> InsertRestringido(MantenimientoRestringidoModel mantenimientoRestringidoModel)
        {
            var query = "INSERT INTO MantenimientoRestringido " +
                "(Clave, Motivo, Departamento, Puesto, FIngresoE, FIngresoM, Nombre, APaterno, AMaterno, FNacimiento, Sexo) " +
                "OUTPUT INSERTED.Clave " +  // Aquí capturamos la clave insertada
                "VALUES (@Clave, @Motivo, @Departamento, @Puesto, @FIngresoE, @FIngresoM, @Nombre, @APaterno, @AMaterno, @FNacimiento, @Sexo);";

            using (var connection = _context.CreateConnection())
            {
                var claveInsertada = await connection.QuerySingleAsync<int>(query, new
                {
                    mantenimientoRestringidoModel.Clave,
                    mantenimientoRestringidoModel.Motivo,
                    mantenimientoRestringidoModel.Departamento,
                    mantenimientoRestringidoModel.Puesto,
                    mantenimientoRestringidoModel.FIngresoE,
                    mantenimientoRestringidoModel.FIngresoM,
                    mantenimientoRestringidoModel.Nombre,
                    mantenimientoRestringidoModel.APaterno,
                    mantenimientoRestringidoModel.AMaterno,
                    mantenimientoRestringidoModel.FNacimiento,
                    mantenimientoRestringidoModel.Sexo
                });

                return claveInsertada;  // Retorna la clave insertada
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateRestringido(MantenimientoRestringidoModel mantenimientoRestringidoModel)
        {
            var query = "UPDATE MantenimientoRestringido " +
                        "SET Motivo = @Motivo " +
                        "WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    mantenimientoRestringidoModel.Motivo,
                    mantenimientoRestringidoModel.Clave

                });
                return result;
            }
        }
    }
}
