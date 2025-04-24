namespace Ganamas.Aplicacion.DTOs;

public class PerfilUsuarioDto
{
    /// <summary>
    /// ID único del usuario
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public required string NombreUsuario { get; set; }
    
    /// <summary>
    /// Correo electrónico
    /// </summary>
    public required string Documento { get; set; }
}
