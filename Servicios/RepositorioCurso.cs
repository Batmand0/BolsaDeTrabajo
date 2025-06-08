using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioCurso
    {
        Task<int> DeleteCursoByClave(int clave);
        Task<IEnumerable<CursoModel>> GetAllCursos();
        Task<CursoModel> GetCursoByClave(int clave);
        Task<int> InsertCurso(CursoModel cursoModel);
        Task<int> UpdateCurso(CursoModel cursoModel);
    }

    public class RepositorioCurso : IRepositorioCurso
    {
        private readonly DapperContext _context;

        public RepositorioCurso(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CursoModel>> GetAllCursos()
        {
            var query = "SELECT * FROM Curso";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<CursoModel>(query);
            }
        }
        public async Task<CursoModel> GetCursoByClave(int clave)
        {
            var query = "SELECT * FROM Curso WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<CursoModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertCurso(CursoModel cursoModel)
        {
            var query = "INSERT INTO Curso (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { cursoModel.Clave, cursoModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateCurso(CursoModel cursoModel)
        {
            var query = "UPDATE Curso SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { cursoModel.Descripcion, cursoModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteCursoByClave(int clave)
        {
            var query = "DELETE FROM Curso WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }
    }
}
