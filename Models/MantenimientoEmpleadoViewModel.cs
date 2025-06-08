using Microsoft.AspNetCore.Mvc.Rendering;

namespace BolsaDeTrabajo.Models
{
    public class MantenimientoEmpleadoViewModel : MantenimientoEmpleadoModel
    {
        public IEnumerable<SelectListItem> Puestos { get; set; }
        public IEnumerable<SelectListItem> Departamentos { get; set; }
        public IEnumerable<SelectListItem> Cursos { get; set; }
        public IEnumerable<MantenimientoEmpleadoModel> Empleados { get; set; }  // Aquí agregas la propiedad para las vacantes

    }
}
