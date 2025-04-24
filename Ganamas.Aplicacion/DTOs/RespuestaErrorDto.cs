namespace Ganamas.Aplicacion.DTOs;

public class RespuestaErrorDto
{
    /// <summary>
    /// Código de error para identificación
    /// </summary>
    public string? Codigo { get; set; }
    
    /// <summary>
    /// Mensaje descriptivo del error
    /// </summary>
    public string? Mensaje { get; set; }
    
    /// <summary>
    /// Detalles adicionales del error (opcional)
    /// </summary>
    public Dictionary<string, string[]>? Detalles { get; set; }
}

public class RespuestaConsultaUSerDto : RespuestaConsultaDto
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; }
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string IdZona { get; set; }
    public string NombreZona { get; set; }
}
public class RespuestaConsultaDto
{
    public bool Exitoso { get; set; }
    public string? Mensaje { get; set; }
}