using System.ComponentModel.DataAnnotations;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoRestringidoModel
    {
        public int Clave { get; set; }
        public string Motivo { get; set; }
        public string Departamento { get; set; }
        public DateTime FIngresoE { get; set; }
        public DateTime FIngresoM { get; set; }
        public string Puesto { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public string CURP { get; set; }
        public string Sexo { get; set; }
        public DateTime FNacimiento { get; set; }
    }
}
