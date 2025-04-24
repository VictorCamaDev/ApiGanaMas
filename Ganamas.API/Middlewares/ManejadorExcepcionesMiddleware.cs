using System.Net;
using System.Text.Json;
using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Excepciones;

namespace Ganamas.API.Middlewares;

public class ManejadorExcepcionesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ManejadorExcepcionesMiddleware(
        RequestDelegate next,
        ILogger<ManejadorExcepcionesMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no manejado: {Mensaje}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private async Task ManejarExcepcionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var respuestaError = new RespuestaErrorDto();
        
        switch (exception)
        {
            case ExcepcionValidacion validacionEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                respuestaError.Codigo = "VALIDATION_ERROR";
                respuestaError.Mensaje = validacionEx.Message;
                respuestaError.Detalles = validacionEx.Errores;
                break;
                
            case ExcepcionRecursoNoEncontrado noEncontradoEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                respuestaError.Codigo = "NOT_FOUND_ERROR";
                respuestaError.Mensaje = noEncontradoEx.Message;
                break;
                
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                respuestaError.Codigo = "SERVER_ERROR";
                respuestaError.Mensaje = _environment.IsDevelopment() 
                    ? exception.Message 
                    : "Ha ocurrido un error interno. Por favor, inténtelo de nuevo más tarde.";
                break;
        }
        
        var opciones = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(respuestaError, opciones);
        
        await context.Response.WriteAsync(json);
    }
}
