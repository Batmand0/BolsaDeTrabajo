using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BolsaDeTrabajo.Controllers
{
    public class ReporteEmpleadoController : Controller
    {
        private readonly IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa;
        private readonly IRepositorioDepartamento repositorioDepartamento;
        private readonly IRepositorioPuestos repositorioPuestos;
        private readonly IRepositorioCurso repositorioCurso;
        private readonly IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado;

        public ReporteEmpleadoController(IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa,
                                            IRepositorioDepartamento repositorioDepartamento,
                                            IRepositorioPuestos repositorioPuestos,
                                            IRepositorioCurso repositorioCurso,
                                            IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado)
        {
            this.repositorioMantenimientoBolsa = repositorioMantenimientoBolsa;
            this.repositorioDepartamento = repositorioDepartamento;
            this.repositorioPuestos = repositorioPuestos;
            this.repositorioCurso = repositorioCurso;
            this.repositorioMantenimientoEmpleado = repositorioMantenimientoEmpleado;
        }

        // GET: ReporteBolsa/ReportesB
        public async Task<IActionResult> ReportesE(string puesto, string departamento, string curso, DateTime? startDate, DateTime? endDate, DateTime? startIngreso, DateTime? endIngreso)
        {
            // Obtén los datos desde la base de datos usando Dapper
            var empleados = await repositorioMantenimientoEmpleado.GetAllEmpleado();

            // Aplica los filtros usando LINQ en memoria
            if (!string.IsNullOrEmpty(curso))
            {
                empleados = empleados.Where(v => v.Curso == curso).ToList();
            }
            if (!string.IsNullOrEmpty(puesto))
            {
                empleados = empleados.Where(v => v.Puesto == puesto).ToList();
            }
            if (!string.IsNullOrEmpty(departamento))
            {
                empleados = empleados.Where(v => v.Departamento == departamento).ToList();
            }
            if (startIngreso.HasValue && endIngreso.HasValue)
            {
                empleados = empleados.Where(v => v.FIngreso >= startIngreso.Value && v.FIngreso <= endIngreso.Value).ToList();
            }

            // Prepara los datos para los filtros en la vista
            var departamentos = await repositorioDepartamento.GetAllDepartamentos();
            var puestos = await repositorioPuestos.GetAllPuestos();
            var cursos = await repositorioCurso.GetAllCursos();

            // Crea el ViewModel con los datos filtrados y las listas para los filtros
            var model = new MantenimientoEmpleadoViewModel
            {
                Departamentos = departamentos.Select(n => new SelectListItem
                {
                    Value = n.Clave.ToString(),
                    Text = n.Descripcion
                }),
                Puestos = puestos.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                }),
                Cursos = cursos.Select(s => new SelectListItem
                {
                    Value = s.Clave.ToString(),
                    Text = s.Descripcion
                }),
                Empleados = empleados    // Vacantes ya filtradas
            };

            // Devuelve la vista con el modelo
            return View(model);
        }
    }
}
