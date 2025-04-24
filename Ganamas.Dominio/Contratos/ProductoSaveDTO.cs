namespace Ganamas.Aplicacion.DTOs;

public class ProductoSaveDTO
{
    public int ID { get; set; }
    public string Nombre { get; set; }
    public string Codigo { get; set; }
    public decimal CantidadRegistrada { get; set; }
    public decimal CantidadAplicada { get; set; }
    public decimal ValorDescuentoUnitario { get; set; }
}
