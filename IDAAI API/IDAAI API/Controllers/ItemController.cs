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
    //[Authorize]
    [ApiController]
    [Route("api/item")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public ItemController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/item/listarItems
        [HttpGet("listarItems")]
        public async Task<ActionResult<List<ItemDTO>>> ListarItems(
            [FromQuery] ItemQuery query)
        {
            try
            {
                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='CN', @i_rfid='{query.Rfid}', @i_inventario='{query.Inventario}'").ToListAsync();
                
                var resultPaginado = Paginacion<Item>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<ItemDTO> listaitemDTO = new();
                foreach (var item in resultPaginado)
                {
                    var itemDTO = mapper.Map<ItemDTO>(item);
                    listaitemDTO.Add(itemDTO);
                }
                return Ok(listaitemDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/item/registrarItem
        [HttpPost("registrarItem")]
        public async Task<ActionResult<ItemDTO>> RegistrarItem(
            [FromBody] RegistrarItemRequest request)
        {
            try
            {
                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='IN', @i_rfid='{request.Rfid}', @i_inventario='{request.Inventario}'").ToListAsync();
                if(result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_24);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_25);
                }
                var itemDTO = mapper.Map<ItemDTO>(result[0]);
                return Ok(itemDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/item/editarItem
        [HttpPut("editarItem")]
        public async Task<ActionResult<ItemDTO>> EditarItem(
            [FromBody] EditarItemRequest request)
        {
            try
            {
                var estaDisponible = 0;
                if (request.EstaDisponible.HasValue)
                {
                    if (request.EstaDisponible == true)
                    {
                        estaDisponible = 1;
                    }
                    else
                    {
                        estaDisponible = 2;
                    }
                }              
                else{
                    estaDisponible = 0;
                }

                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='UP', @i_id='{request.Id}', @i_rfid='{request.Rfid}', @i_inventario='{request.Inventario}', @i_estadoItemId='{estaDisponible}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_26);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_25);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_27);
                }
                var itemDTO = mapper.Map<ItemDTO>(result[0]);
                return Ok(itemDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/item/eliminarItem
        [HttpDelete("eliminarItem")]
        public async Task<ActionResult<ItemDTO>> EliminarItem(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count <1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_28);
                }
                var itemDTO = mapper.Map<ItemDTO>(result[0]);
                return Ok(itemDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}