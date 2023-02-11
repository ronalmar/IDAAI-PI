using IDAAI_APP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;

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
            return RedirectToAction("Clase");
        }

        [HttpGet]
        public async Task<IActionResult> Clase()
        {
            var claims = HttpContext.User.Claims.ToList();

            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if(result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Opcion = "Clase";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Asistencia()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var result2 = await operaciones.GetCarreras(claims) as ObjectResult;
            if (result2 is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaCarreras = result2.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Carreras = Carreras;
            ViewBag.Opcion = "Asistencia";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Inventario()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Opcion = "Inventario";
            ViewBag.Laboratorio = "Clases";

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
            var result2 = await operaciones.GetModulos(claims) as ObjectResult;
            if (result2 is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result2.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            foreach (var inventarioUnidad in Inventario)
            {
                if (inventarioUnidad.Value == "General")
                {
                    inventarioUnidad.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Inventario = Inventario;
            ViewBag.Opcion = "Item";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Prestamo()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Opcion = "Prestamo";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public  async Task<IActionResult> Estudiante()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }           

            var result2 = await operaciones.GetCarreras(claims) as ObjectResult;
            if (result2 is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaCarreras = result2.Value as List<Carrera>;
            List<SelectListItem> Carreras = new();
            foreach (var carrera in listaCarreras)
            {
                Carreras.Add(new SelectListItem() { Text = carrera.Nombre, Value = carrera.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Carreras = Carreras;
            ViewBag.Opcion = "Estudiante";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Carrera()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Opcion = "Carrera";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Modulo()
        {
            var claims = HttpContext.User.Claims.ToList();
            var result = await operaciones.GetModulos(claims) as ObjectResult;
            if (result is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var listaModulos = result.Value as List<Modulo>;
            List<SelectListItem> Modulos = new();
            foreach (var modulo in listaModulos)
            {
                Modulos.Add(new SelectListItem() { Text = modulo.Nombre, Value = modulo.Nombre });
            }

            var respuestaModuloActual = await operaciones.ObtenerModuloActual(claims) as ObjectResult;
            if (respuestaModuloActual is null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Home");
            }
            var datosModuloActual = respuestaModuloActual.Value as Usuario_;
            var moduloActual = datosModuloActual.ModuloActual;
            foreach (var modulo in Modulos)
            {
                if (modulo.Value == moduloActual)
                {
                    modulo.Selected = true;
                }
            }

            ViewBag.Modulos = Modulos;
            ViewBag.Opcion = "Modulo";
            ViewBag.Laboratorio = "Clases";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}