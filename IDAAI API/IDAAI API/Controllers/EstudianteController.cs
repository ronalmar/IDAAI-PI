using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.Entidades.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Dapper;
using Microsoft.Data.SqlClient;
using IDAAI_API.Entidades.Operations.Estudiante;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Utils;
using IDAAI_API.DTOs;
using AutoMapper;
using IDAAI_API.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using IDAAI_API.Entidades.Operations.Requests;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IDAAI_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/estudiante")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class EstudianteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        private readonly ICSVService csvService;

        public EstudianteController(ApplicationDbContext context, IMapper mapper, ICSVService csvService)
        {
            _context = context;
            this.mapper = mapper;
            this.csvService = csvService;
        }

        // api/estudiante/listarPorNombres
        [HttpGet("listarPorNombres")]
        public async Task<ActionResult<List<EstudianteDTO>>> ListarPorNombres(
            [FromQuery] EstudianteNombresQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CN', @i_usuario='{query.Usuario}', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();
                
                var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorCarrera
        [HttpGet("listarPorCarrera")]
        public async Task<ActionResult<EstudianteDTO>> ListarPorCarrera(
            [FromQuery] EstudianteCarreraQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CC', @i_usuario='{query.Usuario}', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                
                var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);             
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorModulo
        [HttpGet("listarPorModulo")]
        public async Task<ActionResult<EstudianteDTO>> ListarPorModulo(
           [FromQuery] EstudianteModuloQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CL', @i_usuario='{query.Usuario}', @i_modulo='{query.Modulo}'").ToListAsync();
               
                var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);              
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<EstudianteDTO>> ListarTodos(
            [FromQuery] EstudianteTodosQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CT', @i_usuario='{query.Usuario}'").ToListAsync();

                var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);              
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorMatricula
        [HttpGet("consultarPorMatricula")]
        public async Task<ActionResult<EstudianteDTO>> ConsultarPorMatricula(
            [FromQuery] EstudianteMatriculaQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CM', @i_usuario='{query.Usuario}', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

                EstudianteDTO estudianteDTO = new();
                if(result.Count == 0)
                {
                    return Ok();
                }
                estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorEmail
        [HttpGet("consultarPorEmail")]
        public async Task<ActionResult<EstudianteDTO>> ConsultarPorEmail(
            [FromQuery] EstudianteEmailQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CE', @i_usuario='{query.Usuario}', @i_email='{query.Email}', @i_modulo='{query.Modulo}'").ToListAsync();

                EstudianteDTO estudianteDTO = new();
                if (result.Count == 0)
                {
                    return Ok();
                }
                estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);             
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/registrarEstudiante
        [HttpPost("registrarEstudiante")]
        public async Task<ActionResult<EstudianteDTO>> RegistrarEstudiante(
            [FromBody] RegistrarEstudianteRequest request)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='IN', @i_usuario='{request.Usuario}', @i_nombres='{request.Nombres}', @i_apellidos='{request.Apellidos}', @i_matricula='{request.Matricula}'" +
                                    $", @i_email='{request.Email}', @i_direccion='{request.Direccion}', @i_carrera='{request.Carrera}', @i_modulo='{request.Modulo}'").ToListAsync();
                if(result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_07);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_12);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/registrarGrupoEstudiante
        [HttpPost("registrarGrupoEstudiante")]
        public async Task<ActionResult<EstudianteDTO>> RegistrarGrupoEstudiante(
            [FromForm] IFormFile Archivo, [FromQuery] EstudianteModuloQuery query)
        {
            try
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();

                if (Archivo == null)
                {
                    return BadRequest(Mensajes.ERROR_VAL_38);
                }

                var datosArchivo = csvService.ReadCSV<GrupoEstudiante>(Archivo.OpenReadStream());
                List<GrupoEstudiante> grupoEstudiantes = new List<GrupoEstudiante>();
                foreach(var estudiante in datosArchivo)
                {
                    if (estudiante.Student.Trim().Contains(","))
                    {
                        grupoEstudiantes.Add(estudiante);
                    }
                }

                List<RegistrarEstudianteRequest> estudiantes= new List<RegistrarEstudianteRequest>();
                foreach (var estudiante in grupoEstudiantes)
                {
                    estudiantes.Add(new RegistrarEstudianteRequest {
                        Nombres = estudiante.Student.Split(",")[0].Trim(),
                        Apellidos = estudiante.Student.Split(",")[1].Trim(),
                        Matricula = estudiante.SISUserID,
                        Email = estudiante.SISLoginID + "@espol.edu.ec",
                        Modulo = query.Modulo
                    });
                }

                XDocument xmlEstudiantes = new(
                    new XElement("Estudiantes", estudiantes.Select(estudiante => 
                        new XElement("Estudiante", 
                            new XElement("Nombres", estudiante.Nombres),
                            new XElement("Apellidos", estudiante.Apellidos),
                            new XElement("Matricula", estudiante.Matricula),
                            new XElement("Email", estudiante.Email),
                            new XElement("Modulo", estudiante.Modulo),
                            new XElement("Carrera", estudiante.Carrera),
                            new XElement("Direccion", estudiante.Direccion))))
                );
                SqlXml xml = new(xmlEstudiantes.CreateReader());

                var result = await _context.Estudiantes
                   .FromSqlRaw($"EXEC sp_estudiante @i_accion='IG', @i_usuario='{query.Usuario}', @i_modulo='{query.Modulo}', @i_xmlEstudiantes='{xmlEstudiantes}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0 && result.Count == 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -1 && result[0].Nombres == "Usuario")
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/procesarRegistroGrupoEstudiante
        [HttpPost("procesarRegistroGrupoEstudiante")]
        public async Task<ActionResult<EstudianteDTO>> ProcesarRegistroGrupoEstudiante(
            [FromBody] RegistrarGrupoEstudianteRequest request)
        {
            try
            {                
                if (request is null)
                {
                    return BadRequest();
                }

                var estudiantes = request.Estudiantes;

                XDocument xmlEstudiantes = new(
                    new XElement("Estudiantes", estudiantes.Select(estudiante =>
                        new XElement("Estudiante",
                            new XElement("Nombres", estudiante.Nombres),
                            new XElement("Apellidos", estudiante.Apellidos),
                            new XElement("Matricula", estudiante.Matricula),
                            new XElement("Email", estudiante.Email),
                            new XElement("Modulo", estudiante.Modulo),
                            new XElement("Carrera", estudiante.Carrera),
                            new XElement("Direccion", estudiante.Direccion))))
                );
                SqlXml xml = new(xmlEstudiantes.CreateReader());

                var result = await _context.Estudiantes
                   .FromSqlRaw($"EXEC sp_estudiante @i_accion='IG', @i_usuario='{request.Usuario}', @i_modulo='{request.Modulo}', @i_xmlEstudiantes='{xmlEstudiantes}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0 && result.Count == 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -1 && result[0].Nombres == "Usuario")
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var resultPaginado = Paginacion<Estudiante>.Paginar(result, 1, 100000);
                List<EstudianteDTO> listaEstudiantesDTO = new();
                foreach (var estudiante in resultPaginado)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                    listaEstudiantesDTO.Add(estudianteDTO);
                }
                return Ok(listaEstudiantesDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/editarEstudiante
        [HttpPut("editarEstudiante")]
        public async Task<ActionResult<EstudianteDTO>> EditarEstudiante(
            [FromBody] EditarEstudianteRequest request)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='UP', @i_usuario='{request.Usuario}', @i_id='{request.Id}', @i_nombres='{request.Nombres}', @i_apellidos='{request.Apellidos}', @i_matricula='{request.Matricula}'" +
                                    $", @i_email='{request.Email}', @i_direccion='{request.Direccion}', @i_carrera='{request.Carrera}', @i_modulo='{request.Modulo}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_09);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_07);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/eliminarEstudiante
        [HttpDelete("eliminarEstudiante")]
        public async Task<ActionResult<EstudianteDTO>> EliminarEstudiante(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_20);
                }
                var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}