using IDAAI_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Globalization;

namespace IDAAI_APP.Controllers
{
    public class MenuController : Controller
    {
        private readonly ILogger<MenuController> _logger;

        public MenuController(ILogger<MenuController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {            
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            string usuario = "ronalmar";

            ViewBag.Usuario = textInfo.ToTitleCase(usuario);
            return View();
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            string usuario = "ronalmar";

            ViewBag.Usuario = textInfo.ToTitleCase(usuario);
            ViewBag.Email = "ronalmar@espol.edu.ec";
            ViewBag.Id = 1;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}