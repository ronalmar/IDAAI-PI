using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations;
using IDAAI_APP.Models.Operations.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace IDAAI_APP.Controllers
{
    public class OperacionesController : Controller
    {
        private readonly string contentType = "application/json";
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
                    command = $"api/estudiante/consultarPorMatricula?Matricula={query.Matricula}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Email))
                {
                    command = $"api/estudiante/consultarPorEmail?Email={query.Email}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Nombres) || !string.IsNullOrEmpty(query.Apellidos))
                {
                    command = $"api/estudiante/listarPorNombres?Nombres={query.Nombres}&Apellidos={query.Apellidos}&Modulo={query.Modulo}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.Carrera) && query.Carrera != "0")
                {
                    command = $"api/estudiante/listarPorCarrera?Carrera={query.Carrera}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/estudiante/listarPorModulo?Modulo={query.Modulo}{pagineo}";
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/estudiante/registrarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/estudiante/registrarGrupoEstudiante?Modulo={request.Modulo}";
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
                    else
                    {
                        await HttpContext.SignOutAsync();
                        return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                    }
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

                command = $"api/estudiante/editarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                    command = $"api/asistencia/consultarPorMatricula?Matricula={query.Matricula}&Modulo={query.Modulo}{pagineo}";
                    esLista = false;
                }
                else if (!string.IsNullOrEmpty(query.Nombres) || !string.IsNullOrEmpty(query.Apellidos))
                {
                    command = $"api/asistencia/listarPorNombres?Nombres={query.Nombres}&Apellidos={query.Apellidos}&Modulo={query.Modulo}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.Carrera) && query.Carrera != "0")
                {
                    command = $"api/asistencia/listarPorCarrera?Carrera={query.Carrera}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/asistencia/listarPorModulo?Modulo={query.Modulo}{pagineo}";
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/asistencia/registrarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/asistencia/editarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                    command = $"api/carrera/listarPorNombre?Nombre={query.Nombre}&Modulo={query.Modulo}{pagineo}";
                }
                else
                {
                    command = $"api/carrera/listarPorNombre?Modulo={query.Modulo}{pagineo}";
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/carrera/registrarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/carrera/editarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                    command = $"api/modulo/listarPorNombre?Nombre={query.Nombre}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.PeriodoAcademico))
                {
                    command = $"api/modulo/listarPorPeriodoAcademico?PeriodoAcademico={query.PeriodoAcademico}{pagineo}";
                }
                else
                {
                    command = $"api/modulo/listarTodos?{pagineo}";
                }

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Modulo> modulos = JsonConvert.DeserializeObject<List<Modulo>>(response);

                    return Ok(modulos);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/modulo/registrarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/modulo/editarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/inventario/listarPorNombre?Nombre={query.Nombre}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return Ok(inventario);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/inventario/registrarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/inventario/editarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/item/listarItems?Rfid={query.Rfid}&Inventario={query.Inventario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Item> items = JsonConvert.DeserializeObject<List<Item>>(response);

                    return Ok(items);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/item/registrarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/item/editarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/listarPrestamosPorModulo?Modulo={query.Modulo}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoModulo> prestamos = JsonConvert.DeserializeObject<List<PrestamoModulo>>(response);

                    return Ok(prestamos);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/registrarPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/registrarGrupoPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/devolverPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/listarPrestamosPorEstudiante?Matricula={query.Matricula}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoEstudiante> prestamos = JsonConvert.DeserializeObject<List<PrestamoEstudiante>>(response);

                    return Ok(prestamos);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/registrarPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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

                command = $"api/prestamo/devolverPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
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
                else
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginUser request)
        {
            string response;
            string error;

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
                error = res.Content.ReadAsStringAsync().Result;
                error = "El usuario o la contraseña son incorrectos";
            }
            return BadRequest(error);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registrarse(RegisterUser request)
        {
            string response;
            string error;

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
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
                else
                {
                    return Unauthorized(res.Content.ReadAsStringAsync().Result);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuario(string user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                //var claims = HttpContext.User.Claims.ToList();
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

                command = $"api/usuario/obtenerUsuario?Usuario={user}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Usuario_> usuarios = JsonConvert.DeserializeObject<List<Usuario_>>(response);

                    return Ok(usuarios[0]);
                }
                else
                {
                    //await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Logout", "Operaciones") });
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

                var claimsRenovados = HttpContext.User.Claims.ToList();
                var tokenRenovado = claimsRenovados[1].Value;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRenovado}");

                command = $"api/usuario/editarUsuario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Json(new { redirectToUrl = Url.Action("Perfil", "Menu") });
                }
                else if(res.StatusCode.Equals(401))
                {
                    await HttpContext.SignOutAsync();
                    return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                }
                else
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return BadRequest("La contraseña ingresada es incorrecta");
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

                command = $"api/inventario/listarPorNombre?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return Ok(inventario);
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("Logout", "Operaciones") });
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

                command = $"api/carrera/listarTodos?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Carrera> carreras = JsonConvert.DeserializeObject<List<Carrera>>(response);

                    return Ok(carreras);
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("Logout", "Operaciones") });
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

                command = $"api/modulo/listarTodos?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Modulo> modulos = JsonConvert.DeserializeObject<List<Modulo>>(response);

                    return Ok(modulos);
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("Logout", "Operaciones") });
                }
            }
        }
    }
}