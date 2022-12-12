using AutoMapper;
using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.DTOs;
using IDAAI_API.Entidades.Models;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Services;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IDAAI_API.Controllers
{
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
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='CN', @i_nombre='{query.Nombre}'").ToListAsync();

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
            [FromQuery] PaginacionQuery query)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='CT'").ToListAsync();

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
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='IN', @i_nombre='{request.Nombre}', @i_descripcion='{request.Descripcion}'").ToListAsync();

                if (result.Count == 0)
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

        // api/modulo/editarModulo
        [HttpPut("editarModulo")]
        public async Task<ActionResult<ModuloDTO>> EditarModulo(
            [FromBody] EditarModuloRequest request)
        {
            try
            {
                var result = await _context.Modulos
                    .FromSqlRaw($"EXEC sp_modulo @i_accion='UP', @i_nombre='{request.Nombre}', @i_descripcion='{request.Descripcion}', @i_id='{request.Id}'").ToListAsync();

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
