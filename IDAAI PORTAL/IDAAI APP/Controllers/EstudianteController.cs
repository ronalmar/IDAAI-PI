using CsvHelper;
using IDAAI_APP.Models;
using IDAAI_APP.Models.Operations.Request;
using IDAAI_APP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Http;

namespace IDAAI_APP.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly ICSVService csvService;

        public EstudianteController(ICSVService csvService)
        {
            this.csvService = csvService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarEstudiantePorGrupo(EstudianteGrupoRegistrarRequest request)
        {

            IFormFile file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return BadRequest("Debe cargar un archivo válido CSV");
            }
            
            var datosArchivo = csvService.ReadCSV<GrupoEstudiante>(file.OpenReadStream());
            List<GrupoEstudiante> grupoEstudiantes = new List<GrupoEstudiante>();
            foreach (var estudiante in datosArchivo)
            {
                if (estudiante.Student.Trim().Contains(","))
                {
                    grupoEstudiantes.Add(estudiante);
                }
            }

            List<EstudianteRegistrarRequest> estudiantes = new List<EstudianteRegistrarRequest>();
            foreach (var estudiante in grupoEstudiantes)
            {
                estudiantes.Add(new EstudianteRegistrarRequest
                {
                    Nombres = estudiante.Student.Split(",")[0].Trim(),
                    Apellidos = estudiante.Student.Split(",")[1].Trim(),
                    Matricula = estudiante.SISUserID,
                    Email = estudiante.SISLoginID + "@espol.edu.ec",
                    Modulo = request.Modulo
                });
            }

            var operaciones = new OperacionesController();

            var claims = HttpContext.User.Claims.ToList();

            var respuesta = await operaciones.EnviarRegistroEstudianteGrupo(estudiantes, request.Modulo, claims) as ObjectResult;

            if(respuesta is null)
            {
                await HttpContext.SignOutAsync();
                return Json(new { redirectToUrl = Url.Action("Login", "Home") });
                //return BadRequest("Ocurrió un error en la ejecución");
            }
            if (respuesta.StatusCode.Equals(400))
            {
                return BadRequest(respuesta.Value);
            }
            else
            {
                return Ok(respuesta.Value);
            }            
        }
    }
}