using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Entidades;
namespace Ganamas.Dominio.Contratos.Repositorios;

public interface IRepositorioUsuario
{
    Task<IEnumerable<Expositor>> ObtenerUsuarioRTC(string docUsuario);
    Task<IEnumerable<ValidacionRTC>> ValidarDocumentoRTC(string rtcDni);
    Task<IEnumerable<ZonasRTC>> ObtenerZonasRTC();
    Task<IEnumerable<TecnicoRTC>> ObtenerTecnicos();
    Task<IEnumerable<ProductosRTC>> ObtenerProductosRTC(ProductoDTO producto);
    Task<ValeResponseDTO> GuardarVale(ValeSaveDTO vale);
    Task<ValeModelPDF> ObtenerValePorNumero(string numeroVale, string idzona);
    Task<byte[]> GenerarPDFValeAsync(ValeModelPDF vale);
    Task<IEnumerable<Cultivos>> ObtenerCultivos();
}
