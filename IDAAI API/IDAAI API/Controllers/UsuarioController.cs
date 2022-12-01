using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Services;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IDAAI_API.Controllers
{
    [ApiController]
    [Route("api/usuario")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // api/usuario/login
        [HttpGet("login")]
        public async Task<ActionResult> Login(
            [FromQuery] LoginQuery query)
        {
            try
            {
                string passwordEncriptado = Encrypt.GetSHA256(query.Password);

                var result = await _context.Autenticacion
                    .FromSqlRaw($"EXEC sp_usuario @i_accion='LG', @i_usuario='{query.Usuario}', @i_password='{passwordEncriptado}'").ToListAsync();
                
                if (result.Count > 0)
                    return Ok(result[0]);
                return NotFound(Mensajes.ERROR_VAL_05);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/usuario/registro
        [HttpPost("registro")]
        public async Task<ActionResult> Registro(
            [FromBody] RegistroRequest request)
        {
            try
            {
                string passwordEncriptado = Encrypt.GetSHA256(request.Password);

                var result = await _context.Autenticacion
                    .FromSqlRaw($"EXEC sp_usuario @i_accion='RG', @i_usuario='{request.Usuario}', @i_password='{passwordEncriptado}', @i_email='{request.Email}'").ToListAsync();

                if (result.Count > 0)
                    return Ok(result[0]);
                return BadRequest(Mensajes.ERROR_VAL_06);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
