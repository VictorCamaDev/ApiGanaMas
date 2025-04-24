namespace Ganamas.Aplicacion.DTOs;

public class ValeDTO
{
    public string? Numero { get; set; }
    public string? IdExpositor { get; set; }
    public string? IdExponente { get; set; }
    public string? NombreAgricultor { get; set; }
    public string? Dni { get; set; }
    public string? Cultivo { get; set; }
    public string? Area { get; set; }
    public string? LongitudX { get; set; }
    public string? LatitudY { get; set; }
    public string? CodigoRTC { get; set; }
    public string? NombreRTC { get; set; }
    public string? LugarCanje { get; set; }
    public int TipoLecturaCoordenadas { get; set; }
    public string? IdLugarCanje { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVigencia { get; set; }
    public string? Observaciones { get; set; }
    public string Celular { get; set; }
    public string? CultivoSecundario { get; set; }
    public string? AreaSecundaria { get; set; }
    public string? CodigoEmpresa { get; set; }
    public string? TipoRegistro { get; set; }
    public bool TratamientoDatos { get; set; }
}
