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
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using IDAAI_API.Entidades.Operations.Requests;

namespace IDAAI_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/inventario")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class InventarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public InventarioController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/inventario/listarPorNombre
        [HttpGet("listarPorNombre")]
        public async Task<ActionResult<List<InventarioDTO>>> ListarPorNombre(
            [FromQuery] InventarioNombreQuery query)
        {
            try
            {
                var result = await _context.Inventario
                    .FromSqlRaw($"EXEC sp_inventario @i_accion='CN', @i_nombre='{query.Nombre}', @i_usuario='{query.Usuario}'").ToListAsync();
                
                var resultPaginado = Paginacion<Inventario>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<InventarioDTO> listainventarioDTO = new();
                foreach (var inventario in resultPaginado)
                {
                    var inventarioDTO = mapper.Map<InventarioDTO>(inventario);
                    listainventarioDTO.Add(inventarioDTO);
                }
                return Ok(listainventarioDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/inventario/registrarInventario
        [HttpPost("registrarInventario")]
        public async Task<ActionResult<InventarioDTO>> RegistrarInventario(
            [FromBody] RegistrarInventarioRequest request)
        {
            try
            {
                var result = await _context.Inventario
                    .FromSqlRaw($"EXEC sp_inventario @i_accion='IN', @i_nombre='{request.Nombre}', @i_usuario='{request.Usuario}', @i_descripcion='{request.Descripcion}', @i_cantidadTotal='{request.CantidadTotal}'").ToListAsync();
                if(result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_21);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var inventarioDTO = mapper.Map<InventarioDTO>(result[0]);
                return Ok(inventarioDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/inventario/editarInventario
        [HttpPut("editarInventario")]
        public async Task<ActionResult<InventarioDTO>> EditarInventario(
            [FromBody] EditarInventarioRequest request)
        {
            try
            {
                var result = await _context.Inventario
                    .FromSqlRaw($"EXEC sp_inventario @i_accion='UP', @i_id='{request.Id}', @i_usuario='{request.Usuario}', @i_nombre='{request.Nombre}', @i_descripcion='{request.Descripcion}', @i_cantidadDisponible='{request.CantidadDisponible}', @i_cantidadTotal='{request.CantidadTotal}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_22);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_21);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_51);
                }

                var inventarioDTO = mapper.Map<InventarioDTO>(result[0]);
                return Ok(inventarioDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/inventario/eliminarInventario
        [HttpDelete("eliminarInventario")]
        public async Task<ActionResult<InventarioDTO>> EliminarInventario(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.Inventario
                    .FromSqlRaw($"EXEC sp_inventario @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count <1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_22);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_50);
                }

                var inventarioDTO = mapper.Map<InventarioDTO>(result[0]);
                return Ok(inventarioDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/inventario/actualizarCantidadesInventario
        [HttpPost("actualizarCantidadesInventario")]
        public async Task<ActionResult<InventarioDTO>> ActualizarCantidadesInventario()
        {
            try
            {
                var result = await _context.Inventario
                    .FromSqlRaw($"EXEC sp_inventario @i_accion='VE'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return Ok(Mensajes.ERROR_VAL_23);
                }
                var inventarioDTO = mapper.Map<InventarioDTO>(result[0]);
                return BadRequest(inventarioDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}