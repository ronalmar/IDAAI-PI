using IDAAI_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace IDAAI_APP.Controllers
{
    public class LaboratorioAbiertoController : Controller
    {
        private readonly ILogger<LaboratorioAbiertoController> _logger;

        public LaboratorioAbiertoController(ILogger<LaboratorioAbiertoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Inventario");
        }

        [HttpGet]
        public IActionResult Asistencia()
        {
            List<SelectListItem> Modulos = new()
            {
                new SelectListItem() { Text = "LST", Value = "LST" },
                new SelectListItem() { Text = "APPSMOV", Value = "APPSMOV" }
            };
            ViewBag.Modulos = Modulos;

            List<SelectListItem> Carreras = new()
            {
                new SelectListItem() { Text = "Ing. Telemática", Value = "Ing. Telemática" },
                new SelectListItem() { Text = "Ing. Ciencias Computacionales", Value = "Ing. Ciencias Computacionales" },
                new SelectListItem() { Text = "Lic. Turismo", Value = "Lic. Turismo" },
                new SelectListItem() { Text = "Ing. Comercial", Value = "Ing. Comercial" }
            };
            ViewBag.Carreras = Carreras;
            return View();
        }

        [HttpGet]
        public IActionResult Inventario()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Item()
        {
            List<SelectListItem> Inventario = new()
            {
                new SelectListItem() { Text = "Placa", Value = "Placa" },
                new SelectListItem() { Text = "Cableado", Value = "Cableado" }
            };
            ViewBag.Inventario = Inventario;
            return View();
        }

        [HttpGet]
        public IActionResult Prestamo()
        {
            List<SelectListItem> Modulos = new()
            {
                new SelectListItem() { Text = "LST", Value = "LST" },
                new SelectListItem() { Text = "APPSMOV", Value = "APPSMOV" }
            };
            ViewBag.Modulos = Modulos;
            return View();
        }

        [HttpGet]
        public IActionResult Estudiante()
        {
            List<SelectListItem> Modulos = new()
            {
                new SelectListItem() { Text = "LST", Value = "LST" },
                new SelectListItem() { Text = "APPSMOV", Value = "APPSMOV" }
            };
            ViewBag.Modulos = Modulos;

            List<SelectListItem> Carreras = new()
            {
                new SelectListItem() { Text = "Ing. Telemática", Value = "Ing. Telemática" },
                new SelectListItem() { Text = "Ing. Ciencias Computacionales", Value = "Ing. Ciencias Computacionales" },
                new SelectListItem() { Text = "Lic. Turismo", Value = "Lic. Turismo" },
                new SelectListItem() { Text = "Ing. Comercial", Value = "Ing. Comercial" }
            };
            ViewBag.Carreras = Carreras;
            return View();
        }

        [HttpGet]
        public IActionResult Carrera()
        {
            List<SelectListItem> Modulos = new()
            {
                new SelectListItem() { Text = "LST", Value = "LST" },
                new SelectListItem() { Text = "APPSMOV", Value = "APPSMOV" }
            };
            ViewBag.Modulos = Modulos;
            return View();
        }

        [HttpGet]
        public IActionResult Modulo()
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