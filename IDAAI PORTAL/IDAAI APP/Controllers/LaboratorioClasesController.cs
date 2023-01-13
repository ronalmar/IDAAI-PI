using IDAAI_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace IDAAI_APP.Controllers
{
    public class LaboratorioClasesController : Controller
    {
        private readonly ILogger<LaboratorioClasesController> _logger;
        private OperacionesController operaciones = new();

        public LaboratorioClasesController(ILogger<LaboratorioClasesController> logger)
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
            List<Modulo> listaModulos = await operaciones.GetModulos();
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            List<Carrera> listaCarreras = await operaciones.GetCarreras();
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Modulos = Modulos;
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
        public async Task<IActionResult> Prestamo()
        {
            List<Modulo> listaModulos = await operaciones.GetModulos();
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }
            ViewBag.Modulos = Modulos;
            return View();
        }

        [HttpGet]
        public  async Task<IActionResult> Estudiante()
        {
            List<Modulo> listaModulos = await operaciones.GetModulos();
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }           

            List<Carrera> listaCarreras = await operaciones.GetCarreras();
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Carreras = Carreras;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Carrera()
        {
            List<Modulo> listaModulos = await operaciones.GetModulos();
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }
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