using AutoMapper;
using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.DTOs;
using IDAAI_API.Entidades.Models;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Services;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IDAAI_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/modulo")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ModuloController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public ModuloController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/modulo/listarPorNombre
        [HttpGet("listarPorNombre")]
        public async Task<ActionResult<List<ModuloDTO>>> ListarPorNombre(
            [FromQuery] ModuloNombreQuery query)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='CN', @i_usuario='{query.Usuario}', @i_nombre='{query.Nombre}'").ToListAsync();

                var resultPaginado = Paginacion<Modulo>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<ModuloDTO> listaModuloDTO = new();
                foreach (var modulo in resultPaginado)
                {
                    var moduloDTO = mapper.Map<ModuloDTO>(modulo);
                    listaModuloDTO.Add(moduloDTO);
                }
                return Ok(listaModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/modulo/listarPorPeriodoAcademico
        [HttpGet("listarPorPeriodoAcademico")]
        public async Task<ActionResult<List<ModuloDTO>>> ListarPorPeriodoAcademico(
            [FromQuery] ModuloPeriodoQuery query)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='CP', @i_usuario='{query.Usuario}', @i_periodoAcademico='{query.PeriodoAcademico}'").ToListAsync();

                var resultPaginado = Paginacion<Modulo>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<ModuloDTO> listaModuloDTO = new();
                foreach (var modulo in resultPaginado)
                {
                    var moduloDTO = mapper.Map<ModuloDTO>(modulo);
                    listaModuloDTO.Add(moduloDTO);
                }
                return Ok(listaModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/modulo/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<List<ModuloDTO>>> ListarTodos(
            [FromQuery] ModuloUsuarioQuery query)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='CT', @i_usuario='{query.Usuario}'").ToListAsync();

                var resultPaginado = Paginacion<Modulo>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<ModuloDTO> listaModuloDTO = new();
                foreach (var modulo in resultPaginado)
                {
                    var moduloDTO = mapper.Map<ModuloDTO>(modulo);
                    listaModuloDTO.Add(moduloDTO);
                }
                return Ok(listaModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/modulo/registrarModulo
        [HttpPost("registrarModulo")]
        public async Task<ActionResult<ModuloDTO>> RegistrarModulo(
            [FromBody] RegistrarModuloRequest request)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='IN', @i_nombre='{request.Nombre}', @i_diasClase='{request.DiasClase}', @i_descripcion='{request.Descripcion}', @i_usuario='{request.Usuario}', @i_periodoAcademico='{request.PeriodoAcademico}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_14);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var moduloDTO = mapper.Map<ModuloDTO>(result[0]);
                return Ok(moduloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/modulo/editarModulo
        [HttpPut("editarModulo")]
        public async Task<ActionResult<ModuloDTO>> EditarModulo(
            [FromBody] EditarModuloRequest request)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='UP', @i_usuario='{request.Usuario}', @i_nombre='{request.Nombre}', @i_descripcion='{request.Descripcion}', @i_periodoAcademico='{request.PeriodoAcademico}', @i_diasClase='{request.DiasClase}', @i_id='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_14);
                }

                var moduloDTO = mapper.Map<ModuloDTO>(result[0]);
                return Ok(moduloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/modulo/eliminarModulo
        [HttpDelete("eliminarModulo")]
        public async Task<ActionResult<ModuloDTO>> EliminarModulo(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                var moduloDTO = mapper.Map<ModuloDTO>(result[0]);
                return Ok(moduloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
