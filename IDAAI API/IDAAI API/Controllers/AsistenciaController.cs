using AutoMapper;
using IDAAI_API.Contexts;
using IDAAI_API.DTOs;
using IDAAI_API.Entidades.Models;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Estudiante;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Services;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IDAAI_API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/asistencia")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AsistenciaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public AsistenciaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/asistencia/listarPorNombres
        [HttpGet("listarPorNombres")]
        public async Task<ActionResult<AsistenciaDTO>> ListarPorNombres(
            [FromQuery] EstudianteNombresQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CN', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();
                
                var resultPaginado = Paginacion<Asistencia>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<AsistenciaDTO> listaAsistenciaDTO = new();
                foreach (var asistencia in resultPaginado)
                {
                    var asistenciaDTO = mapper.Map<AsistenciaDTO>(asistencia);
                    listaAsistenciaDTO.Add(asistenciaDTO);
                }
                return Ok(listaAsistenciaDTO);             
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarPorCarrera
        [HttpGet("listarPorCarrera")]
        public async Task<ActionResult<AsistenciaDTO>> ListarPorCarrera(
            [FromQuery] EstudianteCarreraQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CC', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                
                var resultPaginado = Paginacion<Asistencia>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<AsistenciaDTO> listaAsistenciaDTO = new();
                foreach (var asistencia in resultPaginado)
                {
                    var asistenciaDTO = mapper.Map<AsistenciaDTO>(asistencia);
                    listaAsistenciaDTO.Add(asistenciaDTO);
                }
                return Ok(listaAsistenciaDTO);             
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarPorModulo
        [HttpGet("listarPorModulo")]
        public async Task<ActionResult<AsistenciaDTO>> ListarPorModulo(
           [FromQuery] EstudianteModuloQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CL', @i_modulo='{query.Modulo}'").ToListAsync();
              
                var resultPaginado = Paginacion<Asistencia>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<AsistenciaDTO> listaAsistenciaDTO = new();
                foreach (var asistencia in resultPaginado)
                {
                    var asistenciaDTO = mapper.Map<AsistenciaDTO>(asistencia);
                    listaAsistenciaDTO.Add(asistenciaDTO);
                }
                return Ok(listaAsistenciaDTO);            
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<AsistenciaDTO>> ListarTodos(
            [FromQuery] PaginacionQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CT'").ToListAsync();
               
                var resultPaginado = Paginacion<Asistencia>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<AsistenciaDTO> listaAsistenciaDTO = new();
                foreach (var asistencia in resultPaginado)
                {
                    var asistenciaDTO = mapper.Map<AsistenciaDTO>(asistencia);
                    listaAsistenciaDTO.Add(asistenciaDTO);
                }
                return Ok(listaAsistenciaDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/consultarPorMatricula
        [HttpGet("consultarPorMatricula")]
        public async Task<ActionResult<AsistenciaDTO>> ConsultarPorMatricula(
            [FromQuery] EstudianteMatriculaQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CM', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

                AsistenciaDTO asistenciaDTO = new();
                if (result.Count == 0)
                {
                    return Ok();
                }
                asistenciaDTO = mapper.Map<AsistenciaDTO>(result[0]);
                return Ok(asistenciaDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/registrarRegistroAsistencia
        [HttpPost("registrarRegistroAsistencia")]
        public async Task<ActionResult<AsistenciaDTO>> RegistrarRegistroAsistencia(
            [FromBody] RegistrarRegistroAsistenciaRequest request)
        {
            try
            {
                List<Asistencia> result;
                if (request.EsAsistencia == true)
                {
                    result = await _context.RegistroAsistencia
                        .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='IA', @i_matricula='{request.Matricula}', @i_fecha='{request.Fecha}', @i_modulo='{request.Modulo}'").ToListAsync();
                }
                else
                {
                    result = await _context.RegistroAsistencia
                     .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='IF', @i_matricula='{request.Matricula}', @i_fecha='{request.Fecha}', @i_modulo='{request.Modulo}'").ToListAsync();
                }
                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_11);
                }
                var asistenciaDTO = mapper.Map<AsistenciaDTO>(result[0]);
                return Ok(asistenciaDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/editarRegistroAsistencia
        [HttpPut("editarRegistroAsistencia")]
        public async Task<ActionResult<AsistenciaDTO>> EditarRegistroAsistencia(
            [FromBody] EditarRegistroAsistenciaRequest request)
        {
            try
            {
                List<Asistencia> result;
                if (request.EsAsistencia == true)
                {
                    result = await _context.RegistroAsistencia
                        .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='UA', @i_idRegistroAsistencia='{request.Id}'").ToListAsync();
                }
                else
                {
                    result = await _context.RegistroAsistencia
                     .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='UF', @i_idRegistroAsistencia='{request.Id}'").ToListAsync();
                }
                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_10);
                }
                var asistenciaDTO = mapper.Map<AsistenciaDTO>(result[0]);
                return Ok(asistenciaDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/eliminarRegistroAsistencia
        [HttpDelete("eliminarRegistroAsistencia")]
        public async Task<ActionResult<AsistenciaDTO>> EliminarRegistroAsistencia(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='DE', @i_idRegistroAsistencia='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_10);
                }
                var asistenciaDTO = mapper.Map<AsistenciaDTO>(result[0]);
                return Ok(asistenciaDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
