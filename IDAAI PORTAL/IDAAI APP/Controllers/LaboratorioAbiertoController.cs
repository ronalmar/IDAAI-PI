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

            List<Carrera> listaCarreras = await operaciones.GetCarreras();
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
            List<Inventario> listaInventario = await operaciones.GetInventario();
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
            List<Carrera> listaCarreras = await operaciones.GetCarreras();
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