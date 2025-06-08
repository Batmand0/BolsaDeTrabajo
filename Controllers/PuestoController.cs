using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class PuestoController : Controller
    {
        private readonly IRepositorioPuestos repositorioPuestos;

        public PuestoController(IRepositorioPuestos repositorioPuestos)
        {
            this.repositorioPuestos = repositorioPuestos;
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
            var puesto = await repositorioPuestos.GetPuestoByClave(Clave);

            if (puesto == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = puesto.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PuestoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var puestoExistente = await repositorioPuestos.GetPuestoByClave(model.Clave);

                if (puestoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioPuestos.InsertPuesto(new PuestoModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioPuestos.DeletePuestoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var puestoExistente = await repositorioPuestos.GetPuestoByClave(model.Clave);
                if (puestoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                puestoExistente.Descripcion = model.Descripcion;
                await repositorioPuestos.UpdatePuesto(puestoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
