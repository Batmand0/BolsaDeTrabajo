using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IRepositorioPerfil repositorioPerfil;

        public PerfilController(IRepositorioPerfil repositorioPerfil)
        {
            this.repositorioPerfil = repositorioPerfil;
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
            var perfil = await repositorioPerfil.GetPerfilByClave(Clave);

            if (perfil == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = perfil.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PerfilViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var perfilExistente = await repositorioPerfil.GetPerfilByClave(model.Clave);

                if (perfilExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioPerfil.InsertPerfil(new PerfilModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioPerfil.DeletePerfilByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var perfilExistente = await repositorioPerfil.GetPerfilByClave(model.Clave);
                if (perfilExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                perfilExistente.Descripcion = model.Descripcion;
                await repositorioPerfil.UpdatePerfil(perfilExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
