using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BolsaDeTrabajo.Controllers
{
    public class MantenimientoRestringidoController : Controller
    {
        private readonly IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido;
        private readonly IRepositorioPuestos repositorioPuestos;
        private readonly IRepositorioDepartamento repositorioDepartamento;
        private readonly IRepositorioSexo repositorioSexo;

        public MantenimientoRestringidoController(IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido,
                                                  IRepositorioPuestos repositorioPuestos,
                                                  IRepositorioDepartamento repositorioDepartamento, 
                                                  IRepositorioSexo repositorioSexo)
        {
            this.repositorioMantenimientoRestringido = repositorioMantenimientoRestringido;
            this.repositorioPuestos = repositorioPuestos;
            this.repositorioDepartamento = repositorioDepartamento;
            this.repositorioSexo = repositorioSexo;
        }
        // GET: MantenimientoRestringidoController/Create
        public async Task<IActionResult> Crear()
        {
            var puestos = await repositorioPuestos.GetAllPuestos();
            var departamentos = await repositorioDepartamento.GetAllDepartamentos();
            var sexo = await repositorioSexo.GetAllSexos();

            var viewModel = new MantenimientoRestringidoViewModel
            {
                // Crear SelectList para Preparatorias
                Sexos = sexo.Select(s => new SelectListItem
                {
                    Value = s.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = s.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Departamentos = departamentos.Select(d => new SelectListItem
                {
                    Value = d.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = d.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Puestos = puestos.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = p.Descripcion
                })
            };
            return View(viewModel);
        }
        public async Task<IActionResult> GetRestringidoByClave(int Clave)
        {
            var empleado = await repositorioMantenimientoRestringido.GetRestringidoByClave(Clave);

            if (empleado != null)
            {
                return Json(empleado); // Devuelve la vacante en formato JSON
            }
            else
            {
                return NotFound(); // Devuelve un error 404 si no se encuentra la vacante
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MantenimientoRestringidoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors in the ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                // Recargar listas en caso de error de validación
                var puestos = await repositorioPuestos.GetAllPuestos();
                var departamentos = await repositorioDepartamento.GetAllDepartamentos();
                var sexos = await repositorioSexo.GetAllSexos();

                // Asigna las listas de preparatorias y carreras al ViewModel
                model.Puestos = puestos.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Departamentos = departamentos.Select(d => new SelectListItem
                {
                    Value = d.Clave.ToString(),
                    Text = d.Descripcion
                });
                model.Sexos = sexos.Select(s => new SelectListItem
                {
                    Value = s.Clave.ToString(),
                    Text = s.Descripcion
                });
                return View(model); // Devuelve la vista con errores de validación
            }
            if (action == "guardar")
            {
                // Lógica para Modificar
                var restringidoExistente = await repositorioMantenimientoRestringido.GetRestringidoByClave(model.Clave);
                if (restringidoExistente == null)
                {
                    return NotFound("Empleado no encontrado.");
                }

                restringidoExistente.Motivo = model.Motivo;

                await repositorioMantenimientoRestringido.UpdateRestringido(restringidoExistente);

                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
