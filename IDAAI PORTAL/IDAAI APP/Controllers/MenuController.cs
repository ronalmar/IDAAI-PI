using IDAAI_APP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Globalization;

namespace IDAAI_APP.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;
        private OperacionesController operaciones = new();

        public MenuController(ILogger<MenuController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {            
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var usuario = HttpContext.User.Identity.Name;

            ViewBag.Usuario = textInfo.ToTitleCase(usuario);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            var user = HttpContext.User.Identity.Name;

            var result = await operaciones.ObtenerUsuario(user) as OkObjectResult;
            var datosUsuario = result.Value as Usuario_;

            ViewBag.Usuario = textInfo.ToTitleCase(datosUsuario.Usuario);
            ViewBag.Email = datosUsuario.Email is null || datosUsuario.Email == "" ? "-" : datosUsuario.Email;
            ViewBag.Id = datosUsuario.Id;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}