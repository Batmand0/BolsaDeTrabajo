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
    public class ReporteBolsaController : Controller
    {
        private readonly IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa;
        private readonly IRepositorioSexo repositorioSexo;
        private readonly IRepositorioNacionalidad repositorioNacionalidad;
        private readonly IRepositorioPerfil repositorioPerfil;
        private readonly IRepositorioExperiencia repositorioExperiencia;
        private readonly IRepositorioEscolaridad repositorioEscolaridad;
        private readonly IRepositorioEstadoCivil repositorioEstadoCivil;
        private readonly IRepositorioDocumento repositorioDocumento;
        private readonly IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado;

        public ReporteBolsaController(IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa,
                                            IRepositorioSexo repositorioSexo,
                                            IRepositorioNacionalidad repositorioNacionalidad,
                                            IRepositorioPerfil repositorioPerfil,
                                            IRepositorioExperiencia repositorioExperiencia,
                                            IRepositorioEscolaridad repositorioEscolaridad,
                                            IRepositorioEstadoCivil repositorioEstadoCivil,
                                            IRepositorioDocumento repositorioDocumento,
                                            IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado)
        {
            this.repositorioMantenimientoBolsa = repositorioMantenimientoBolsa;
            this.repositorioSexo = repositorioSexo;
            this.repositorioNacionalidad = repositorioNacionalidad;
            this.repositorioPerfil = repositorioPerfil;
            this.repositorioExperiencia = repositorioExperiencia;
            this.repositorioEscolaridad = repositorioEscolaridad;
            this.repositorioEstadoCivil = repositorioEstadoCivil;
            this.repositorioDocumento = repositorioDocumento;
            this.repositorioMantenimientoEmpleado = repositorioMantenimientoEmpleado;
        }

        // GET: ReporteBolsa/ReportesB
        public async Task<IActionResult> ReportesB(string sexo, string perfil, string nacionalidad, DateTime? startDate, DateTime? endDate, DateTime? startIngreso, DateTime? endIngreso)
        {
            // Obtén los datos desde la base de datos usando Dapper
            var vacantes = await repositorioMantenimientoBolsa.GetAllBolsa();

            // Aplica los filtros usando LINQ en memoria
            if (!string.IsNullOrEmpty(sexo))
            {
                vacantes = vacantes.Where(v => v.Sexo == sexo).ToList();
            }
            if (!string.IsNullOrEmpty(perfil))
            {
                vacantes = vacantes.Where(v => v.Perfil == perfil).ToList();
            }
            if (!string.IsNullOrEmpty(nacionalidad))
            {
                vacantes = vacantes.Where(v => v.Nacionalidad == nacionalidad).ToList();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                vacantes = vacantes.Where(v => v.FNacimiento >= startDate.Value && v.FNacimiento <= endDate.Value).ToList();
            }
            if (startIngreso.HasValue && endIngreso.HasValue)
            {
                vacantes = vacantes.Where(v => v.FIngreso >= startIngreso.Value && v.FIngreso <= endIngreso.Value).ToList();
            }

            // Prepara los datos para los filtros en la vista
            var nacionalidades = await repositorioNacionalidad.GetAllNacionalidades();
            var perfiles = await repositorioPerfil.GetAllPerfiles();
            var sexoList = await repositorioSexo.GetAllSexos();

            // Crea el ViewModel con los datos filtrados y las listas para los filtros
            var model = new MantenimientoBolsaViewModel
            {
                Nacionalidades = nacionalidades.Select(n => new SelectListItem
                {
                    Value = n.Clave.ToString(),
                    Text = n.Descripcion
                }),
                Perfiles = perfiles.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                }),
                Sexos = sexoList.Select(s => new SelectListItem
                {
                    Value = s.Clave.ToString(),
                    Text = s.Descripcion
                }),
                Vacantes = vacantes // Vacantes ya filtradas
            };

            // Devuelve la vista con el modelo
            return View(model);
        }
    }
}
