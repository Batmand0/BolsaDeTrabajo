using System.ComponentModel.DataAnnotations;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoEmpleadoModel
    {
        [Required]
        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public string Correo {get; set;}
        [Required]
        public string Puesto { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public DateTime FIngreso { get; set; }
        public string Curso { get; set; }
        public int ClaveVacante { get; set; }
        public string Horario { get; set; }
    }
}
