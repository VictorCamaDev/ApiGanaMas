namespace Ganamas.Aplicacion.DTOs;

public class ValeSaveDTO
{
    public ValeDTO Vale { get; set; }
    public List<ProductoSaveDTO> Productos { get; set; }
    public int TotalItems { get; set; }
    public decimal DescuentoTotal { get; set; }
}
