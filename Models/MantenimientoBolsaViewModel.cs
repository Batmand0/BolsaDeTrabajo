using Microsoft.AspNetCore.Mvc.Rendering;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoBolsaViewModel : MantenimientoBolsaModel
    {
        public IEnumerable<SelectListItem> Escolaridades { get; set; }
        public IEnumerable<SelectListItem> Nacionalidades { get; set; }
        public IEnumerable<SelectListItem> Sexos { get; set; }
        public IEnumerable<SelectListItem> Perfiles { get; set; }
        public IEnumerable<SelectListItem> Experiencias { get; set; }
        public IEnumerable<SelectListItem> EstadoCiviles { get; set; }
        public IEnumerable<SelectListItem> Documentos { get; set; }
        public IEnumerable<MantenimientoBolsaModel> Vacantes { get; set; }  // Aquí agregas la propiedad para las vacantes
    }
}
