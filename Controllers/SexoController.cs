using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class SexoController : Controller
    {
        private readonly IRepositorioSexo repositorioSexo;

        public SexoController(IRepositorioSexo repositorioSexo)
        {
            this.repositorioSexo = repositorioSexo;
        }
        // GET: SexoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SexoController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var sexo = await repositorioSexo.GetSexoByClave(Clave);

            if (sexo == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = sexo.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SexoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var sexoExistente = await repositorioSexo.GetSexoByClave(model.Clave);

                if (sexoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioSexo.InsertSexo(new SexoModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioSexo.DeleteSexoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var sexoExistente = await repositorioSexo.GetSexoByClave(model.Clave);
                if (sexoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                sexoExistente.Descripcion = model.Descripcion;
                await repositorioSexo.UpdateSexo(sexoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
