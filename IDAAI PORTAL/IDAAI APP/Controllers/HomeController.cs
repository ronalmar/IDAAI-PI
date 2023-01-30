using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations.Request;
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
            ViewBag.Opcion = "Home";
            ViewBag.PantallaControl = "Home";
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
        public async Task<IActionResult> ValidarLogin()
        {
            using (var client = new HttpClient())
            {
                var claims = HttpContext.User.Claims.ToList();
                if(claims.Count < 1)
                {
                    return RedirectToAction("Login", "Home");
                }
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                OperacionesController operaciones = new();

                var respuestaToken = await operaciones.RenovarToken(claims) as OkObjectResult;
                var claimsNuevos = respuestaToken.Value as List<Claim>;

                if(claimsNuevos.Count < 1)
                {
                    return RedirectToAction("Login", "Home");
                }

                var requestFecha = new EditarFechaUsuarioRequest();
                requestFecha.Usuario = usuario;
                var respuestaFechaActualizada = await operaciones.ActualizarFechaUsuario(requestFecha, claimsNuevos) as ObjectResult;

                if (respuestaFechaActualizada.StatusCode.Equals(400))
                {
                    return RedirectToAction("Login", "Home");
                }

                var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Menu");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Opcion = "Login";
            ViewBag.PantallaControl = "Home";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registro()
        {
            ViewBag.Opcion = "Registro";
            ViewBag.PantallaControl = "Home";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AsistenciaEstudiante()
        {
            ViewBag.Opcion = "AsistenciaEstudiante";
            ViewBag.PantallaControl = "Home";
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