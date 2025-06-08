using Microsoft.AspNetCore.Mvc.Rendering;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoRestringidoViewModel : MantenimientoRestringidoModel
    {
        public IEnumerable<SelectListItem> Puestos { get; set; }
        public IEnumerable<SelectListItem> Departamentos { get; set; }
        public IEnumerable<SelectListItem> Sexos { get; set; }
    }
}
