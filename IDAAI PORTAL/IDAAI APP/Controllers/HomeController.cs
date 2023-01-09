using IDAAI_APP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IDAAI_APP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(LoginUser loginUser)
        {
            // Proceso de Iniciar Sesion
            return RedirectToAction("Index");
        }       

        [HttpPost]
        public IActionResult Registrarse(LoginUser loginUser)
        {
            // Proceso de Registrarse
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}