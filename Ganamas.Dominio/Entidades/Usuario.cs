namespace Ganamas.Dominio.Entidades;

public class Usuario
{
    /// <summary>
    /// Identificador único del usuario
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Nombre de usuario único
    /// </summary>
    public required string NombreUsuario { get; set; }
    
    /// <summary>
    /// Correo electrónico del usuario
    /// </summary>
    public required string Correo { get; set; }
    
    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public required string NombreCompleto { get; set; }
    
    /// <summary>
    /// Hash de la contraseña
    /// </summary>
    public string? HashContrasena { get; set; }
    
    /// <summary>
    /// Salt para la contraseña
    /// </summary>
    public string? SaltContrasena { get; set; }
    
    /// <summary>
    /// Token de refresco para renovar el token JWT
    /// </summary>
    public string? TokenRefresco { get; set; }
    
    /// <summary>
    /// Fecha de expiración del token de refresco
    /// </summary>
    public DateTime? ExpiracionTokenRefresco { get; set; }
    
    /// <summary>
    /// Fecha de registro del usuario
    /// </summary>
    public DateTime FechaRegistro { get; set; }
}
public class Expositor
{
    public string Id { get; set; }
    public string Opcion { get; set; }
    public string ID_Zona { get; set; }
    public string Nombre { get; set; }
}

