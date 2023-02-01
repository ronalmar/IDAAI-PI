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
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace IDAAI_API.Controllers
{
    [Authorize]
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
                    .FromSqlRaw($"EXEC sp_item @i_accion='CN', @i_rfid='{query.Rfid}', @i_usuario='{query.Usuario}', @i_inventario='{query.Inventario}'").ToListAsync();
                
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
                    .FromSqlRaw($"EXEC sp_item @i_accion='IN', @i_rfid='{request.Rfid}', @i_usuario='{request.Usuario
                    }', @i_inventario='{request.Inventario}'").ToListAsync();
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
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
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
                    .FromSqlRaw($"EXEC sp_item @i_accion='UP', @i_id='{request.Id}', @i_usuario='{request.Usuario}', @i_rfid='{request.Rfid}', @i_inventario='{request.Inventario}', @i_estadoItemId='{estaDisponible}'").ToListAsync();

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
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
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

        [AllowAnonymous]
        // api/item/enviarItemsDetectados
        [HttpPost("enviarItemsDetectados")]
        public async Task<ActionResult<ItemDTO>> EnviarItemsDetectados(
            [FromBody] GrupoItemsDetectadosRequest request)
        {
            try
            {
                XDocument xmlRfids = new(
                   new XElement("Items", request.Rfids.Select(rfid => new XElement("Item", new XElement("Rfid", rfid))))
                );
                SqlXml xml = new(xmlRfids.CreateReader());

                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='EI', @i_grupoItemsDetectados='{xmlRfids}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_41);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_42);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_48);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_49);
                }

                var resultPaginado = Paginacion<Item>.Paginar(result, 1, 100000);
                List<ItemDTO> listaItemsDTO = new();
                foreach (var item in resultPaginado)
                {
                    var itemDTO = mapper.Map<ItemDTO>(item);
                    listaItemsDTO.Add(itemDTO);
                }
                return Ok(listaItemsDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        // api/item/enviarItemsDetectadosRfid
        [HttpPost("enviarItemsDetectadosRfid")]
        public async Task<ActionResult<ItemDTO>> EnviarItemsDetectadosRfid(
           [FromBody] GrupoItemsDetectadosRfidRequest request)
        {
            try
            {
                List<string> rfids = new();

                foreach (var dato in request.Rfids)
                {
                    var rfid = dato.Rfid;
                    rfids.Add(rfid);
                }

                XDocument xmlRfids = new(
                   new XElement("Items", rfids.Select(rfid => new XElement("Item", new XElement("Rfid", rfid))))
                );
                SqlXml xml = new(xmlRfids.CreateReader());

                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='EI', @i_grupoItemsDetectados='{xmlRfids}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_41);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_42);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_48);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_49);
                }

                var resultPaginado = Paginacion<Item>.Paginar(result, 1, 100000);
                List<ItemDTO> listaItemsDTO = new();
                foreach (var item in resultPaginado)
                {
                    var itemDTO = mapper.Map<ItemDTO>(item);
                    listaItemsDTO.Add(itemDTO);
                }
                return Ok(listaItemsDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        // api/item/EnviarItemsDetectadosZebra
        [HttpPost("enviarItemsDetectadosZebra")]
        public async Task<ActionResult<ItemDTO>> EnviarItemsDetectadosZebra(
           [FromBody] GrupoItemsDetectadosZebraRequest request)
        {
            try
            {
                List<string> rfids= new();

                foreach(var dato in request.ListaDatos)
                {
                    var rfid = dato.data.idHex;
                    rfids.Add(rfid);
                }

                XDocument xmlRfids = new(
                   new XElement("Items", rfids.Select(rfid => new XElement("Item", new XElement("Rfid", rfid))))
                );
                SqlXml xml = new(xmlRfids.CreateReader());

                var result = await _context.Items
                    .FromSqlRaw($"EXEC sp_item @i_accion='EI', @i_grupoItemsDetectados='{xmlRfids}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_41);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_42);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_48);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_49);
                }

                var resultPaginado = Paginacion<Item>.Paginar(result, 1, 100000);
                List<ItemDTO> listaItemsDTO = new();
                foreach (var item in resultPaginado)
                {
                    var itemDTO = mapper.Map<ItemDTO>(item);
                    listaItemsDTO.Add(itemDTO);
                }
                return Ok(listaItemsDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}