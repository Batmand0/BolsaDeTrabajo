using System.ComponentModel.DataAnnotations;

namespace BolsaDeTrabajo.Models
{
    public class HorarioModel
    {
        [Required(ErrorMessage = "El campo Clave es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "La clave debe contener solo números.")]
        public int Clave { get; set; }
        [Required(ErrorMessage = "El campo Hora de entrada es requerido")]
        [Display(Name = "Hora de entrada")]
        public string HoraEntrada { get; set; }
        [Required(ErrorMessage = "El campo Hora de salida es requerido")]
        [Display(Name = "Hora de salida")]
        public string HoraSalida { get; set; }
        [Required(ErrorMessage = "El campo Dias a la semana es requerido")]
        [Range(minimum: 1, maximum: 6, ErrorMessage = "Las días deben de ser entre 1 a 6")]
        [Display(Name = "Dias a la semana")]
        public int DiaSemana { get; set; }
        [Range(minimum: 100, maximum: 4000, ErrorMessage = "Las horas deben de ser entre 1 a 40")]
        [Display(Name = "Horas a la semana")]
        public decimal HoraSemana1 {  get; set; }

    }
}
