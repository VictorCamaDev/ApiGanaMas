using System.Data;
using System.Data.SqlClient;
using Dapper;
using Ganamas.Infraestructura.Persistencia.AccesoDatos;
using Microsoft.Extensions.Configuration;

namespace Ganamas.Infraestructura.Persistencia.AccesoDatos;
/// <summary>
/// Implementación de acceso a datos para SQL Server usando Dapper
/// </summary>
public class AccesoDatosSql : IAccesoDatos
{
    private readonly string _cadenaConexion;

    public AccesoDatosSql(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("ConexionPredeterminada") 
            ?? throw new InvalidOperationException("La cadena de conexión 'ConexionPredeterminada' no está configurada");
    }

    /// <summary>
    /// Crea una nueva conexión a la base de datos
    /// </summary>
    private async Task<SqlConnection> CrearConexionAsync()
    {
        var conexion = new SqlConnection(_cadenaConexion);
        await conexion.OpenAsync();
        return conexion;
    }

    public async Task<IEnumerable<T>> ConsultarAsync<T>(string procedimiento, object parametros = null) where T : class
    {
        using var conexion = await CrearConexionAsync();
        return await conexion.QueryAsync<T>(
            procedimiento,
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }
    public async Task<T> ConsultarAsync2<T>(string procedimiento, object parametros)
    {
        using var conexion = await CrearConexionAsync();

        var command = new CommandDefinition(
            procedimiento,
            parametros,
            commandType: CommandType.StoredProcedure
        );

        return await conexion.QueryFirstOrDefaultAsync<T>(command);
    }

    public async Task<T> ConsultarPrimeroAsync<T>(string procedimiento, object parametros = null) where T : class
    {
        using var conexion = await CrearConexionAsync();
        return await conexion.QueryFirstOrDefaultAsync<T>(
            procedimiento,
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<T> ConsultarValorAsync<T>(string procedimiento, object parametros = null)
    {
        using var conexion = await CrearConexionAsync();
        return await conexion.ExecuteScalarAsync<T>(
            procedimiento,
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> EjecutarAsync(string procedimiento, object parametros = null)
    {
        using var conexion = await CrearConexionAsync();
        return await conexion.ExecuteAsync(
            procedimiento,
            parametros,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<int> EjecutarTransaccionAsync(string procedimiento, object parametros = null)
    {
        using var conexion = await CrearConexionAsync();
        using var transaccion = await conexion.BeginTransactionAsync();
        
        try
        {
            var resultado = await conexion.ExecuteAsync(
                procedimiento,
                parametros,
                transaction: transaccion,
                commandType: CommandType.StoredProcedure
            );
            
            await transaccion.CommitAsync();
            return resultado;
        }
        catch
        {
            await transaccion.RollbackAsync();
            throw;
        }
    }

    public async Task<IDbTransaction> IniciarTransaccionAsync()
    {
        var conexion = await CrearConexionAsync();
        return await conexion.BeginTransactionAsync();
    }

    public void ConfirmarTransaccion(IDbTransaction transaccion)
    {
        transaccion.Commit();
        transaccion.Connection?.Close();
    }

    public void RevertirTransaccion(IDbTransaction transaccion)
    {
        transaccion.Rollback();
        transaccion.Connection?.Close();
    }
}
