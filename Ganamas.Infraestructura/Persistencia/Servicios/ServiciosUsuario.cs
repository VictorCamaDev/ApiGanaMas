using System.Data;
using Azure.Core;
using Dapper;
using Ganamas.Aplicacion.Contratos;
using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Contratos.Repositorios;
using Ganamas.Dominio.Entidades;
using Ganamas.Dominio.Excepciones;
using Ganamas.Infraestructura.Persistencia.AccesoDatos;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;

namespace Ganamas.Infraestructura.Repositorios;

public class ServiciosUsuario : IServicioUsuario
{
    private readonly IAccesoDatos _accesoDatos;

    public ServiciosUsuario(IAccesoDatos accesoDatos)
    {
        _accesoDatos = accesoDatos;
    }

    public async Task<IEnumerable<Expositor>> ObtenerUsuarioRTC(string docUsuario)
    {
        var datosUsuario = await _accesoDatos.ConsultarAsync<Expositor>(
            "ObtenerAgendaPorDNI",
            new { DNI = docUsuario }
        );
        return datosUsuario;
    }
    public async Task<IEnumerable<ValidacionRTC>> ValidarDocumentoRTC(string rtcDni)
    {
        var datosUsuario = await _accesoDatos.ConsultarAsync<ValidacionRTC>(
            "ObtenerAgendaPorDNI",
            new { DNI = rtcDni }
        );
        return datosUsuario;
    }
    public async Task<IEnumerable<ZonasRTC>> ObtenerZonasRTC()
    {
        var datosZonas = await _accesoDatos.ConsultarAsync<ZonasRTC>(
            "GSNET_ZONAS_OBTENER"
        );
        return datosZonas;
    }
    public async Task<IEnumerable<TecnicoRTC>> ObtenerTecnicos()
    {
        var datosZonas = await _accesoDatos.ConsultarAsync<TecnicoRTC>(
            "ObtenerAgendaTecnico"
        );
        return datosZonas;
    }
    public async Task<IEnumerable<Cultivos>> ObtenerCultivos()
    {
        var datosCultivos = await _accesoDatos.ConsultarAsync<Cultivos>(
            "ObtenerCultivos"
        );
        return datosCultivos;
    }
    public async Task<IEnumerable<ProductosRTC>> ObtenerProductosRTC(ProductoDTO producto)
    {
        var datosUsuario = await _accesoDatos.ConsultarAsync<ProductosRTC>(
        "GSNET_LISTAR_VALES_DSCTO_KARDEX_TIENDA_VG",
            new { formato = "0", kardex = "0", idZona = producto.idZona, nombreZona = producto.nombreZona }
        );
        return datosUsuario;
    }
    public async Task<ValeResponseDTO> GuardarVale(ValeSaveDTO request)
    {
        var productosDataTable = ConvertirADataTable(request.Productos);

        var parametros = new DynamicParameters();
        parametros.Add("@NroValeDescuento", request.Vale.Numero);
        parametros.Add("@Cliente", "");
        parametros.Add("@NroDocumentoAgricultor", request.Vale.Dni);
        parametros.Add("@NombreAgricultor", request.Vale.NombreAgricultor);
        parametros.Add("@Telefono", request.Vale.Celular);
        parametros.Add("@Codigo", "001");
        parametros.Add("@Cultivo", request.Vale.Cultivo);
        parametros.Add("@Area", request.Vale.Area);
        parametros.Add("@LogX", request.Vale.LongitudX);
        parametros.Add("@LatY", request.Vale.LatitudY);
        parametros.Add("@TipoLecturaCoordenadas", request.Vale.TipoLecturaCoordenadas);
        parametros.Add("@DescuentoTotalRegistrado", request.DescuentoTotal);
        parametros.Add("@Rtc", request.Vale.CodigoRTC);
        parametros.Add("@RtcNombre", request.Vale.NombreRTC);
        parametros.Add("@Fecha", request.Vale.FechaEmision);
        parametros.Add("@FechaVigencia", request.Vale.FechaVigencia);
        parametros.Add("@ID_Zona", request.Vale.IdLugarCanje);
        parametros.Add("@CanjearEn", request.Vale.LugarCanje ?? "");
        parametros.Add("@TiendasPreferencia", "");
        parametros.Add("@Observaciones", request.Vale.Observaciones ?? "");
        parametros.Add("@NroItems", request.TotalItems);
        parametros.Add("@UsuarioModificacion", 1);
        parametros.Add("@Accion", "Insertar");
        parametros.Add("@EnTienda", true);
        parametros.Add("@UsuarioCreacion", 1);
        parametros.Add("@FechaHoraCreacion", DateTime.Now);
        parametros.Add("@Cultivo2", request.Vale.CultivoSecundario);
        parametros.Add("@Area2", request.Vale.AreaSecundaria);
        parametros.Add("@TipoRegistro", request.Vale.TipoRegistro);
        parametros.Add("@TratamientoDatos", request.Vale.TratamientoDatos);

        // TVP
        parametros.Add("@Productos", productosDataTable.AsTableValuedParameter("ValeDetalleType"));

        var resultado = await _accesoDatos.ConsultarAsync2<ValeResponseDTO>("sp_GuardarValeDescuento", parametros);

        return resultado;
    }

    private DataTable ConvertirADataTable(List<ProductoSaveDTO> productos)
    {
        var table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Nombre", typeof(string));
        table.Columns.Add("Codigo", typeof(string));
        table.Columns.Add("CantidadRegistrada", typeof(decimal));
        table.Columns.Add("CantidadAplicada", typeof(decimal));
        table.Columns.Add("ValorDescuentoUnitario", typeof(decimal));

        foreach (var p in productos)
        {
            Console.WriteLine($"ID: {p.ID}, Nombre: {p.Nombre}, Código: {p.Codigo}, CantidadRegistrada: {p.CantidadRegistrada}, CantidadAplicada: {p.CantidadAplicada}, ValorDescuentoUnitario: {p.ValorDescuentoUnitario}");
            table.Rows.Add(p.ID, p.Nombre, p.Codigo, p.CantidadRegistrada, p.CantidadAplicada, p.ValorDescuentoUnitario);
        }


        return table;
    }

    public async Task<ValeModelPDF> ObtenerValePorNumero(string numeroVale, string idzona)
    {
        try
        {
            // Primero obtenemos la cabecera
            var cabecera = await _accesoDatos.ConsultarPrimeroAsync<ValeModelPDF>(
                "sp_ObtenerValePorNumero",
                new { NroValeDescuento = numeroVale, IdZona = idzona }
            ); 

            if (cabecera == null)
            {
                throw new ExcepcionRecursoNoEncontrado("No se encontró la cabecera para el vale", null);
            }

            var productos = await _accesoDatos.ConsultarAsync<ProductoValeModelPDF>(
                "sp_ObtenerValeProductoPorNumero",
                new { NroValeDescuento = cabecera.IdValeDescuento }
            );

            cabecera.Productos = productos.ToList();

            return cabecera;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el vale {numeroVale}: {ex.Message}", ex);
        }
    }

}
