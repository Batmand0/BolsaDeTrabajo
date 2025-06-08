using BolsaDeTrabajo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BolsaDeTrabajo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalogos()
        {
            return View();
        }

        public IActionResult Mantenimientos()
        {
            return View();
        }

        public IActionResult Reportes()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
