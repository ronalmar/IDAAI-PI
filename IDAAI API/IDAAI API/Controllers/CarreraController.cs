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
    [Route("api/carrera")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CarreraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public CarreraController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/carrera/listarPorNombre
        [HttpGet("listarPorNombre")]
        public async Task<ActionResult<List<CarreraDTO>>> ListarPorNombre(
            [FromQuery] CarreraNombreQuery query)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='CN', @i_usuario='{query.Usuario}', @i_nombre='{query.Nombre}', @i_modulo='{query.Modulo}'").ToListAsync();

                var resultPaginado = Paginacion<Carrera>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<CarreraDTO> listaCarreraDTO = new();
                foreach (var carrera in resultPaginado)
                {
                    var carreraDTO = mapper.Map<CarreraDTO>(carrera);
                    listaCarreraDTO.Add(carreraDTO);
                }
                return Ok(listaCarreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/carrera/listarPorModulo
        [HttpGet("listarPorModulo")]
        public async Task<ActionResult<List<CarreraDTO>>> ListarPorModulo(
            [FromQuery] CarreraModuloQuery query)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='CL', @i_usuario='{query.Usuario}', @i_modulo='{query.Modulo}'").ToListAsync();

                var resultPaginado = Paginacion<Carrera>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<CarreraDTO> listaCarreraDTO = new();
                foreach (var carrera in resultPaginado)
                {
                    var carreraDTO = mapper.Map<CarreraDTO>(carrera);
                    listaCarreraDTO.Add(carreraDTO);
                }
                return Ok(listaCarreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/carrera/listarTodos
        [HttpGet("listarTodos")]
        public async Task<ActionResult<List<CarreraDTO>>> ListarTodos(
            [FromQuery] CarreraTodosQuery query)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='CT', @i_usuario='{query.Usuario}'").ToListAsync();

                var resultPaginado = Paginacion<Carrera>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<CarreraDTO> listaCarreraDTO = new();
                foreach (var carrera in resultPaginado)
                {
                    var carreraDTO = mapper.Map<CarreraDTO>(carrera);
                    listaCarreraDTO.Add(carreraDTO);
                }
                return Ok(listaCarreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/carrera/registrarCarrera
        [HttpPost("registrarCarrera")]
        public async Task<ActionResult<CarreraDTO>> RegistrarCarrera(
            [FromBody] RegistrarCarreraRequest request)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='IN', @i_usuario='{request.Usuario}', @i_nombre='{request.Nombre}', @i_modulo='{request.Modulo}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_17);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                var carreraDTO = mapper.Map<CarreraDTO>(result[0]);
                return Ok(carreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/carrera/editarCarrera
        [HttpPut("editarCarrera")]
        public async Task<ActionResult<CarreraDTO>> EditarCarrera(
            [FromBody] EditarCarreraRequest request)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='UP', @i_usuario='{request.Usuario}', @i_nombre='{request.Nombre}', @i_modulo='{request.Modulo}', @i_id='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_18);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_17);
                }
                var carreraDTO = mapper.Map<CarreraDTO>(result[0]);
                return Ok(carreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/carrera/eliminarCarrera
        [HttpDelete("eliminarCarrera")]
        public async Task<ActionResult<CarreraDTO>> EliminarCarrera(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.Carreras
                    .FromSqlRaw($"EXEC sp_carrera @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_12);
                }
                var carreraDTO = mapper.Map<CarreraDTO>(result[0]);
                return Ok(carreraDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
