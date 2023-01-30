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
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IDAAI_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/prestamo")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PrestamoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public PrestamoController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // api/prestamo/listarPrestamosPorEstudiante
        [HttpGet("listarPrestamosPorEstudiante")]
        public async Task<ActionResult<List<PrestamoEstudianteDTO>>> ListarPrestamosPorEstudiante(
            [FromQuery] PrestamoEstudianteQuery query)
        {
            try
            {
                var result = await _context.PrestamosPorEstudiantes
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='CE', @i_usuario='{query.Usuario}', @i_matricula='{query.Matricula}'").ToListAsync();
                
                var resultPaginado = Paginacion<PrestamoEstudiante>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<PrestamoEstudianteDTO> listaPrestamoEstudianteDTO = new();
                foreach (var prestamoEstudiante in resultPaginado)
                {
                    var prestamoEstudianteDTO = mapper.Map<PrestamoEstudianteDTO>(prestamoEstudiante);
                    listaPrestamoEstudianteDTO.Add(prestamoEstudianteDTO);
                }
                return Ok(listaPrestamoEstudianteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/listarPrestamosPorModulo
        [HttpGet("listarPrestamosPorModulo")]
        public async Task<ActionResult<List<PrestamoModuloDTO>>> ListarPrestamosPorModulo(
            [FromQuery] PrestamoModuloQuery query)
        {
            try
            {
                var result = await _context.PrestamosPorModulos
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='CM', @i_usuario='{query.Usuario}', @i_modulo='{query.Modulo}'").ToListAsync();

                var resultPaginado = Paginacion<PrestamoModulo>.Paginar(result, query.Pagina, query.RecordsPorPagina);
                List<PrestamoModuloDTO> listaPrestamoModuloDTO = new();
                foreach (var prestamoModulo in resultPaginado)
                {
                    var prestamoModuloDTO = mapper.Map<PrestamoModuloDTO>(prestamoModulo);
                    listaPrestamoModuloDTO.Add(prestamoModuloDTO);
                }
                return Ok(listaPrestamoModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/registrarPrestamoPorEstudiante
        [HttpPost("registrarPrestamoPorEstudiante")]
        public async Task<ActionResult<PrestamoEstudianteDTO>> RegistrarPrestamoPorEstudiante(
            [FromBody] RegistrarPrestamoEstudianteRequest request)
        {
            try
            {
                var result = await _context.PrestamosPorEstudiantes
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='IN', @i_usuario='{request.Usuario}', @i_modoClases='{0}', @i_rfid='{request.Rfid}', @i_matricula='{request.Matricula}'").ToListAsync();
                if(result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_11);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_26);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_29);
                }
                if (result[0].Id == -10)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -11)
                {
                    return BadRequest(Mensajes.ERROR_VAL_52);
                }


                var prestamoItemEstudianteDTO = mapper.Map<PrestamoEstudianteDTO>(result[0]);
                return Ok(prestamoItemEstudianteDTO);               
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/registrarPrestamoPorModulo
        [HttpPost("registrarPrestamoPorModulo")]
        public async Task<ActionResult<PrestamoModuloDTO>> RegistrarPrestamoPorModulo(
            [FromBody] RegistrarPrestamoModuloRequest request)
        {
            try
            {
                var result = await _context.PrestamosPorModulos
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='IN', @i_usuario='{request.Usuario}', @i_modoClases='{1}', @i_grupoItems='{0}', @i_rfid='{request.Rfid}', @i_modulo='{request.Modulo}'").ToListAsync();
                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -4)
                {
                    return BadRequest(Mensajes.ERROR_VAL_26);
                }
                if (result[0].Id == -5)
                {
                    return BadRequest(Mensajes.ERROR_VAL_29);
                }
                if (result[0].Id == -9)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -12)
                {
                    return BadRequest(Mensajes.ERROR_VAL_52);
                }


                var prestamoItemModuloDTO = mapper.Map<PrestamoModuloDTO>(result[0]);
                return Ok(prestamoItemModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/registrarGrupoPrestamoPorModulo
        [HttpPost("registrarGrupoPrestamoPorModulo")]
        public async Task<ActionResult<PrestamoModuloDTO>> RegistrarGrupoPrestamoPorModulo(
            [FromBody] RegistrarGrupoPrestamoModuloRequest request)
        {
            try
            {
                XDocument xmlRfids = new(
                    new XElement("Items", request.Rfids.Select(rfid => new XElement("Item", new XElement("Rfid", rfid))))
                );
                SqlXml xml = new(xmlRfids.CreateReader());

                var result = await _context.PrestamosPorModulos
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='IN', @i_usuario='{request.Usuario}', @i_modoClases='{1}', @i_grupoItems='{1}', @i_xmlItems='{xmlRfids}', @i_modulo='{request.Modulo}'").ToListAsync();
                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_13);
                }
                if (result[0].Id == -6)
                {
                    return BadRequest(Mensajes.ERROR_VAL_30);
                }
                if (result[0].Id == -7)
                {
                    return BadRequest(Mensajes.ERROR_VAL_31);
                }
                if (result[0].Id == -8)
                {
                    return BadRequest(Mensajes.ERROR_VAL_32);
                }
                if (result[0].Id == -9)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }
                if (result[0].Id == -13)
                {
                    return BadRequest(Mensajes.ERROR_VAL_52);
                }


                var resultPaginado = Paginacion<PrestamoModulo>.Paginar(result, request.Pagina, request.RecordsPorPagina);
                List<PrestamoModuloDTO> listaPrestamoModuloCreadosDTO = new();
                foreach (var prestamoModulo in resultPaginado)
                {
                    var prestamoModuloDTO = mapper.Map<PrestamoModuloDTO>(prestamoModulo);
                    listaPrestamoModuloCreadosDTO.Add(prestamoModuloDTO);
                }
                return Ok(listaPrestamoModuloCreadosDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/devolverPrestamoPorEstudiante
        [HttpPut("devolverPrestamoPorEstudiante")]
        public async Task<ActionResult<PrestamoEstudianteDTO>> DevolverPrestamoPorEstudiante(
            [FromBody] DevolverPrestamoRequest request)
        {
            try
            {

                var result = await _context.PrestamosPorEstudiantes
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='UE', @i_usuario='{request.Usuario}', @i_id='{request.Id}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_33);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_34);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_35);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var prestamoItemEstudianteDTO = mapper.Map<PrestamoEstudianteDTO>(result[0]);
                return Ok(prestamoItemEstudianteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/devolverPrestamoPorModulo
        [HttpPut("devolverPrestamoPorModulo")]
        public async Task<ActionResult<PrestamoEstudianteDTO>> DevolverPrestamoPorModulo(
            [FromBody] DevolverPrestamoRequest request)
        {
            try
            {

                var result = await _context.PrestamosPorModulos
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='UM', @i_usuario='{request.Usuario}', @i_id='{request.Id}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_33);
                }
                if (result[0].Id == -1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_34);
                }
                if (result[0].Id == -2)
                {
                    return BadRequest(Mensajes.ERROR_VAL_36);
                }
                if (result[0].Id == -3)
                {
                    return BadRequest(Mensajes.ERROR_VAL_19);
                }

                var prestamoItemModuloDTO = mapper.Map<PrestamoModuloDTO>(result[0]);
                return Ok(prestamoItemModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/eliminarPrestamoPorEstudiante
        [HttpDelete("eliminarPrestamoPorEstudiante")]
        public async Task<ActionResult<PrestamoEstudianteDTO>> EliminarPrestamoPorEstudiante(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.PrestamosPorEstudiantes
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='DE', @i_id='{request.Id}'").ToListAsync();

                if (result.Count <1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_37);
                }
                var prestamoItemEstudianteDTO = mapper.Map<PrestamoEstudianteDTO>(result[0]);
                return Ok(prestamoItemEstudianteDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // api/prestamo/eliminarPrestamoPorModulo
        [HttpDelete("eliminarPrestamoPorModulo")]
        public async Task<ActionResult<PrestamoModuloDTO>> EliminarPrestamoPorModulo(
            [FromBody] EliminarModuloRequest request)
        {
            try
            {
                var result = await _context.PrestamosPorModulos
                    .FromSqlRaw($"EXEC sp_prestamoitem @i_accion='DM', @i_id='{request.Id}'").ToListAsync();

                if (result.Count < 1)
                {
                    return BadRequest(Mensajes.ERROR_VAL_08);
                }
                if (result[0].Id == 0)
                {
                    return BadRequest(Mensajes.ERROR_VAL_37);
                }
                var prestamoItemModuloDTO = mapper.Map<PrestamoModuloDTO>(result[0]);
                return Ok(prestamoItemModuloDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}