using Azure.Core;
using IDAAI_API.Contexts;
using IDAAI_API.Entidades.Operations.Consultas;
using IDAAI_API.Entidades.Operations.Requests;
using IDAAI_API.Services;
using IDAAI_API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
        private readonly string secretKey;

        public UsuarioController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
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
                {
                    var token = ConstruirToken(query);
                    return Ok(token);
                }                    
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = ""});
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
                {
                    LoginQuery credencialesUsuario = new LoginQuery
                    {
                        Usuario = request.Usuario,
                        Password = request.Password,
                    };
                    var token = ConstruirToken(credencialesUsuario);
                    return Ok(token);
                }                    
                return BadRequest(Mensajes.ERROR_VAL_06);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/usuario/renovarToken
        [HttpGet("renovarToken")]
        [Authorize]
        public ActionResult<RespuestaAutenticacion> RenovarToken()
        {
            var usuarioClaim = HttpContext.User.Claims.Where(claim => claim.Type == "Usuario").FirstOrDefault();
            var usuario = usuarioClaim.Value;
            var credencialesUsuario = new LoginQuery
            {
                Usuario = usuario,
            };
            return ConstruirToken(credencialesUsuario);
        }

        private RespuestaAutenticacion ConstruirToken(LoginQuery credencialesUsuario)
        {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim("Usuario", credencialesUsuario.Usuario));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(240),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);

            RespuestaAutenticacion respuestaAutenticacion = new RespuestaAutenticacion
            {
                Token = tokenCreado,
                Expiracion = tokenDescriptor.Expires
            };

            return respuestaAutenticacion;
        }
    }
}
