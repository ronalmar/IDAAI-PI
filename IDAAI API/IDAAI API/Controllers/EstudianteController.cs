using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.Entidades.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Dapper;
using Microsoft.Data.SqlClient;
using IDAAI_API.Entidades.Operations.Estudiante;
using IDAAI_API.Entidades.Operations;

namespace IDAAI_API.Controllers
{
    [ApiController]
    [Route("api/estudiante")]
    public class EstudianteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstudianteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // api/estudiante/listarPorNombres
        [HttpGet("listarPorNombres")]
        public async Task<ActionResult<Estudiante>> listarPorNombres(
            [FromQuery] NombresQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CN', @i_nombres='{query.Nombres}', @i_apellidos='{query.Apellidos}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorCarrera
        [HttpGet("listarPorCarrera")]
        public async Task<ActionResult<Estudiante>> listarPorCarrera(
            [FromQuery] CarreraQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CC', @i_carrera='{query.Carrera}', @i_modulo='{query.Modulo}'").ToListAsync();
                if(result.Count > 0)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarPorModulo
        [HttpGet("listarPorModulo")]
        public async Task<ActionResult<Estudiante>> listarPorModulo(
           [FromQuery] ModuloQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CL', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<Estudiante>> listarTodos()
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CT'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorMatricula
        [HttpGet("consultarPorMatricula")]
        public async Task<ActionResult<Estudiante>> consultarPorMatricula(
            [FromQuery] MatriculaQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CM', @i_matricula='{query.Matricula}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result[0]);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/estudiante/consultarPorEmail
        [HttpGet("consultarPorEmail")]
        public async Task<ActionResult<Estudiante>> consultarPorEmail(
            [FromQuery] EmailQuery query)
        {
            try
            {
                var result = await _context.Estudiantes
                    .FromSqlRaw($"EXEC sp_estudiante @i_accion='CE', @i_email='{query.Email}', @i_modulo='{query.Modulo}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result[0]);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}