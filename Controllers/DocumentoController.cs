using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BolsaDeTrabajo.Controllers
{
    public class DocumentoController : Controller
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public DocumentoController(IRepositorioDocumento repositorioDocumento) 
        {
            this.repositorioDocumento = repositorioDocumento;
        }
        // GET: DocumentoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DocumentoController/Create
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDescripcion([FromQuery] int Clave)
        {
            // Busca el producto por su clave
            var documento = await repositorioDocumento.GetDocumentoByClave(Clave);

            if (documento == null)
            {
                return Json(null);
            }

            // Devuelve la descripción en formato JSON
            return Ok(new { descripcion = documento.Descripcion });
        }

        [HttpPost]
        public async Task<IActionResult> Crear(DocumentoViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe un producto con la misma clave
                var documentoExistente = await repositorioDocumento.GetDocumentoByClave(model.Clave);

                if (documentoExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Alta
                await repositorioDocumento.InsertDocumento(new DocumentoModel { Clave = model.Clave, Descripcion = model.Descripcion });
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                // Lógica para Baja
                await repositorioDocumento.DeleteDocumentoByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var documentoExistente = await repositorioDocumento.GetDocumentoByClave(model.Clave);
                if (documentoExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }
                documentoExistente.Descripcion = model.Descripcion;
                await repositorioDocumento.UpdateDocumento(documentoExistente);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
