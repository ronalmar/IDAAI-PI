using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations;
using IDAAI_APP.Models.Operations.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
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
        public async Task<List<Estudiante>> ListarEstudiante(EstudianteQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                            return estudiantes;
                        }
                        estudiantes.Add(estudiante);
                    }
                    return estudiantes;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudiante(EstudianteRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/estudiante/registrarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }
        
        [HttpPost]
        public async Task<IActionResult> RegistrarEstudianteGrupo(EstudianteGrupoRegistrarRequest request)
        {
            string response;
            string error;
            command = $"api/estudiante/registrarGrupoEstudiante?Modulo={request.Modulo}";

            using (var client = new HttpClient())
            {
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
                    error = res.Content.ReadAsStringAsync().Result;
                }
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarEstudiante(EstudianteEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/estudiante/editarEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarEstudiante(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Asistencia>> ListarAsistencia(AsistenciaQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                            return listaAsistencia;
                        }
                        listaAsistencia.Add(asistencia);
                    }
                    return listaAsistencia;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAsistencia(AsistenciaRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/asistencia/registrarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarAsistencia(AsistenciaEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/asistencia/editarRegistroAsistencia";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarAsistencia(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Carrera>> ListarCarrera(CarreraQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                            return carreras;
                        }
                        carreras.Add(carrera);
                    }
                    return carreras;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCarrera(CarreraRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/carrera/registrarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarCarrera(CarreraEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/carrera/editarCarrera";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCarrera(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Modulo>> ListarModulo(ModuloQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

               
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

                    return modulos;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarModulo(ModuloRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/modulo/registrarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarModulo(ModuloEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/modulo/editarModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarModulo(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Inventario>> ListarInventario(InventarioQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/inventario/listarPorNombre?Nombre={query.Nombre}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return inventario;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarInventario(InventarioRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/inventario/registrarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPost]
        public async Task<IActionResult> SincronizarInventario()
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/inventario/actualizarCantidadesInventario";

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarInventario(InventarioEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/inventario/editarInventario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarInventario(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Item>> ListarItem(ItemQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/item/listarItems?Rfid={query.Rfid}&Inventario={query.Inventario}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Item> items = JsonConvert.DeserializeObject<List<Item>>(response);

                    return items;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarItem(ItemRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/item/registrarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }
        
        [HttpPut]
        public async Task<IActionResult> EditarItem(ItemEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/item/editarItem";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarItem(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<PrestamoModulo>> ListarPrestamo(PrestamoModuloQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/listarPrestamosPorModulo?Modulo={query.Modulo}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoModulo> prestamos = JsonConvert.DeserializeObject<List<PrestamoModulo>>(response);

                    return prestamos;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamo(PrestamoModuloRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/registrarPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamoGrupo(PrestamoGrupoModuloRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/registrarGrupoPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarPrestamo(PrestamoEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/devolverPrestamoPorModulo";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPrestamo(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<PrestamoEstudiante>> ListarPrestamoLA(PrestamoEstudianteQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/listarPrestamosPorEstudiante?Matricula={query.Matricula}{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<PrestamoEstudiante> prestamos = JsonConvert.DeserializeObject<List<PrestamoEstudiante>>(response);

                    return prestamos;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPrestamoLA(PrestamoEstudianteRegistrarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/registrarPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PostAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpPut]
        public async Task<IActionResult> EditarPrestamoLA(PrestamoEditarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/prestamo/devolverPrestamoPorEstudiante";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Ok(response);
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarPrestamoLA(EliminarRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

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
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
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

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Usuario)
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

                    return Ok(response);
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
        public async Task<Usuario_> ObtenerUsuario(string user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/usuario/obtenerUsuario?Usuario={user}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Usuario_> usuarios = JsonConvert.DeserializeObject<List<Usuario_>>(response);

                    return usuarios[0];
                }
            }
            return null;
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioRequest request)
        {
            string response;
            string error;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/usuario/editarUsuario";
                stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, contentType);

                HttpResponseMessage res = await client.PutAsync(command, stringContent);

                if (res.IsSuccessStatusCode)
                {
                    response = res.Content.ReadAsStringAsync().Result;

                    return Json(new { redirectToUrl = Url.Action("Perfil", "Menu") });
                }
                error = res.Content.ReadAsStringAsync().Result;
            }
            return BadRequest(error);
        }

        [HttpGet]
        public async Task<List<Inventario>> GetInventario()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/inventario/listarPorNombre?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Inventario> inventario = JsonConvert.DeserializeObject<List<Inventario>>(response);

                    return inventario;
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<List<Carrera>> GetCarreras()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/carrera/listarTodos?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Carrera> carreras = JsonConvert.DeserializeObject<List<Carrera>>(response);

                    return carreras;
                }
            }
            return null;
        }

        [HttpGet]
        public async Task<List<Modulo>> GetModulos()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                command = $"api/modulo/listarTodos?{pagineo}";

                HttpResponseMessage res = await client.GetAsync(command);

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;

                    List<Modulo> modulos = JsonConvert.DeserializeObject<List<Modulo>>(response);

                    return modulos;
                }
            }
            return null;
        }
    }
}