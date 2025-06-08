using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BolsaDeTrabajo.Controllers
{
    public class MantenimientoEmpleadoController : Controller
    {
        private readonly IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado;

        private readonly IRepositorioDepartamento repositorioDepartamento;
        private readonly IRepositorioPuestos repositorioPuestos;
        private readonly IRepositorioCurso repositorioCurso;
        private readonly IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa;
        private readonly IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido;

        public MantenimientoEmpleadoController(IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado,
                                            IRepositorioDepartamento repositorioDepartamento,
                                            IRepositorioPuestos repositorioPuestos,
                                            IRepositorioCurso repositorioCurso,
                                            IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa,
                                            IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido)
        {
            this.repositorioMantenimientoEmpleado = repositorioMantenimientoEmpleado;
            this.repositorioCurso = repositorioCurso;
            this.repositorioMantenimientoBolsa = repositorioMantenimientoBolsa;
            this.repositorioMantenimientoRestringido = repositorioMantenimientoRestringido;
            this.repositorioDepartamento = repositorioDepartamento;
            this.repositorioPuestos = repositorioPuestos;
        }

        // GET: MantenimientoBolsaController/Create
        public async Task<IActionResult> Crear()
        {
            var puestos = await repositorioPuestos.GetAllPuestos();
            var departamentos = await repositorioDepartamento.GetAllDepartamentos();
            var cursos = await repositorioCurso.GetAllCursos();

            var viewModel = new MantenimientoEmpleadoViewModel
            {
                // Crear SelectList para Preparatorias
                Cursos = cursos.Select(c => new SelectListItem
                {
                    Value = c.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = c.Descripcion
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

        public async Task<IActionResult> GetEmpleadoByClave(int Clave)
        {
            var empleado = await repositorioMantenimientoEmpleado.GetEmpleadoByClave(Clave);

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
        public async Task<IActionResult> Crear(MantenimientoEmpleadoViewModel model, string action)
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
                var cursos = await repositorioCurso.GetAllCursos();

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
                model.Cursos = cursos.Select(c => new SelectListItem
                {
                    Value = c.Clave.ToString(),
                    Text = c.Descripcion
                });
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un empleado con la misma clave
                var empleadoExistente = await repositorioMantenimientoEmpleado.GetEmpleadoByClave(model.Clave);

                // Verifica si ya existe una vacante en bolsa con la misma clave
                var vacanteExistente = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);

                if (vacanteExistente == null)
                {
                    ModelState.AddModelError(string.Empty, "No existe la vacante.");
                    return View(model); // Retorna la vista con el mensaje de error
                }

                if (empleadoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un empleado con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }


                // Inserta los datos de la bolsa
                var nuevaEmpleado = new MantenimientoEmpleadoModel
                {
                    Nombre = vacanteExistente.Nombre,
                    APaterno = vacanteExistente.APaterno,
                    AMaterno = vacanteExistente.AMaterno,
                    Correo = vacanteExistente.Correo,
                    FIngreso = DateTime.Now,
                    Clave = model.Clave,
                    Departamento = model.Departamento,
                    Puesto = model.Puesto,
                    ClaveVacante = model.Clave,
                    Curso = model.Curso
                };
                vacanteExistente.Estatus = "Empleado";
                await repositorioMantenimientoBolsa.UpdateBolsa(vacanteExistente);
                // Inserta la bolsa en la base de datos y recupera su ID
                var claveBolsa = await repositorioMantenimientoEmpleado.InsertEmpleado(nuevaEmpleado);
                return RedirectToAction("Crear");
            }
            else if (action == "restringir")
            {
                var infoBolsa = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);
                var infoEmpleado = await repositorioMantenimientoEmpleado.GetEmpleadoByClave(model.Clave);
                var nuevoRestringido = new MantenimientoRestringidoModel
                {
                    Nombre = infoBolsa.Nombre,
                    AMaterno = infoBolsa.AMaterno,
                    APaterno = infoBolsa.APaterno,
                    FNacimiento = infoBolsa.FNacimiento,
                    Sexo = infoBolsa.Sexo,
                    CURP = infoBolsa.CURP,
                    FIngresoE = infoEmpleado.FIngreso,
                    FIngresoM = DateTime.Now,
                    Clave = model.Clave,
                    Departamento = model.Departamento,
                    Puesto = model.Puesto
                };
                var ingresaRestringido = await repositorioMantenimientoRestringido.InsertRestringido(nuevoRestringido);
                // Lógica para Baja
                await repositorioMantenimientoBolsa.DeleteBolsaByClave(model.Clave);
                await repositorioMantenimientoEmpleado.DeleteEmpleadoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var empleadoExistente = await repositorioMantenimientoEmpleado.GetEmpleadoByClave(model.Clave);
                if (empleadoExistente == null)
                {
                    return NotFound("Empleado no encontrado.");
                }

                empleadoExistente.FIngreso = model.FIngreso;
                empleadoExistente.Departamento = model.Departamento;
                empleadoExistente.Puesto = model.Puesto;
                empleadoExistente.Curso = model.Curso;

                await repositorioMantenimientoEmpleado.UpdateEmpleado(empleadoExistente);

                return RedirectToAction("Crear");
            }
            else if (action == "bolsa")
            {
                // Lógica para Baja
                var infoBolsa = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);
                infoBolsa.Estatus = "Bolsa";
                await repositorioMantenimientoBolsa.UpdateBolsa(infoBolsa);
                await repositorioMantenimientoEmpleado.DeleteEmpleadoByClave(model.Clave);

                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
