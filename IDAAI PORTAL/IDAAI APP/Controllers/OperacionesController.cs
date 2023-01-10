using IDAAI_API.Entidades.Models;
using IDAAI_API.Entidades.Operations.Estudiante;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_APP.Models;
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
        public async Task<List<Estudiante>> ListarEstudiantes(EstudianteQuery query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                if (!string.IsNullOrEmpty(query.Matricula))
                {
                    command = $"api/estudiante/consultarPorMatricula?Matricula={query.Matricula}&Modulo={query.Modulo}{pagineo}";
                }
                else if (!string.IsNullOrEmpty(query.Email))
                {
                    command = $"api/estudiante/consultarPorEmail?Email={query.Email}&Modulo={query.Modulo}{pagineo}";
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

                    List<Estudiante> estudiantes = JsonConvert.DeserializeObject<List<Estudiante>>(response);

                    return estudiantes;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudiante(RegistrarEstudianteRequest request)
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
        public async Task<IActionResult> EditarEstudiante(EditarEstudianteRequest request)
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
        public async Task<List<Modulo>> ListarModulos(ModuloQuery query)
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