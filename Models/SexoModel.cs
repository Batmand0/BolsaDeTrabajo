using System.ComponentModel.DataAnnotations;

namespace BolsaDeTrabajo.Models
{
    public class SexoModel
    {
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "La clave debe contener solo números.")]
        public int Clave { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
