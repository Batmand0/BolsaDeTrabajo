using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class ExperienciaController : Controller
    {
        private readonly IRepositorioExperiencia repositorioExperiencia;

        public ExperienciaController(IRepositorioExperiencia repositorioExperiencia)
        {
            this.repositorioExperiencia = repositorioExperiencia;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var experiencia = await repositorioExperiencia.GetExperienciaByClave(Clave);

            if (experiencia == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = experiencia.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ExperienciaViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var experienciaExistente = await repositorioExperiencia.GetExperienciaByClave(model.Clave);

                if (experienciaExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioExperiencia.InsertExperiencia(new ExperienciaModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioExperiencia.DeleteExperienciaByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var experienciaExistente = await repositorioExperiencia.GetExperienciaByClave(model.Clave);
                if (experienciaExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                experienciaExistente.Descripcion = model.Descripcion;
                await repositorioExperiencia.UpdateExperiencia(experienciaExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
