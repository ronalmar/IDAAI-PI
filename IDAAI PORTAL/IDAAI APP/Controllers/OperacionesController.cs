using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations;
using IDAAI_APP.Models.Operations.Request;
using IDAAI_APP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
//using System.Web.Mvc;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IDAAI_APP.Controllers
{   
    public class OperacionesController : Controller
    {
        private readonly string contentType = "application/json";
        // PROD
        //private static readonly string serverName = "localhost";
        //private static readonly string portNumber = "2000";
        //private readonly string apiUrl = $"https://{serverName}:{portNumber}/";

        // DEV CONVEYER
        //private static readonly string serverName = "192.168.1.22";
        //private static readonly string portNumber = "45456";
        //private readonly string apiUrl = $"https://{serverName}:{portNumber}/";

        // DEV
        private static readonly string serverName = "localhost";
        private static readonly string portNumber = "44321";
        private readonly string apiUrl = $"https://{serverName}:{portNumber}/";

        private readonly string pagineo = "&Pagina=1&RecordsPorPagina=100000";
        private bool esLista = true;
        private string command;
        private StringContent stringContent;       

        public OperacionesController()
        {

        }        

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListarEstudiante(EstudianteQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                if (!string.IsNullOrEmpty(query.Matricula))
                {
                    command = $"api/estudiante/consultarPorMatricula?Usuario={usuario}&Matricula={query.Matricula}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Email))
                {
                    command = $"api/estudiante/consultarPorEmail?Usuario={usuario}&Email={query.Email}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Nombres) || !string.IsNullOrEmpty(query.Apellidos))
                {
                    command = $"api/estudiante/listarPorNombres?Usuario={usuario}&Nombres={query.Nombres}&Apellidos={query.Apellidos}&Modulo={query.Modulo}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.Carrera) && query.Carrera != "0")
                {
                    command = $"api/estudiante/listarPorCarrera?Usuario={usuario}&Carrera={query.Carrera}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/estudiante/listarPorModulo?Usuario={usuario}&Modulo={query.Modulo}{pagineo}";
                }

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    List<Estudiante> estudiantes = new();

                    if (esLista)
                    {
                        estudiantes = JsonConvert.DeserializeObject<List<Estudiante>>(response);
                    }
                    else
                    {
                        var estudiante = JsonConvert.DeserializeObject<Estudiante>(response);
                        if (estudiante is null)
                        {
                            return Ok(estudiantes);
                        }
                        estudiantes.Add(estudiante);
                    }
                    return Ok(estudiantes);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudiante(EstudianteRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/estudiante/registrarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudianteGrupo(EstudianteGrupoRegistrarRequest request)
        {
            string response;
            command = $"api/estudiante/registrarGrupoEstudiante?Modulo={request.Modulo}";

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");


                request.Usuario = usuario;
                command = $"api/estudiante/registrarGrupoEstudiante?Usuario={usuario}&Modulo={request.Modulo}";
                using (var content = new MultipartFormDataContent())
                {
                    byte[] Bytes = new byte[request.Archivo[0].Length + 1];
                    using (var ms = new MemoryStream())
                    {
                        await request.Archivo[0].CopyToAsync(ms);
                        Bytes = ms.ToArray();
                    }
                    var fileContent = new ByteArrayContent(Bytes);

                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = request.Archivo[0].FileName };
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    fileContent.Headers.ContentLength= request.Archivo[0].Length;

                    content.Add(fileContent, "Archivo", request.Archivo[0].FileName);

                    var requestUri = apiUrl + command;

                    var res = client.PostAsync(requestUri, content).Result;

                    if (res.IsSuccessStatusCode)
                    {
                        response = res.Content.ReadAsStringAsync().Result;

                        return Ok(response);
                    }
                    else if (res.ReasonPhrase.Equals("Unauthorized"))
                    {
                        await HttpContext.SignOutAsync();
                        return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                    }
                    else
                    {
                        return BadRequest(res.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarRegistroEstudianteGrupo(List<EstudianteRegistrarRequest> estudiantes, string modulo, List<Claim> claims)
        {
            string response;
            command = $"api/estudiante/procesarRegistroGrupoEstudiante";

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/estudiante/procesarRegistroGrupoEstudiante";

                foreach(var estudiante in estudiantes)
                {
                    estudiante.Usuario = usuario;
                }

                EstudianteGrupoRequest request = new()
                {
                    Usuario = usuario,
                    Modulo = modulo,
                    Estudiantes = estudiantes
                };

                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    var error = res.Content.ReadAsStringAsync().Result;
                    return BadRequest(error);
                }

            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarEstudiante(EstudianteEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/estudiante/editarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarEstudiante(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/estudiante/eliminarEstudiante";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl+command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConsultarClase(PaginacionQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/asistencia/consultarClase?Usuario={usuario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    List<Asistencia> listaAsistencia = new();

                    if (esLista)
                    {
                        listaAsistencia = JsonConvert.DeserializeObject<List<Asistencia>>(response);
                    }
                    else
                    {
                        var asistencia = JsonConvert.DeserializeObject<Asistencia>(response);
                        if (asistencia is null)
                        {
                            return Ok(listaAsistencia);
                        }
                        listaAsistencia.Add(asistencia);
                    }
                    return Ok(listaAsistencia);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsistencia(AsistenciaQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                if (!string.IsNullOrEmpty(query.Matricula))
                {
                    command = $"api/asistencia/consultarPorMatricula?Usuario={usuario}&Matricula={query.Matricula}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Nombres) || !string.IsNullOrEmpty(query.Apellidos))
                {
                    command = $"api/asistencia/listarPorNombres?Usuario={usuario}&Nombres={query.Nombres}&Apellidos={query.Apellidos}&Modulo={query.Modulo}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.Carrera) && query.Carrera != "0")
                {
                    command = $"api/asistencia/listarPorCarrera?Usuario={usuario}&Carrera={query.Carrera}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/asistencia/listarPorModulo?Usuario={usuario}&Modulo={query.Modulo}{pagineo}";
                }

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    List<Asistencia> listaAsistencia = new();

                    if (esLista)
                    {
                        listaAsistencia = JsonConvert.DeserializeObject<List<Asistencia>>(response);
                    }
                    else
                    {
                        var asistencia = JsonConvert.DeserializeObject<Asistencia>(response);
                        if(asistencia is null)
                        {
                            return Ok(listaAsistencia);
                        }
                        listaAsistencia.Add(asistencia);
                    }
                    return Ok(listaAsistencia);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAsistencia(AsistenciaRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/asistencia/registrarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarAsistencia(AsistenciaEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/asistencia/editarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarAsistencia(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/asistencia/eliminarRegistroAsistencia";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarCarrera(CarreraQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                if (!string.IsNullOrEmpty(query.Nombre))
                {
                    command = $"api/carrera/listarPorNombre?Usuario={usuario}&Nombre={query.Nombre}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/carrera/listarPorModulo?Usuario={usuario}&Modulo={query.Modulo}{pagineo}";
                }

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    List<Carrera> carreras = new();

                    if (esLista)
                    {
                        carreras = JsonConvert.DeserializeObject<List<Carrera>>(response);
                    }
                    else
                    {
                        var carrera = JsonConvert.DeserializeObject<Carrera>(response);
                        if (carrera is null)
                        {
                            return Ok(carreras);
                        }
                        carreras.Add(carrera);
                    }
                    return Ok(carreras);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCarrera(CarreraRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/carrera/registrarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarCarrera(CarreraEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/carrera/editarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCarrera(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/carrera/eliminarCarrera";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarModulo(ModuloQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");


                if (!string.IsNullOrEmpty(query.Nombre))
                {
                    command = $"api/modulo/listarPorNombre?Usuario={usuario}&Nombre={query.Nombre}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.PeriodoAcademico))
                {
                    command = $"api/modulo/listarPorPeriodoAcademico?Usuario={usuario}&PeriodoAcademico={query.PeriodoAcademico}{pagineo}";
                }
                else
                {
                    command = $"api/modulo/listarTodos?Usuario={usuario}{pagineo}";
                }

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Modulo> modulos = JsonConvert.DeserializeObject<List<Modulo>>(response);

                    return Ok(modulos);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarModulo(ModuloRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/modulo/registrarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if(res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarModulo(ModuloEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/modulo/editarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarModulo(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/modulo/eliminarModulo";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarInventario(InventarioQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/inventario/listarPorNombre?Usuario={usuario}&Nombre={query.Nombre}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return Ok(inventario);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarInventario(InventarioRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/inventario/registrarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> SincronizarInventario()
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/inventario/actualizarCantidadesInventario";

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarInventario(InventarioEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/inventario/editarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarInventario(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/inventario/eliminarInventario";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarItem(ItemQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/item/listarItems?Usuario={usuario}&Rfid={query.Rfid}&Inventario={query.Inventario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Item> items = JsonConvert.DeserializeObject<List<Item>>(response);

                    return Ok(items);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarItem(ItemRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/item/registrarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> EditarItem(ItemEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/item/editarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarItem(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/item/eliminarItem";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarPrestamo(PrestamoModuloQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/prestamo/listarPrestamosPorModulo?Usuario={usuario}&Modulo={query.Modulo}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoModulo> prestamos = JsonConvert.DeserializeObject<List<PrestamoModulo>>(response);

                    return Ok(prestamos);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamo(PrestamoModuloRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/prestamo/registrarPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamoGrupo(PrestamoGrupoModuloRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/prestamo/registrarGrupoPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarPrestamo(PrestamoEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/prestamo/devolverPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPrestamo(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");
                command = $"api/prestamo/eliminarPrestamoPorModulo";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarPrestamoLA(PrestamoEstudianteQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/prestamo/listarPrestamosPorEstudiante?Usuario={usuario}&Matricula={query.Matricula}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoEstudiante> prestamos = JsonConvert.DeserializeObject<List<PrestamoEstudiante>>(response);

                    return Ok(prestamos);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamoLA(PrestamoEstudianteRegistrarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/prestamo/registrarPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarPrestamoLA(PrestamoEditarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                request.Usuario = usuario;
                command = $"api/prestamo/devolverPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPrestamoLA(EliminarRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/prestamo/eliminarPrestamoPorEstudiante";

                var deleteRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(apiUrl + command),
                    Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType)
                };

                HttpResponseMessage res = await client.SendAsync(deleteRequest);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> EstablecerAsistencia(EstablecerAsistenciaRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));      

                command = $"api/asistencia/establecerAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [System.Web.Mvc.ValidateInput(false)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginUser request)
        {
            string response;
            //
            //var claimsPrueba = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Name, request.Usuario),
            //            new Claim(ClaimTypes.Authentication, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc3VhcmlvIjoiYWRtaW4xIiwibmJmIjoxNjc1MjI0MzE4LCJleHAiOjE2NzUyMjYxMTgsImlhdCI6MTY3NTIyNDMxOH0.wNb3KHv46MTihdronYJSbYEdaITQR-QvSZ6u7v-DlW8")
            //        };

            //var claimsIdentityPrueba = new ClaimsIdentity(claimsPrueba, "Login");

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentityPrueba));
            //return Json(new { redirectToUrl = Url.Action("Index", "Menu") });
            //
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/usuario/loginUsuario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);
                
                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;
                    Token_ token = JsonConvert.DeserializeObject<Token_>(response);                    

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Usuario),
                        new Claim(ClaimTypes.Authentication, token.Token)
                    };                    

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { redirectToUrl = Url.Action("Index", "Menu") });
                }
                else if(res.ReasonPhrase.Equals("Unauthorized"))
                {                    
                    return BadRequest("El usuario o contraseña ingresados no son correctos");
                }
                else
                {
                    var error = res.Content.ReadAsStringAsync().Result;
                    return BadRequest(error);
                }
            }
        }        

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registrarse(RegisterUser request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/usuario/registrarUsuario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;
                    Token_ token = JsonConvert.DeserializeObject<Token_>(response);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Usuario),
                        new Claim(ClaimTypes.Authentication, token.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { redirectToUrl = Url.Action("Index", "Menu") });
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            ViewBag.Opcion = "Logout";
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }       

        [HttpGet]
        public async Task<IActionResult> RenovarToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(response);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Ok(res.Content.ReadAsStringAsync().Result);
                }
                else if(res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]        
        public async Task<IActionResult> RenovarToken(List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(response);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    return Ok(claimsNuevos);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    var claimsNuevos = new List<Claim>();
                    return Ok(claimsNuevos);
                    //return RedirectToAction("Login", "Home");
                }
                else
                {
                    var claimsNuevos = new List<Claim>();
                    return Ok(claimsNuevos);
                    //return RedirectToAction("Login", "Home");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuario(string user, List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                //command = $"api/usuario/renovarToken";

                //HttpResponseMessage resToken = await client.GetAsync(command);

                //if (resToken.IsSuccessStatusCode)
                //{
                //    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                //    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                //    var claimsNuevos = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.Name, usuario),
                //        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                //    };

                //    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                //}

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/usuario/obtenerUsuario?Usuario={user}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Usuario_> usuarios = JsonConvert.DeserializeObject<List<Usuario_>>(response);

                    return Ok(usuarios[0]);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerModuloActual()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();

                var user = claims[0].Value;

                var result = await ObtenerUsuario(user, claims) as OkObjectResult;
                if (result is null)
                {
                    return RedirectToAction("Logout", "Operaciones");
                }
                var datosUsuario = result.Value as Usuario_;

                return Ok(datosUsuario);

                //var usuario = claims[0].Value;
                //var token = claims[1].Value;
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                //command = $"api/usuario/renovarToken";

                //HttpResponseMessage resToken = await client.GetAsync(command);

                //if (resToken.IsSuccessStatusCode)
                //{
                //    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                //    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                //    var claimsNuevos = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.Name, usuario),
                //        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                //    };

                //    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                //}

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                //command = $"api/usuario/obtenerUsuario?Usuario={user}";

                //HttpResponseMessage res = await client.GetAsync(command);

                //if (res.IsSuccessStatusCode)
                //{
                //    var response = res.Content.ReadAsStringAsync().Result;

                //    List<Usuario_> usuarios = JsonConvert.DeserializeObject<List<Usuario_>>(response);

                //    return Ok(usuarios[0]);
                //}
                //else
                //{
                //    //await HttpContext.SignOutAsync();
                //    return null;
                //}
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerModuloActual(List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();

                var user = claims[0].Value;

                var result = await ObtenerUsuario(user, claims) as OkObjectResult;
                if (result is null)
                {
                    return RedirectToAction("Logout", "Operaciones");
                }
                var datosUsuario = result.Value as Usuario_;

                return Ok(datosUsuario);

                //var usuario = claims[0].Value;
                //var token = claims[1].Value;
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                //command = $"api/usuario/renovarToken";

                //HttpResponseMessage resToken = await client.GetAsync(command);

                //if (resToken.IsSuccessStatusCode)
                //{
                //    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                //    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                //    var claimsNuevos = new List<Claim>
                //    {
                //        new Claim(ClaimTypes.Name, usuario),
                //        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                //    };

                //    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                //}

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                //command = $"api/usuario/obtenerUsuario?Usuario={user}";

                //HttpResponseMessage res = await client.GetAsync(command);

                //if (res.IsSuccessStatusCode)
                //{
                //    var response = res.Content.ReadAsStringAsync().Result;

                //    List<Usuario_> usuarios = JsonConvert.DeserializeObject<List<Usuario_>>(response);

                //    return Ok(usuarios[0]);
                //}
                //else
                //{
                //    //await HttpContext.SignOutAsync();
                //    return null;
                //}
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarModuloActual(EditarUsuarioModuloActualRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                request.Usuario = usuario;
                command = $"api/usuario/actualizarModuloActual";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    command = $"api/usuario/renovarToken";

                    HttpResponseMessage resToken = await client.GetAsync(command);

                    if (resToken.IsSuccessStatusCode)
                    {
                        var responseToken = resToken.Content.ReadAsStringAsync().Result;
                        Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                        var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                        var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    }

                    Usuario_ respuestaUsuario = JsonConvert.DeserializeObject<Usuario_>(response);

                    return Ok(respuestaUsuario);
                }
                else if(res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioRequest request)
        {
            string response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/editarUsuario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    command = $"api/usuario/renovarToken";

                    HttpResponseMessage resToken = await client.GetAsync(command);

                    if (resToken.IsSuccessStatusCode)
                    {
                        var responseToken = resToken.Content.ReadAsStringAsync().Result;
                        Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                        var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                        var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    }

                    return Json(new { redirectToUrl = Url.Action("Perfil", "Menu") });
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarFechaUsuario(EditarFechaUsuarioRequest request, List<Claim> claims)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                request.Usuario = usuario;
                command = $"api/usuario/actualizarFecha";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {        
                    return Ok(res.Content.ReadAsStringAsync().Result);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> SincronizarAsistencia()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/asistencia/sincronizarAsistencia";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    //var response = res.Content.ReadAsStringAsync().Result;
                    //List<Asistencia> listaAsistencia = new();

                    //if (esLista)
                    //{
                    //    listaAsistencia = JsonConvert.DeserializeObject<List<Asistencia>>(response);
                    //}
                    //else
                    //{
                    //    var asistencia = JsonConvert.DeserializeObject<Asistencia>(response);
                    //    if (asistencia is null)
                    //    {
                    //        return Ok(listaAsistencia);
                    //    }
                    //    listaAsistencia.Add(asistencia);
                    //}
                    //return Ok(listaAsistencia);
                    return Ok(res.Content.ReadAsStringAsync().Result);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInventario(List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/inventario/listarPorNombre?Usuario={usuario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return Ok(inventario);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCarreras(List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/carrera/listarTodos?Usuario={usuario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Carrera> carreras = JsonConvert.DeserializeObject<List<Carrera>>(response);

                    return Ok(carreras);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetModulos(List<Claim> claims)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();
                var usuario = claims[0].Value;
                var token = claims[1].Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                command = $"api/usuario/renovarToken";

                HttpResponseMessage resToken = await client.GetAsync(command);

                if (resToken.IsSuccessStatusCode)
                {
                    var responseToken = resToken.Content.ReadAsStringAsync().Result;
                    Token_ tokenNuevo = JsonConvert.DeserializeObject<Token_>(responseToken);

                    var claimsNuevos = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario),
                        new Claim(ClaimTypes.Authentication, tokenNuevo.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claimsNuevos, "Login");

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                //var claimsRenovados = HttpContext.User.Claims.ToList();
                //var tokenRenovado = claimsRenovados[1].Value;

                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/modulo/listarTodos?Usuario={usuario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Modulo> modulos = JsonConvert.DeserializeObject<List<Modulo>>(response);

                    return Ok(modulos);
                }
                else if (res.ReasonPhrase.Equals("Unauthorized"))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    return BadRequest(res.Content.ReadAsStringAsync().Result);
                }
            }
        }
    }
}