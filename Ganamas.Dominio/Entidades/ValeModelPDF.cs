namespace Ganamas.Dominio.Entidades;

public class ValeModelPDF
{
    public string? IdValeDescuento { get; set; }
    public string? NroValeDescuento { get; set; }                  // NroValeDescuento
    public string? NroDocumentoAgricultor { get; set; }                     // NroDocumentoAgricultor
    public string? NombreAgricultor { get; set; }
    public string? Telefono { get; set; }
    public string? Codigo { get; set; }                  // Codigo
    public string? Cultivo { get; set; }
    public string? Area { get; set; }
    public double? LogX { get; set; }
    public double? LatY { get; set; }
    public int TipoLecturaCoordenadas { get; set; }
    public decimal DescuentoTotalRegistrado { get; set; }
    public decimal DescuentoTotalAplicado { get; set; }
    public string? Rtc { get; set; }               // Rtc
    public string? RtcNombre { get; set; }
    public DateTime Fecha { get; set; }
    public DateTime FechaVigencia { get; set; }
    public string? CanjearEn { get; set; }
    public int NroItems { get; set; }
    public string? Estado { get; set; }
    public string? Observaciones { get; set; }
    public string? UsuarioCreacion { get; set; }
    public DateTime FechaHoraCreacion { get; set; }
    public decimal? DescuentoReal { get; set; }
    public decimal? DescuentoTotalReal { get; set; }
    public bool FlagFinalizado { get; set; }
    public string? NroBoleta { get; set; }
    public decimal? ImporteBoleta { get; set; }
    public decimal? ImporteSeleccionadoBoleta { get; set; }
    public string? Cultivo2 { get; set; }
    public string? Area2 { get; set; }
    public string? TipoRegistro { get; set; }
    public string? TratamientoDatos { get; set; }

    // Agrega esta propiedad para incluir la lista de productos (detalle)
    public List<ProductoValeModelPDF> Productos { get; set; }
}

public class ProductoValeModelPDF
{
    public string? ID_Item { get; set; }           // ID_Item
    public string? Nombre { get; set; }           // Nombre
    public string? Unidad { get; set; }           // Unidad
    public decimal CantidadRegistrada { get; set; }
    public decimal CantidadAplicada { get; set; }
    public decimal ValorDescuentoUnitario { get; set; }
}