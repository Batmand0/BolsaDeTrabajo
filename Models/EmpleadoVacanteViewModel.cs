using Microsoft.AspNetCore.Mvc.Rendering;

namespace BolsaDeTrabajo.Models
{
    public class EmpleadoVacanteViewModel
    {
        // Propiedades de la Tabla
        public int Clave { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public string Correo { get; set; }
        public string Observaciones { get; set; }

        // Filtros
        public string SelectedPuesto { get; set; }
        public string SelectedDepartamento { get; set; }
        public string SelectedCurso { get; set; }
        public DateTime? StartDate { get; set; } // Fecha de nacimiento "Desde"
        public DateTime? EndDate { get; set; }   // Fecha de nacimiento "Hasta"
        public DateTime? StartIngreso { get; set; } // Fecha de ingreso "Desde"
        public DateTime? EndIngreso { get; set; }   // Fecha de ingreso "Hasta"

        // Listas para los Selects
        public IEnumerable<SelectListItem> Puestos { get; set; }
        public IEnumerable<SelectListItem> Departamentos { get; set; }
        public IEnumerable<SelectListItem> Cursos { get; set; }

        // Resultados
        public IEnumerable<MantenimientoEmpleadoModel> Empleados { get; set; }
    }
}
