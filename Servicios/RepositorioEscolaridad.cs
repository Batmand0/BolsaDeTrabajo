using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioEscolaridad
    {
        Task<int> DeleteEscolaridadByClave(int clave);
        Task<IEnumerable<EscolaridadModel>> GetAllEscolaridades();
        Task<EscolaridadModel> GetEscolaridadByClave(int clave);
        Task<int> InsertEscolaridad(EscolaridadModel escolaridadModel);
        Task<int> UpdateEscolaridad(EscolaridadModel escolaridadModel);
    }
    public class RepositorioEscolaridad : IRepositorioEscolaridad
    {
        private readonly DapperContext _context;

        public RepositorioEscolaridad(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EscolaridadModel>> GetAllEscolaridades()
        {
            var query = "SELECT * FROM Escolaridad";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<EscolaridadModel>(query);
                return result;
            }
        }

        public async Task<EscolaridadModel> GetEscolaridadByClave(int clave)
        {
            var query = "SELECT * FROM Escolaridad WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<EscolaridadModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertEscolaridad(EscolaridadModel escolaridadModel)
        {
            var query = "INSERT INTO Escolaridad (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { escolaridadModel.Clave, escolaridadModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateEscolaridad(EscolaridadModel escolaridadModel)
        {
            var query = "UPDATE Escolaridad SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { escolaridadModel.Descripcion, escolaridadModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteEscolaridadByClave(int clave)
        {
            var query = "DELETE FROM Escolaridad WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
