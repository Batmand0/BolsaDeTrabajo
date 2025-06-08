using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoBolsaModel
    {
        [Required]
        public int Clave { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string APaterno { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string AMaterno { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nombre { get; set; }
        [Required]
        public DateTime FNacimiento { get; set; }
        public DateTime FIngreso { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [RegularExpression(@"^[A-ZÑ]{4}\d{6}[HM][A-Z]{2}[A-Z]{3}[A-Z\d]{1}\d{1}$", ErrorMessage = "El {0} no tiene un formato válido.")]
        public string CURP { get; set; }
        public string Estatus { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Nacionalidad { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string EstadoCivil { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Escolaridad { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string  Experiencia { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [RegularExpression(@"^[A-ZÑ&]{3,4}\d{6}[A-Z\d]{3}$", ErrorMessage = "El {0} no tiene un formato válido.")]
        public string RFC { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "El {0} debe tener 11 dígitos.")]
        public string NSS { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Perfil { get; set; }
        public string Documento { get; set; }
        public string Observaciones { get; set; }
        
    }
}
