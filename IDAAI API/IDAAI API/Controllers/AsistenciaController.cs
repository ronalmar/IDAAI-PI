using IDAAI_API.Contexts;
using IDAAI_API.Entidades.Models;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Estudiante;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IDAAI_API.Controllers
{
    [ApiController]
    [Route("api/asistencia")]
    public class AsistenciaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AsistenciaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // api/asistencia/listarPorNombres
        [HttpGet("listarPorNombres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Estudiante>> listarPorNombres(
            [FromQuery] NombresQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CN', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarPorCarrera
        [HttpGet("listarPorCarrera")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Estudiante>> listarPorCarrera(
            [FromQuery] CarreraQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CC', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                if (result.Count > 0)
                    return Ok(result);
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarPorModulo
        [HttpGet("listarPorModulo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Estudiante>> listarPorModulo(
           [FromQuery] ModuloQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CL', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/listarTodos
        [HttpGet("listarTodos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Estudiante>> listarTodos()
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CT'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/asistencia/consultarPorMatricula
        [HttpGet("consultarPorMatricula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Estudiante>> consultarPorMatricula(
            [FromQuery] MatriculaQuery query)
        {
            try
            {
                var result = await _context.RegistroAsistencia
                    .FromSqlRaw($"EXEC sp_registroasistencia @i_accion='CM', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result[0]);
                return NotFound(Mensajes.ERROR_VAL_04);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
