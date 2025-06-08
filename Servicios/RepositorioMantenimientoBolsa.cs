using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioMantenimientoBolsa
    {
        Task<int> DeleteBolsaByClave(int clave);
        Task<IEnumerable<MantenimientoBolsaModel>> GetAllBolsa();
        Task<MantenimientoBolsaModel> GetBolsaByClave(int clave);
        Task<int> InsertBolsa(MantenimientoBolsaModel mantenimientoBolsaModel);
        Task<int> UpdateBolsa(MantenimientoBolsaModel mantenimientoBolsaModel);
    }
    public class RepositorioMantenimientoBolsa : IRepositorioMantenimientoBolsa
    {
        private readonly DapperContext _context;

        public RepositorioMantenimientoBolsa(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MantenimientoBolsaModel>> GetAllBolsa()
        {
            var query = "SELECT * FROM MantenimientoBolsa";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<MantenimientoBolsaModel>(query);
            }
        }
        public async Task<MantenimientoBolsaModel> GetBolsaByClave(int clave)
        {
            var query = "SELECT * FROM MantenimientoBolsa WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<MantenimientoBolsaModel>(query, new { Clave = clave });
            }
        }
        public async Task<int> InsertBolsa(MantenimientoBolsaModel mantenimientoBolsaModel)
        {
            var query = "INSERT INTO MantenimientoBolsa " +
                "(Clave, APaterno, AMaterno, Nombre, FNacimiento, FIngreso, Direccion, Correo, CURP, Estatus, Sexo, Nacionalidad, EstadoCivil, Experiencia, Perfil, Escolaridad, Documento, Observaciones, RFC, NSS) " +
                "OUTPUT INSERTED.Clave " +  // Aquí capturamos la clave insertada
                "VALUES (@Clave, @APaterno, @AMaterno, @Nombre, @FNacimiento, @FIngreso, @Direccion, @Correo, @CURP, @Estatus, @Sexo, @Nacionalidad, @EstadoCivil, @Experiencia, @Perfil, @Escolaridad, @Documento, @Observaciones, @RFC, @NSS);";

            using (var connection = _context.CreateConnection())
            {
                var claveInsertada = await connection.QuerySingleAsync<int>(query, new
                {
                    mantenimientoBolsaModel.Clave,
                    mantenimientoBolsaModel.APaterno,
                    mantenimientoBolsaModel.AMaterno,
                    mantenimientoBolsaModel.Nombre,
                    mantenimientoBolsaModel.FNacimiento,
                    mantenimientoBolsaModel.Direccion,
                    mantenimientoBolsaModel.Correo,
                    mantenimientoBolsaModel.CURP,
                    mantenimientoBolsaModel.Estatus,
                    mantenimientoBolsaModel.Sexo,
                    mantenimientoBolsaModel.Perfil,
                    mantenimientoBolsaModel.Documento,
                    mantenimientoBolsaModel.Nacionalidad,
                    mantenimientoBolsaModel.EstadoCivil,
                    mantenimientoBolsaModel.Experiencia,
                    mantenimientoBolsaModel.Escolaridad,
                    mantenimientoBolsaModel.Observaciones,
                    mantenimientoBolsaModel.FIngreso,
                    mantenimientoBolsaModel.RFC,
                    mantenimientoBolsaModel.NSS
                });

                return claveInsertada;  // Retorna la clave insertada
            }
        }

        // Actualizar un producto existente
        public async Task<int> UpdateBolsa(MantenimientoBolsaModel mantenimientoBolsaModel)
        {
            var query = "UPDATE MantenimientoBolsa " +
                        "SET APaterno = @APaterno, AMaterno = @AMaterno, Nombre = @Nombre, FNacimiento = @FNacimiento, " +
                        "Direccion = @Direccion, Correo = @Correo, CURP = @CURP, Estatus = @Estatus, " +
                        "Sexo = @Sexo, Nacionalidad = @Nacionalidad, EstadoCivil = @EstadoCivil, " +
                        "Experiencia = @Experiencia, Perfil = @Perfil, Observaciones = @Observaciones ," +
                        "RFC = @RFC, NSS = @NSS " +
                        "WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new {
                    mantenimientoBolsaModel.Clave,
                    mantenimientoBolsaModel.APaterno,
                    mantenimientoBolsaModel.AMaterno,
                    mantenimientoBolsaModel.Nombre,
                    mantenimientoBolsaModel.FNacimiento,
                    mantenimientoBolsaModel.FIngreso,
                    mantenimientoBolsaModel.Direccion,
                    mantenimientoBolsaModel.Correo,
                    mantenimientoBolsaModel.CURP,
                    mantenimientoBolsaModel.Estatus,
                    mantenimientoBolsaModel.Sexo,
                    mantenimientoBolsaModel.Nacionalidad,
                    mantenimientoBolsaModel.EstadoCivil,
                    mantenimientoBolsaModel.Perfil,
                    mantenimientoBolsaModel.Experiencia,
                    mantenimientoBolsaModel.Escolaridad,
                    mantenimientoBolsaModel.Observaciones,
                    mantenimientoBolsaModel.RFC,
                    mantenimientoBolsaModel.NSS
                });
                return result;
            }
        }

        // Eliminar un producto por Clave (que es también el Id)
        public async Task<int> DeleteBolsaByClave(int clave)
        {
            var query = "DELETE FROM MantenimientoBolsa WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }

    }

    
}
