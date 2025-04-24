using System.Drawing;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Ganamas.Aplicacion.Contratos;
using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Contratos.Repositorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;

namespace Ganamas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IRepositorioUsuario _repositorioUsuario;

    public UsuarioController(
        ILogger<UsuarioController> logger,
        IRepositorioUsuario repositorioUsuario)
    {
        _logger = logger;
        _repositorioUsuario = repositorioUsuario;
    }

    [HttpPost("ObtenerUsuarioRTC")]
    public async Task<IActionResult> ObtenerUsuarioRTC([FromBody]string solicitud)
    {
        var resultado = await _repositorioUsuario.ObtenerUsuarioRTC(solicitud);
        
        if (!resultado.Any())
        {
            return Unauthorized(new RespuestaErrorDto 
            { 
                Mensaje = "No se encontró información con el DNI ingresado",
                Codigo = "AUTH_ERROR_006"
            });
        }              
        return Ok(resultado);
    }

    [HttpGet("ObtenerZonasRTC")]    
    public async Task<IActionResult> ObtenerZonasRTC()
    {
        var resultado = await _repositorioUsuario.ObtenerZonasRTC();

        if (!resultado.Any())
        {
            return Unauthorized(new RespuestaErrorDto
            {
                Mensaje = "No se encontró información con el DNI ingresado",
                Codigo = "AUTH_ERROR_006"
            });
        }
        return Ok(resultado);
    }

    [HttpGet("ObtenerTecnicos")]
    public async Task<IActionResult> ObtenerTecnicos()
    {
        var resultado = await _repositorioUsuario.ObtenerTecnicos();

        if (!resultado.Any())
        {
            return Unauthorized(new RespuestaErrorDto
            {
                Mensaje = "No se pudo obtener información de los técnicos.",
                Codigo = "AUTH_ERROR_006"
            });
        }
        return Ok(resultado);
    }

    [HttpPost("ObtenerProductosRTC")]
    public async Task<IActionResult> ObtenerProductosRTC([FromBody] ProductoDTO producto)
    {
        var resultado = await _repositorioUsuario.ObtenerProductosRTC(producto);

        if (!resultado.Any())
        {
            return Unauthorized(new RespuestaErrorDto
            {
                Mensaje = "No se encontró información con el DNI ingresado",
                Codigo = "AUTH_ERROR_006"
            });
        }
        return Ok(resultado);
    }

    [HttpPost("ValidarDocumentoRTC")]
    public async Task<IActionResult> ValidarDocumentoRTC([FromBody] string rtcDni)
    {
        var resultado = await _repositorioUsuario.ValidarDocumentoRTC(rtcDni);

        if (!resultado.Any())
        {
            return Unauthorized(new RespuestaErrorDto
            {
                Mensaje = "No se encontró información con el DNI ingresado",
                Codigo = "AUTH_ERROR_006"
            });
        }
        return Ok(resultado);
    }

    [HttpPost("GuardarVale")]
    public async Task<IActionResult> GuardarVale([FromBody] ValeSaveDTO vale)
    {
        var resultado = await _repositorioUsuario.GuardarVale(vale);

        if (!resultado.ValeNumber.Any())
        {
            return Unauthorized(new RespuestaErrorDto
            {
                Mensaje = "El número de vale ya existe, verifique el número ingresado",
            });
        }
        return Ok(resultado);
    }

    [HttpGet("GenerarPDFVale/{numeroVale}/{idzona}")]
    public async Task<IActionResult> GenerarPDFVale(string numeroVale, string idzona)
    {
        try
        {
            var vale = await _repositorioUsuario.ObtenerValePorNumero(numeroVale, idzona);
            if (vale == null)
            {
                _logger.LogWarning($"Vale no encontrado: {numeroVale} - {idzona}");
                return NotFound($"Vale con número {numeroVale} y zona {idzona} no encontrado");
            }

            var pdfBytes = await _repositorioUsuario.GenerarPDFValeAsync(vale);

            return File(pdfBytes, "application/pdf", $"vale-{numeroVale}.pdf");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al generar PDF para vale {numeroVale} - {idzona}");
            return StatusCode(500, $"Error al generar el PDF: {ex.Message}");
        }
    }

    [HttpGet("ObtenerCultivos")]
    public async Task<IActionResult> ObtenerCultivos()
    {
        try
        {
            var cultivos = await _repositorioUsuario.ObtenerCultivos();

            if (cultivos == null || !cultivos.Any())
            {
                return NotFound(new RespuestaErrorDto
                {
                    Mensaje = "No se encontraron cultivos registrados.",
                    Codigo = "CULTIVO_NOT_FOUND"
                });
            }

            return Ok(cultivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cultivos");
            return StatusCode(500, new RespuestaErrorDto
            {
                Mensaje = "Ocurrió un error inesperado al obtener los cultivos.",
                Codigo = "INTERNAL_SERVER_ERROR"
            });
        }
    }
}
