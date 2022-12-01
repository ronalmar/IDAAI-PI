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

namespace IDAAI_API.Controllers
{
    [ApiController]
    [Route("api/estudiante")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            [FromQuery] NombresQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CN', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();
                if (result.Count > 0)
                {
                    var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                    List<EstudianteDTO> listaEstudiantesDTO = new();
                    foreach (var estudiante in resultPaginado)
                    {
                        var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                        listaEstudiantesDTO.Add(estudianteDTO);
                    }
                    return Ok(listaEstudiantesDTO);
                }
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorCarrera
        [HttpGet("listarPorCarrera")]
        public async Task<ActionResult<Estudiante>> ListarPorCarrera(
            [FromQuery] CarreraQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CC', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                if (result.Count > 0) 
                {
                    var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                    List<EstudianteDTO> listaEstudiantesDTO = new();
                    foreach (var estudiante in resultPaginado)
                    {
                        var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                        listaEstudiantesDTO.Add(estudianteDTO);
                    }
                    return Ok(listaEstudiantesDTO);
                }
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorModulo
        [HttpGet("listarPorModulo")]
        public async Task<ActionResult<Estudiante>> ListarPorModulo(
           [FromQuery] ModuloQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CL', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                {
                    var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                    List<EstudianteDTO> listaEstudiantesDTO = new();
                    foreach (var estudiante in resultPaginado)
                    {
                        var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                        listaEstudiantesDTO.Add(estudianteDTO);
                    }
                    return Ok(listaEstudiantesDTO);
                }
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<Estudiante>> ListarTodos(
            [FromQuery] PaginacionQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CT'").ToListAsync();


                if (result.Count > 0)
                {
                    var resultPaginado = Paginacion<Estudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                    List<EstudianteDTO> listaEstudiantesDTO = new();
                    foreach (var estudiante in resultPaginado)
                    {
                        var estudianteDTO = mapper.Map<EstudianteDTO>(estudiante);
                        listaEstudiantesDTO.Add(estudianteDTO);
                    }
                    return Ok(listaEstudiantesDTO);
                }
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorMatricula
        [HttpGet("consultarPorMatricula")]
        public async Task<ActionResult<Estudiante>> ConsultarPorMatricula(
            [FromQuery] MatriculaQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CM', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                    return Ok(estudianteDTO);
                }                  
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorEmail
        [HttpGet("consultarPorEmail")]
        public async Task<ActionResult<Estudiante>> ConsultarPorEmail(
            [FromQuery] EmailQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CE', @i_email='{query.Email}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                {
                    var estudianteDTO = mapper.Map<EstudianteDTO>(result[0]);
                    return Ok(estudianteDTO);
                }
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}