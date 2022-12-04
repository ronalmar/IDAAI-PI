using AutoMapper;
using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.DTOs;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    return BadRequest(Mensajes.ERROR_VAL_15);
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
