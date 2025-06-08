using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioDocumento
    {
        Task<int> DeleteDocumentoByClave(int clave);
        Task<IEnumerable<DocumentoModel>> GetAllDocumentos();
        Task<DocumentoModel> GetDocumentoByClave(int clave);
        Task<List<DocumentoBolsaModel>> GetDocumentosByClaveBolsa(int claveBolsa);
        Task<int> InsertarDocumentoBolsa(DocumentoBolsaModel documentoBolsa);
        Task<int> InsertDocumento(DocumentoModel documentoModel);
        Task<int> UpdateDocumento(DocumentoModel documentoModel);
    }
    public class RepositorioDocumento : IRepositorioDocumento
    {
        private readonly DapperContext _context;

        public RepositorioDocumento(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DocumentoModel>> GetAllDocumentos()
        {
            var query = "SELECT * FROM Documento";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<DocumentoModel>(query);
            }
        }
        public async Task<DocumentoModel> GetDocumentoByClave(int clave)
        {
            var query = "SELECT * FROM Documento WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<DocumentoModel>(query, new { Clave = clave });
            }
        }
        // Insertar un nuevo producto
        public async Task<int> InsertDocumento(DocumentoModel documentoModel)
        {
            var query = "INSERT INTO Documento (Clave, Descripcion) VALUES (@Clave, @Descripcion)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { documentoModel.Clave, documentoModel.Descripcion });
                return result;
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateDocumento(DocumentoModel documentoModel)
        {
            var query = "UPDATE Documento SET Descripcion = @Descripcion WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { documentoModel.Descripcion, documentoModel.Clave });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteDocumentoByClave(int clave)
        {
            var query = "DELETE FROM Documento WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }

        public async Task<int> InsertarDocumentoBolsa(DocumentoBolsaModel documentoBolsa)
        {
            var query = "INSERT INTO DocumentoBolsa (ClaveBolsa, ClaveDocumento) VALUES (@ClaveBolsa, @ClaveDocumento)";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    documentoBolsa.ClaveBolsa,
                    documentoBolsa.ClaveDocumento
                });
                return result;
            }
        }

        public async Task<List<DocumentoBolsaModel>> GetDocumentosByClaveBolsa(int claveBolsa)
        {
            // Define la consulta SQL para obtener los documentos por clave de bolsa
            var query = "SELECT * FROM DocumentoBolsa WHERE ClaveBolsa = @ClaveBolsa";

            using (var connection = _context.CreateConnection())
            {
                // Ejecuta la consulta y mapea el resultado a una lista de DocumentoBolsaModel
                var documentos = await connection.QueryAsync<DocumentoBolsaModel>(query, new { ClaveBolsa = claveBolsa });
                return documentos.ToList(); // Devuelve la lista de documentos
            }
        }
    }
}
