using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly IRepositorioDepartamento repositorioDepartamento;

        public DepartamentoController(IRepositorioDepartamento repositorioDepartamento) 
        {
            this.repositorioDepartamento = repositorioDepartamento;
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
            var departamento = await repositorioDepartamento.GetDepartamentoByClave(Clave);

            if (departamento == null)
            {
                return Json(null); 
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = departamento.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(DepartamentoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var departamentoExistente = await repositorioDepartamento.GetDepartamentoByClave(model.Clave);

                if (departamentoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioDepartamento.InsertDepartamento(new DepartamentoModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioDepartamento.DeleteDepartamentoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var productoExistente = await repositorioDepartamento.GetDepartamentoByClave(model.Clave);
                if (productoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                productoExistente.Descripcion = model.Descripcion;
                await repositorioDepartamento.UpdateDepartamento(productoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }

    }
}
