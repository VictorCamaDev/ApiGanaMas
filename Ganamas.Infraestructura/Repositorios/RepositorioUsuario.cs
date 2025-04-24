using System.Data;
using System.Globalization;
using AutoMapper;
using Ganamas.Aplicacion.Contratos;
using Ganamas.Aplicacion.DTOs;
using Ganamas.Dominio.Contratos.Repositorios;
using Ganamas.Dominio.Entidades;
using Ganamas.Dominio.Excepciones;
using Ganamas.Infraestructura.Persistencia.AccesoDatos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.Extensions.Logging;

namespace Ganamas.Infraestructura.Repositorios;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly IAccesoDatos _accesoDatos;
    private readonly IServicioUsuario _servicioUsuario;
    private readonly IMapper _mapper;
    private readonly ILogger<RepositorioUsuario> _logger;

    public RepositorioUsuario(IAccesoDatos accesoDatos, IServicioUsuario servicioUsuario, IMapper mapper, ILogger<RepositorioUsuario> logger)
    {
        _mapper = mapper;
        _accesoDatos = accesoDatos;
        _servicioUsuario = servicioUsuario;
        _logger = logger;
    }
    public async Task<IEnumerable<Expositor>> ObtenerUsuarioRTC(string docUsuario)
    {
        try
        {
            var datosUsuario = await _servicioUsuario.ObtenerUsuarioRTC(docUsuario);
            return _mapper.Map<IEnumerable<Expositor>>(datosUsuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener menú para usuario: {UsuarioId}", docUsuario);
            throw new ExcepcionRecursoNoEncontrado("Error al obtener el menú del usuario", ex);
        }
    }
    public async Task<IEnumerable<ValidacionRTC>> ValidarDocumentoRTC(string rtcDni)
    {
        try
        {
            var datosUsuario = await _servicioUsuario.ValidarDocumentoRTC(rtcDni);

            if (datosUsuario != null && datosUsuario.Any())
            {
                return new List<ValidacionRTC>
            {
                new ValidacionRTC
                {
                    isValid = true,
                    message = "Documento válido"
                }
            };
            }

            return new List<ValidacionRTC>
        {
            new ValidacionRTC
            {
                isValid = false,
                message = "El DNI ingresado no está registrado en Grupo Silvestre"
            }
        };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener menú para usuario: {UsuarioId}", rtcDni);
            throw new ExcepcionRecursoNoEncontrado("Error al obtener el menú del usuario", ex);
        }
    }

    public async Task<IEnumerable<ZonasRTC>> ObtenerZonasRTC()
    {
        try
        {
            var datosZona = await _servicioUsuario.ObtenerZonasRTC();
            return _mapper.Map<IEnumerable<ZonasRTC>>(datosZona);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener zonas RTCS");
            throw new ExcepcionRecursoNoEncontrado("Error al obtener zonas RTCS", ex);
        }
    }
    public async Task<IEnumerable<TecnicoRTC>> ObtenerTecnicos()
    {
        try
        {
            var datosTecnicos = await _servicioUsuario.ObtenerTecnicos();

            var datosTransformados = datosTecnicos.Select(t => new TecnicoRTC
            {
                Id = t.Id,
                Opcion = ToTitleCase(t.Opcion)
            });

            return datosTransformados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener información sobre los técnicos");
            throw new ExcepcionRecursoNoEncontrado("Error al obtener información sobre los técnicos", ex);
        }
    }

    private string ToTitleCase(string input)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input?.ToLower() ?? "");
    }
    public async Task<IEnumerable<ProductosRTC>> ObtenerProductosRTC(ProductoDTO producto)
    {
        try
        {
            var datosUsuario = await _servicioUsuario.ObtenerProductosRTC(producto);
            return _mapper.Map<IEnumerable<ProductosRTC>>(datosUsuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los productos");
            throw new ExcepcionRecursoNoEncontrado("Error al obtener productos", ex);
        }
    }
    public async Task<ValeResponseDTO> GuardarVale(ValeSaveDTO vale)
    {
        try
        {
            var datosVale = await _servicioUsuario.GuardarVale(vale);
            return _mapper.Map<ValeResponseDTO>(datosVale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "El número de vale ya existe, verifique el número ingresado");
            throw new ExcepcionRecursoNoEncontrado("El número de vale ya existe, verifique el número ingresado", ex);
        }
    }

    public async Task<ValeModelPDF> ObtenerValePorNumero(string numeroVale, string idzona)
    {
        try
        {
            var datosVale = await _servicioUsuario.ObtenerValePorNumero(numeroVale, idzona);
            return _mapper.Map<ValeModelPDF>(datosVale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "No hay vale generado con dicho Número");
            throw new ExcepcionRecursoNoEncontrado("No hay vale generado con dicho Número", ex);
        }
    }

    public async Task<byte[]> GenerarPDFValeAsync(ValeModelPDF vale)
    {
        using var ms = new MemoryStream();
        var doc = new Document(PageSize.A6, 20, 20, 20, 20);
        var writer = PdfWriter.GetInstance(doc, ms);
        doc.Open();

        // Fuentes
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
        var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
        var redFont = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.RED);

        // Logo + título
        var logo = Image.GetInstance("Utilidades/Images/imagen.png");
        logo.ScaleAbsolute(70f, 30f);
        logo.Alignment = Image.ALIGN_LEFT;

        var headerTable = new PdfPTable(2) { WidthPercentage = 100 };
        headerTable.SetWidths(new float[] { 1, 3 });
        headerTable.AddCell(new PdfPCell(logo) { Border = Rectangle.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE });
        headerTable.AddCell(new PdfPCell(new Phrase("VALE DE DESCUENTO ESTÁNDAR", titleFont))
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_CENTER,
            VerticalAlignment = Element.ALIGN_MIDDLE
        });
        doc.Add(headerTable);

        // Cliente
        doc.Add(new Paragraph($"Cliente: {vale.NombreAgricultor}", boldFont));

        // Línea separadora
        var separato2r = new Paragraph(new Chunk(new LineSeparator(0.5f, 100f, BaseColor.WHITE, Element.ALIGN_CENTER, -1)));
        doc.Add(separato2r);

        // DNI y SIL
        var idTable = new PdfPTable(2) { WidthPercentage = 100 };
        idTable.SetWidths(new float[] { 1, 1 });
        idTable.AddCell(CreateCell($"DNI: {vale.NroDocumentoAgricultor}", boldFont, false));
        idTable.AddCell(new PdfPCell(new Phrase($"SIL - {vale.NroValeDescuento}", redFont))
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_RIGHT
        });
        doc.Add(idTable);

        // Productos
        var prodTable = new PdfPTable(3) { WidthPercentage = 100 };
        prodTable.SetWidths(new float[] { 8, 1.7f, 1.7f });
        prodTable.AddCell(CreateCell("PRODUCTO", boldFont, true));
        prodTable.AddCell(CreateCell("CANT.", boldFont, true, Element.ALIGN_CENTER));
        prodTable.AddCell(CreateCell("DSCT.", boldFont, true, Element.ALIGN_RIGHT));

        foreach (var p in vale.Productos)
        {
            prodTable.AddCell(CreateCell(p.Nombre, normalFont, false));
            prodTable.AddCell(CreateCell(p.CantidadRegistrada.ToString("0"), normalFont, false, Element.ALIGN_CENTER));
            prodTable.AddCell(CreateCell($"S/ {p.ValorDescuentoUnitario:N2}", normalFont, false, Element.ALIGN_RIGHT));
        }
        doc.Add(prodTable);

        // Línea separadora
        var separator = new Paragraph(new Chunk(new LineSeparator(0.5f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
        doc.Add(separator);

        // Total
        var totalTable = new PdfPTable(2) { WidthPercentage = 100 };
        totalTable.SetWidths(new float[] { 3, 1 });
        totalTable.AddCell(CreateCell("DSCTO. CLIENTE MAX:", boldFont, false));
        totalTable.AddCell(CreateCell($"S/ {vale.DescuentoTotalRegistrado:N2}", boldFont, false, Element.ALIGN_RIGHT));
        doc.Add(totalTable);
        doc.Add(new Paragraph(" "));

        // Info + QR
        var infoQRTable = new PdfPTable(2) { WidthPercentage = 100 };
        infoQRTable.SetWidths(new float[] { 2, 1 });

        var infoCol = new PdfPTable(1);
        infoCol.AddCell(CreateMixedCell("VIG. DE CANJE: ", $"{vale.FechaVigencia:dd-MM-yyyy}", boldFont, normalFont));
        infoCol.AddCell(CreateMixedCell("FECHA: ", $"{vale.Fecha:dd-MM-yyyy}", boldFont, normalFont));
        infoCol.AddCell(CreateMixedCell("RTC: ", vale.RtcNombre, boldFont, normalFont));

        infoQRTable.AddCell(new PdfPCell(infoCol) { Border = Rectangle.NO_BORDER });

        var qrImage = GenerarQR("https://www.mecalux.es/");
        qrImage.ScaleAbsolute(80f, 80f);
        qrImage.Alignment = Image.ALIGN_RIGHT;
        infoQRTable.AddCell(new PdfPCell(qrImage) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });

        doc.Add(infoQRTable);

        // Cierra el documento antes de posicionar contenido directo
        doc.Close();

        // Bloque de firma al fondo
        using var finalMs = new MemoryStream();
        var reader = new PdfReader(ms.ToArray());
        var stamper = new PdfStamper(reader, finalMs);
        var cb = stamper.GetOverContent(1);

        var ct = new ColumnText(cb);
        ct.SetSimpleColumn(
            20f, // margen izquierdo
            20f, // margen inferior
            reader.GetPageSize(1).Width - 20f, // margen derecho
            80f // altura hasta donde se dibuja (ajustable)
        );

        ct.AddElement(new Paragraph($"Yo: {vale.NombreAgricultor}", normalFont));
        ct.AddElement(new Paragraph($"autorizo el uso de mi celular {vale.Telefono} para fines comerciales.", normalFont));
        ct.AddElement(new Paragraph("\nFirma: ___________________________", normalFont));
        ct.Go();

        stamper.Close();
        reader.Close();

        return finalMs.ToArray();
    }

    private Image GenerarQR(string contenido)
    {
        var qrWriter = new BarcodeQRCode(contenido, 100, 100, null);
        return qrWriter.GetImage();
    }

    private PdfPCell CreateCell(string text, Font font, bool topBorder = false, int alignment = Element.ALIGN_LEFT)
    {
        return new PdfPCell(new Phrase(text, font))
        {
            Border = topBorder ? Rectangle.TOP_BORDER : Rectangle.NO_BORDER,
            HorizontalAlignment = alignment,
            PaddingTop = 4f,
            PaddingBottom = 2f
        };
    }
    private PdfPCell CreateMixedCell(string label, string value, Font labelFont, Font valueFont, int alignment = Element.ALIGN_LEFT)
    {
        var phrase = new Phrase();
        phrase.Add(new Chunk(label, labelFont));
        phrase.Add(new Chunk(value, valueFont));

        return new PdfPCell(phrase)
        {
            Border = Rectangle.NO_BORDER,
            PaddingTop = 2f,
            PaddingBottom = 2f,
            HorizontalAlignment = alignment
        };
    }

    public async Task<IEnumerable<Cultivos>> ObtenerCultivos()
    {
        try
        {
            var datosCultivos = await _servicioUsuario.ObtenerCultivos();
            return _mapper.Map<IEnumerable<Cultivos>>(datosCultivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de cultivos");
            throw new ExcepcionRecursoNoEncontrado("Error al obtener cultivos", ex);
        }
    }
}