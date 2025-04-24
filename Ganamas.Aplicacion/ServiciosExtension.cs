using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ganamas.Aplicacion;

public static class ServiciosExtension
{
    public static IServiceCollection RegistrarServiciosAplicacion(this IServiceCollection servicios)
    {
        // Registrar servicios de aplicación
        
        // Registrar validadores
        servicios.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Registrar AutoMapper
        servicios.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return servicios;
    }
}
