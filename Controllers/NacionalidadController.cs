using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class NacionalidadController : Controller
    {
        private readonly IRepositorioNacionalidad repositorioNacionalidad;

        public NacionalidadController(IRepositorioNacionalidad repositorioNacionalidad)
        {
            this.repositorioNacionalidad = repositorioNacionalidad;
        }
        // GET: NacionalidadController
        public ActionResult Index()
        {
            return View();
        }

        // GET: NacionalidadController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var nacionalidad = await repositorioNacionalidad.GetNacionalidadByClave(Clave);

            if (nacionalidad == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = nacionalidad.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(NacionalidadViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var nacionalidadExistente = await repositorioNacionalidad.GetNacionalidadByClave(model.Clave);

                if (nacionalidadExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioNacionalidad.InsertNacionalidad(new NacionalidadModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioNacionalidad.DeleteNacionalidadByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var nacionalidadExistente = await repositorioNacionalidad.GetNacionalidadByClave(model.Clave);
                if (nacionalidadExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                nacionalidadExistente.Descripcion = model.Descripcion;
                await repositorioNacionalidad.UpdateNacionalidad(nacionalidadExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
