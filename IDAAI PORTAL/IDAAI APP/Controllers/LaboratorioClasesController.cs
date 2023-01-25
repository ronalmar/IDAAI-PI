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
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as OkObjectResult;
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var result2 = await operaciones.GetCarreras(claims) as OkObjectResult;
            var listaCarreras = result2.Value as List<Carrera>;
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
        public async Task<IActionResult> Prestamo()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as OkObjectResult;
            var listaModulos = result.Value as List<Modulo>;
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
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as OkObjectResult;
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }           

            var result2 = await operaciones.GetCarreras(claims) as OkObjectResult;
            var listaCarreras = result2.Value as List<Carrera>;
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
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as OkObjectResult;
            var listaModulos = result.Value as List<Modulo>;
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