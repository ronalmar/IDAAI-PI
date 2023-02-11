using IDAAI_APP.Models;
using Microsoft.AspNetCore.Authentication;
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
            return RedirectToAction("Item");
        }

        [HttpGet]
        public async Task<IActionResult> Asistencia()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetCarreras(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaCarreras = result.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Carreras = Carreras;
            ViewBag.Opcion = "Asistencia";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [HttpGet]
        public IActionResult Inventario()
        {
            ViewBag.Opcion = "Inventario";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Item()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetInventario(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaInventario = result.Value as List<Inventario>;
            List<SelectListItem> Inventario = new();
            foreach (var inventario in listaInventario)
            {
                Inventario.Add(new SelectListItem() { Text = inventario.Nombre, Value = inventario.Nombre });
            }

            foreach (var inventarioUnidad in Inventario)
            {
                if (inventarioUnidad.Value == "General")
                {
                    inventarioUnidad.Selected = true;
                }
            }

            ViewBag.Inventario = Inventario;
            ViewBag.Opcion = "Item";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [HttpGet]
        public IActionResult Prestamo()
        {
            ViewBag.Opcion = "Prestamo";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Estudiante()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetCarreras(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaCarreras = result.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            ViewBag.Carreras = Carreras;
            ViewBag.Opcion = "Estudiante";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [HttpGet]
        public IActionResult Carrera()
        {
            ViewBag.Opcion = "Carrera";
            ViewBag.Laboratorio = "Abierto";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}