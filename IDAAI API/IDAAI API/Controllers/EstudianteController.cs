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

namespace IDAAI_API.Controllers
{
    //[Authorize]
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

        public EstudianteController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/estudiante/listarPorNombres
        [HttpGet("listarPorNombres")]
        public async Task<ActionResult<List<EstudianteDTO>>> ListarPorNombres(
            [FromQuery] EstudianteNombresQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CN', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();
                
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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CC', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                
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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CL', @i_modulo='{query.Modulo}'").ToListAsync();
               
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
            [FromQuery] PaginacionQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CT'").ToListAsync();

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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CM', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CE', @i_email='{query.Email}', @i_modulo='{query.Modulo}'").ToListAsync();

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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='IN', @i_nombres='{request.Nombres}', @i_apellidos='{request.Apellidos}', @i_matricula='{request.Matricula}'" +
                                    $", @i_email='{request.Email}', @i_direccion='{request.Direccion}', @i_carrera='{request.Carrera}', @i_modulo='{request.Modulo}'").ToListAsync();
                
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
                var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                return Ok(estudianteDTO);               
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
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='UP', @i_id='{request.Id}', @i_nombres='{request.Nombres}', @i_apellidos='{request.Apellidos}', @i_matricula='{request.Matricula}'" +
                                    $", @i_email='{request.Email}', @i_direccion='{request.Direccion}', @i_carrera='{request.Carrera}', @i_modulo='{request.Modulo}'").ToListAsync();

                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_09);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_12);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
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