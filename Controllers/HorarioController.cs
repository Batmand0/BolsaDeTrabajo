using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class HorarioController : Controller
    {
        private readonly IRepositorioHorario repositorioHorario;

        public HorarioController(IRepositorioHorario repositorioHorario)
        {
            this.repositorioHorario = repositorioHorario;
        }

        // GET: HorarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HorarioController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDatos([FromQuery] int Clave)
        {
            // Busca el horario por su clave
            var horario = await repositorioHorario.ObtenerHorarioPorClave(Clave);

            if (horario == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new
            {
                HoraEntrada = horario.HoraEntrada,
                HoraSalida = horario.HoraSalida,
                DiaSemana = horario.DiaSemana,
                HoraSemana = horario.HoraSemana1 });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(HorarioViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un horario con la misma clave
                var horarioExistente = await repositorioHorario.ObtenerHorarioPorClave(model.Clave);

                if (horarioExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un horario con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioHorario.InsertarHorario(new HorarioModel { Clave = model.Clave,
                                                                            HoraEntrada = model.HoraEntrada,
                                                                            HoraSalida = model.HoraSalida,
                                                                            DiaSemana = model.DiaSemana,
                                                                            HoraSemana1 = model.HoraSemana1 / 100
                });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioHorario.EliminarHorario(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var horarioExistente = await repositorioHorario.ObtenerHorarioPorClave(model.Clave);
                if (horarioExistente == null)
                {
                    return NotFound("Horario no encontrado.");
                }
                horarioExistente.HoraEntrada = model.HoraEntrada;
                horarioExistente.HoraSalida = model.HoraSalida;
                horarioExistente.DiaSemana = model.DiaSemana;
                horarioExistente.HoraEntrada = model.HoraEntrada;
                await repositorioHorario.ActualizarHorario(horarioExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
