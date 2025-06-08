using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class EscolaridadController : Controller
    {
        private readonly IRepositorioEscolaridad repositorioEscolaridad;

        public EscolaridadController(IRepositorioEscolaridad repositorioEscolaridad)
        {
            this.repositorioEscolaridad = repositorioEscolaridad;
        }
        // GET: EscolaridadController
        public ActionResult Index()
        {
            return View();
        }


        // GET: EscolaridadController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var escolaridad = await repositorioEscolaridad.GetEscolaridadByClave(Clave);

            if (escolaridad == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = escolaridad.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EscolaridadViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var escolaridadExistente = await repositorioEscolaridad.GetEscolaridadByClave(model.Clave);

                if (escolaridadExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioEscolaridad.InsertEscolaridad(new EscolaridadModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioEscolaridad.DeleteEscolaridadByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var documentoExistente = await repositorioEscolaridad.GetEscolaridadByClave(model.Clave);
                if (documentoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                documentoExistente.Descripcion = model.Descripcion;
                await repositorioEscolaridad.UpdateEscolaridad(documentoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
