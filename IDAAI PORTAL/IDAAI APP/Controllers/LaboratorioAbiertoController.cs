using IDAAI_APP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace IDAAI_APP.Controllers
{
    public class LaboratorioAbiertoController : Controller
    {
        private readonly ILogger<LaboratorioAbiertoController> _logger;
        private OperacionesController operaciones = new();

        public LaboratorioAbiertoController(ILogger<LaboratorioAbiertoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Asistencia");
        }

        [HttpGet]
        public async Task<IActionResult> Asistencia()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetCarreras(claims) as OkObjectResult;
            var listaCarreras = result.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Carreras = Carreras;
            return View();
        }

        [HttpGet]
        public IActionResult Inventario()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Item()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetInventario(claims) as OkObjectResult;
            var listaInventario = result.Value as List<Inventario>;
            List<SelectListItem> Inventario = new();
            foreach (var inventario in listaInventario)
            {
                Inventario.Add(new SelectListItem() { Text = inventario.Nombre, Value = inventario.Nombre });
            }
            ViewBag.Inventario = Inventario;
            return View();
        }

        [HttpGet]
        public IActionResult Prestamo()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Estudiante()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetCarreras(claims) as OkObjectResult;
            var listaCarreras = result.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Carreras = Carreras;
            return View();
        }

        [HttpGet]
        public IActionResult Carrera()
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