using BolsaDeTrabajo.Models;
using Dapper;

namespace BolsaDeTrabajo.Servicios
{
    public interface IRepositorioHorario
    {
        Task<int> ActualizarHorario(HorarioModel horario);
        Task<int> EliminarHorario(int clave);
        Task<int> InsertarHorario(HorarioModel horario);
        Task<HorarioModel> ObtenerHorarioPorClave(int clave);
    }
    public class RepositorioHorario : IRepositorioHorario
    {
        private readonly DapperContext _context;

        public RepositorioHorario(DapperContext context)
        {
            _context = context;
        }

        // Insertar un nuevo horario
        public async Task<int> InsertarHorario(HorarioModel horario)
        {
            var query = @"INSERT INTO Horario (Clave, HoraEntrada, HoraSalida, DiaSemana, HoraSemana1) 
                      VALUES (@Clave, @HoraEntrada, @HoraSalida, @DiaSemana, @HoraSemana1)";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    horario.Clave,
                    horario.HoraEntrada,
                    horario.HoraSalida,
                    horario.DiaSemana,
                    horario.HoraSemana1
                });
                return result;
            }
        }

        // Actualizar un horario existente
        public async Task<int> ActualizarHorario(HorarioModel horario)
        {
            var query = @"UPDATE Horario 
                      SET HoraEntrada = @HoraEntrada, HoraSalida = @HoraSalida, DiaSemana = @DiaSemana, HoraSemana1 = @HoraSemana1
                      WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new
                {
                    horario.HoraEntrada,
                    horario.HoraSalida,
                    horario.DiaSemana,
                    horario.HoraSemana1,
                    horario.Clave
                });
                return result;
            }
        }

        // Eliminar un horario por Clave
        public async Task<int> EliminarHorario(int clave)
        {
            var query = "DELETE FROM Horario WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Clave = clave });
                return result;
            }
        }

        // Obtener un horario por Clave
        public async Task<HorarioModel> ObtenerHorarioPorClave(int clave)
        {
            var query = "SELECT * FROM Horario WHERE Clave = @Clave";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<HorarioModel>(query, new { Clave = clave });
            }
        }
    }

    
}
