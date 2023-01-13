using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations;
using IDAAI_APP.Models.Operations.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Mime;
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
                else if (!string.IsNullOrEmpty(query.Carrera))
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
                else if (!string.IsNullOrEmpty(query.Carrera))
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