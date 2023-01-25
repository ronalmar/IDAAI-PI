using IDAAI_APP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace IDAAI_APP.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Cancelar()
        {
            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(LoginUser loginUser)
        {
            // Proceso de Iniciar Sesion
            return View("Index");
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