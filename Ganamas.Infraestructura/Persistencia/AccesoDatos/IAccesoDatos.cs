using System.Data;

namespace Ganamas.Infraestructura.Persistencia.AccesoDatos;

/// <summary>
/// Interfaz para el acceso a datos mediante procedimientos almacenados
/// </summary>
public interface IAccesoDatos
{
    /// <summary>
    /// Ejecuta un procedimiento almacenado que devuelve un conjunto de resultados
    /// </summary>
    Task<IEnumerable<T>> ConsultarAsync<T>(string procedimiento, object parametros = null) where T : class;
    Task<T> ConsultarAsync2<T>(string procedimiento, object parametros = null);

    /// <summary>
    /// Ejecuta un procedimiento almacenado que devuelve un único resultado
    /// </summary>
    Task<T> ConsultarPrimeroAsync<T>(string procedimiento, object parametros = null) where T : class;
    
    /// <summary>
    /// Ejecuta un procedimiento almacenado que devuelve un valor escalar
    /// </summary>
    Task<T> ConsultarValorAsync<T>(string procedimiento, object parametros = null);
    
    /// <summary>
    /// Ejecuta un procedimiento almacenado que no devuelve resultados
    /// </summary>
    Task<int> EjecutarAsync(string procedimiento, object parametros = null);
    
    /// <summary>
    /// Ejecuta un procedimiento almacenado dentro de una transacción
    /// </summary>
    Task<int> EjecutarTransaccionAsync(string procedimiento, object parametros = null);
    
    /// <summary>
    /// Inicia una transacción
    /// </summary>
    Task<IDbTransaction> IniciarTransaccionAsync();
    /// <summary>
    /// Confirma una transacción
    /// </summary>
    void ConfirmarTransaccion(IDbTransaction transaccion);
    
    /// <summary>
    /// Revierte una transacción
    /// </summary>
    void RevertirTransaccion(IDbTransaction transaccion);
}
