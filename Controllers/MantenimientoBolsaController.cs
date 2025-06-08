using BolsaDeTrabajo.Models;
using BolsaDeTrabajo.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace BolsaDeTrabajo.Controllers
{
    public class MantenimientoBolsaController : Controller
    {
        private readonly IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa;

        private readonly IRepositorioSexo repositorioSexo;
        private readonly IRepositorioPerfil repositorioPerfil;
        private readonly IRepositorioEscolaridad repositorioEscolaridad;
        private readonly IRepositorioEstadoCivil repositorioEstadoCivil;
        private readonly IRepositorioDocumento repositorioDocumento;
        private readonly IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado;
        private readonly IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido;
        private readonly IRepositorioNacionalidad repositorioNacionalidad;
        private readonly IRepositorioExperiencia repositorioExperiencia;

        public MantenimientoBolsaController(IRepositorioMantenimientoBolsa repositorioMantenimientoBolsa,
                                            IRepositorioSexo repositorioSexo,
                                            IRepositorioNacionalidad repositorioNacionalidad,
                                            IRepositorioPerfil repositorioPerfil,
                                            IRepositorioExperiencia repositorioExperiencia,
                                            IRepositorioEscolaridad repositorioEscolaridad,
                                            IRepositorioEstadoCivil repositorioEstadoCivil,
                                            IRepositorioDocumento repositorioDocumento,
                                            IRepositorioMantenimientoEmpleado repositorioMantenimientoEmpleado,
                                            IRepositorioMantenimientoRestringido repositorioMantenimientoRestringido)
        {
            this.repositorioMantenimientoBolsa = repositorioMantenimientoBolsa;
            this.repositorioSexo = repositorioSexo;
            this.repositorioNacionalidad = repositorioNacionalidad;
            this.repositorioPerfil = repositorioPerfil;
            this.repositorioExperiencia = repositorioExperiencia;
            this.repositorioEscolaridad = repositorioEscolaridad;
            this.repositorioEstadoCivil = repositorioEstadoCivil;
            this.repositorioDocumento = repositorioDocumento;
            this.repositorioMantenimientoEmpleado = repositorioMantenimientoEmpleado;
            this.repositorioMantenimientoRestringido = repositorioMantenimientoRestringido;
        }
        // GET: MantenimientoBolsaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MantenimientoBolsaController/Create
        public async Task<IActionResult> Crear()
        {
            var nacionalidades = await repositorioNacionalidad.GetAllNacionalidades();
            var experiencias = await repositorioExperiencia.GetAllExperiencias();
            var sexo = await repositorioSexo.GetAllSexos();
            var escolaridades = await repositorioEscolaridad.GetAllEscolaridades();
            var estadoscivil = await repositorioEstadoCivil.GetAllEstadosCivil();
            var perfiles = await repositorioPerfil.GetAllPerfiles();
            var documentos = await repositorioDocumento.GetAllDocumentos();

            var viewModel = new MantenimientoBolsaViewModel
            {
                // Crear SelectList para Preparatorias
                Nacionalidades = nacionalidades.Select(n => new SelectListItem
                {
                    Value = n.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = n.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Documentos = documentos.Select(n => new SelectListItem
                {
                    Value = n.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = n.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Experiencias = experiencias.Select(e => new SelectListItem
                {
                    Value = e.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = e.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Sexos = sexo.Select(s => new SelectListItem
                {
                    Value = s.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = s.Descripcion
                }),
                // Crear SelectList para Preparatorias
                Escolaridades = escolaridades.Select(e => new SelectListItem
                {
                    Value = e.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = e.Descripcion
                }),
                // Crear SelectList para Preparatorias
                EstadoCiviles = estadoscivil.Select(e => new SelectListItem
                {
                    Value = e.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = e.Descripcion
                }),
                Perfiles = perfiles.Select(e => new SelectListItem
                {
                    Value = e.Clave.ToString(), // Asegúrate de usar la llave primaria adecuada
                    Text = e.Descripcion
                })

            };
            return View(viewModel);
        }
        
        public async Task<IActionResult> GetBolsaByClave(int Clave)
        {
            var vacante = await repositorioMantenimientoBolsa.GetBolsaByClave(Clave);
            if (vacante != null)
            {
                return Json(vacante); // Devuelve la vacante en formato JSON
            }
            else
            {
                return NotFound(); // Devuelve un error 404 si no se encuentra la vacante
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MantenimientoBolsaViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                // Log the errors in the ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                
                // Recargar listas en caso de error de validación
                var nacionalidades = await repositorioNacionalidad.GetAllNacionalidades();
                var experiencias = await repositorioExperiencia.GetAllExperiencias();
                var sexo = await repositorioSexo.GetAllSexos();
                var escolaridades = await repositorioEscolaridad.GetAllEscolaridades();
                var estadoscivil = await repositorioEstadoCivil.GetAllEstadosCivil();
                var perfiles = await repositorioPerfil.GetAllPerfiles();
                var documentos = await repositorioDocumento.GetAllDocumentos();

                // Asigna las listas de preparatorias y carreras al ViewModel
                model.Nacionalidades = nacionalidades.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Experiencias = experiencias.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Documentos = documentos.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Escolaridades = escolaridades.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.EstadoCiviles = estadoscivil.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Sexos = sexo.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                model.Perfiles = perfiles.Select(p => new SelectListItem
                {
                    Value = p.Clave.ToString(),
                    Text = p.Descripcion
                });
                return View(model); // Devuelve la vista con errores de validación
            }

            if (action == "alta")
            {
                // Verifica si ya existe una vacante con la misma clave
                var bolsaExistente = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);

                // Verifica si ya existe un restringido con el misma CURP
                var EsRestringido = await repositorioMantenimientoRestringido.GetRestringidoByClave(model.Clave);


                if (bolsaExistente != null)
                {
                    ModelState.AddModelError(string.Empty, "Ya existe una vacante con esta clave.");
                    return View(model); // Retorna la vista con el mensaje de error
                }

                // Verifica si EsRestringido no es null antes de acceder a CURP
                if (EsRestringido != null && model.CURP == EsRestringido.CURP)
                {
                    ModelState.AddModelError(string.Empty, "Persona restringida.");
                    return View(model); // Retorna la vista con el mensaje de error
                }

                if (model.Clave == 0)
                {
                    ModelState.AddModelError(string.Empty, "La clave de la vacante no debe de ser 0");
                    return View(model); // Retorna la vista con el mensaje de error
                }

                // Inserta los datos de la bolsa
                var nuevaBolsa = new MantenimientoBolsaModel
                {
                    Clave = model.Clave,
                    APaterno = model.APaterno,
                    AMaterno = model.AMaterno,
                    Nombre = model.Nombre,
                    Escolaridad = model.Escolaridad,
                    EstadoCivil = model.EstadoCivil,
                    Experiencia = model.Experiencia,
                    Nacionalidad = model.Nacionalidad,
                    Documento = model.Documento,
                    Perfil = model.Perfil,
                    Observaciones = model.Observaciones,
                    Direccion = model.Direccion,
                    Sexo = model.Sexo,
                    CURP = model.CURP,
                    RFC = model.RFC,
                    NSS = model.NSS,
                    Estatus = "Bolsa",
                    FNacimiento = model.FNacimiento,
                    Correo = model.Correo,
                    FIngreso = DateTime.Now,
                    
                };

                // Inserta la bolsa en la base de datos y recupera su ID
                var claveBolsa = await repositorioMantenimientoBolsa.InsertBolsa(nuevaBolsa);
                return RedirectToAction("Crear");
            }
            else if (action == "baja")
            {
                if(model.Estatus == "Empleado")
                {
                    ModelState.AddModelError(string.Empty, "No se puede dar de baja a una vacante ya contratada.");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                // Lógica para Baja
                await repositorioMantenimientoBolsa.DeleteBolsaByClave(model.Clave);
                return RedirectToAction("Crear");
            }
            else if (action == "modificar")
            {
                // Lógica para Modificar
                var bolsaExistente = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);
                if (bolsaExistente == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                bolsaExistente.APaterno = model.APaterno;
                bolsaExistente.AMaterno = model.AMaterno;
                bolsaExistente.Nombre = model.Nombre;
                bolsaExistente.Correo = model.Correo;
                bolsaExistente.Perfil = model.Perfil;
                bolsaExistente.CURP = model.CURP;
                bolsaExistente.RFC = model.RFC;
                bolsaExistente.NSS = model.NSS;
                bolsaExistente.Direccion = model.Direccion;
                bolsaExistente.Observaciones = model.Observaciones;
                bolsaExistente.Documento = model.Documento; // Asignación de documento principal
                bolsaExistente.Escolaridad = model.Escolaridad;
                bolsaExistente.EstadoCivil = model.EstadoCivil;
                bolsaExistente.Experiencia = model.Experiencia;
                bolsaExistente.Nacionalidad = model.Nacionalidad;
                bolsaExistente.Sexo = model.Sexo;
                bolsaExistente.FNacimiento = model.FNacimiento;
                bolsaExistente.Perfil = model.Perfil;
                bolsaExistente.Estatus = model.Estatus;

                await repositorioMantenimientoBolsa.UpdateBolsa(bolsaExistente);

                return RedirectToAction("Crear");
            } 
            else if (action == "restringir")
            {
                if (model.Estatus == "Empleado")
                {
                    ModelState.AddModelError(string.Empty, "Favor de restringir desde Mantenimiento Empleado");
                    return View(model); // Retorna la vista con el mensaje de error
                }
                var infoBolsa = await repositorioMantenimientoBolsa.GetBolsaByClave(model.Clave);
                var nuevoRestringido = new MantenimientoRestringidoModel
                {
                    Nombre = infoBolsa.Nombre,
                    AMaterno = infoBolsa.AMaterno,
                    APaterno = infoBolsa.APaterno,
                    FNacimiento = infoBolsa.FNacimiento,
                    CURP = infoBolsa.CURP,
                    Sexo = infoBolsa.Sexo,
                    FIngresoE = infoBolsa.FIngreso,
                    FIngresoM = DateTime.Now,
                    Clave = model.Clave
                };
                var ingresaRestringido = await repositorioMantenimientoRestringido.InsertRestringido(nuevoRestringido);
                // Lógica para Baja
                await repositorioMantenimientoBolsa.DeleteBolsaByClave(model.Clave);
                return RedirectToAction("Crear");
            }

            return View(model); // Si no se selecciona ninguna acción
        }
    }
}
