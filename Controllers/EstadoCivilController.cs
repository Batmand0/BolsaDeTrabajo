using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class EstadoCivilController : Controller
    {
        private readonly IRepositorioEstadoCivil repositorioEstadoCivil;

        public EstadoCivilController(IRepositorioEstadoCivil repositorioEstadoCivil)
        {
            this.repositorioEstadoCivil = repositorioEstadoCivil;
        }
        // GET: EstadoCivilController
        public ActionResult Index()
        {
            return View();
        }


        // GET: EstadoCivilController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var estadoCivil = await repositorioEstadoCivil.GetEstadoCivilByClave(Clave);

            if (estadoCivil == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = estadoCivil.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EstadoCivilViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var estadoCivilExistente = await repositorioEstadoCivil.GetEstadoCivilByClave(model.Clave);

                if (estadoCivilExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioEstadoCivil.InsertEstadoCivil(new EstadoCivilModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioEstadoCivil.DeleteEstadoCivilByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var estadoCivilExistente = await repositorioEstadoCivil.GetEstadoCivilByClave(model.Clave);
                if (estadoCivilExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                estadoCivilExistente.Descripcion = model.Descripcion;
                await repositorioEstadoCivil.UpdateEstadoCivil(estadoCivilExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
