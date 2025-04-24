using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Entidades;

namespace Ganamas.Aplicacion.Contratos;

public interface IServicioUsuario
{
    /// <summary>
    /// Obtiene un usuario por su documento
    /// </summary>
    Task<IEnumerable<Expositor>> ObtenerUsuarioRTC(string docUsuario);
    Task<IEnumerable<ValidacionRTC>> ValidarDocumentoRTC(string rtcDni);
    Task<IEnumerable<ProductosRTC>> ObtenerProductosRTC(ProductoDTO producto);
    Task<ValeResponseDTO> GuardarVale(ValeSaveDTO producto);
    Task<IEnumerable<ZonasRTC>> ObtenerZonasRTC();
    Task<IEnumerable<TecnicoRTC>> ObtenerTecnicos();
    Task<IEnumerable<Cultivos>> ObtenerCultivos();
    Task<ValeModelPDF> ObtenerValePorNumero(string numeroVale, string idzona);
}
