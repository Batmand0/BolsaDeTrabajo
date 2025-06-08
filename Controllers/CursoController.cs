using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class CursoController : Controller
    {
        private readonly IRepositorioCurso repositorioCurso;

        public CursoController(IRepositorioCurso repositorioCurso)
        {
            this.repositorioCurso = repositorioCurso;
        }
        // GET: CursoController
        public ActionResult Index()
        {
            return View();
        }
        // GET: CursoController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var curso = await repositorioCurso.GetCursoByClave(Clave);

            if (curso == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = curso.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CursoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var cursoExistente = await repositorioCurso.GetCursoByClave(model.Clave);

                if (cursoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioCurso.InsertCurso(new CursoModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioCurso.DeleteCursoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var cursoExistente = await repositorioCurso.GetCursoByClave(model.Clave);
                if (cursoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                cursoExistente.Descripcion = model.Descripcion;
                await repositorioCurso.UpdateCurso(cursoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
