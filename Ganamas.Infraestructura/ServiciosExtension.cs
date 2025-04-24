using Ganamas.Aplicacion.Contratos;
using Ganamas.Dominio.Contratos.Repositorios;
using Ganamas.Infraestructura.Persistencia.AccesoDatos;
using Ganamas.Infraestructura.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ganamas.Infraestructura;

public static class ServiciosExtension
{
    public static IServiceCollection RegistrarServiciosInfraestructura(this IServiceCollection servicios, IConfiguration configuracion)
    {
        // Registrar acceso a datos
        servicios.AddSingleton<IAccesoDatos, AccesoDatosSql>();
        
        // Registrar repositorios
        servicios.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        
        // Registrar servicios de infraestructura
        servicios.AddScoped<IServicioUsuario, ServiciosUsuario>();
        
        return servicios;
    }
}
