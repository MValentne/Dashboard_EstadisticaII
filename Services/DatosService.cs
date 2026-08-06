namespace DashboardEstadisticaII.Services;

using DashboardEstadisticaII.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

/// <summary>
/// Servicio para cargar y almacenar los datos del dataset.
/// Registrado como Scoped (singleton en WASM) para compartir datos entre páginas.
/// </summary>
public class DatosService
{
    public List<Venta> Ventas { get; private set; } = new();
    public bool DatosCargados => Ventas.Count > 0;
    public string? ErrorMensaje { get; private set; }
    public string? NombreArchivo { get; private set; }

    /// <summary>Se dispara cuando los datos cambian (archivo nuevo cargado).</summary>
    public event Action? OnDatosChanged;

    /// <summary>
    /// Carga un archivo CSV o XLSX desde el componente InputFile.
    /// </summary>
    public async Task CargarArchivo(IBrowserFile archivo)
    {
        ErrorMensaje = null;
        NombreArchivo = archivo.Name;
        var extension = Path.GetExtension(archivo.Name).ToLowerInvariant();

        try
        {
            if (extension == ".csv")
            {
                await CargarCSV(archivo);
            }
            else if (extension is ".xlsx" or ".xls")
            {
                await CargarExcel(archivo);
            }
            else
            {
                ErrorMensaje = "Formato no soportado. Use archivos .csv o .xlsx";
                return;
            }

            if (Ventas.Count == 0 && ErrorMensaje == null)
                ErrorMensaje = "El archivo no contiene datos válidos.";

            OnDatosChanged?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorMensaje = $"Error al procesar el archivo: {ex.Message}";
            Ventas = new();
            OnDatosChanged?.Invoke();
        }
    }

    // ============================================================
    // Parseo de CSV
    // ============================================================

    private async Task CargarCSV(IBrowserFile archivo)
    {
        using var stream = archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
        using var reader = new StreamReader(stream);
        var contenido = await reader.ReadToEndAsync();

        var lineas = contenido.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);
        if (lineas.Length < 2)
        {
            ErrorMensaje = "El archivo CSV está vacío o no tiene filas de datos.";
            return;
        }

        // Detectar encabezados
        var header = ParsearLineaCSV(lineas[0]);
        var idxZona = BuscarColumna(header, "zona", "sucursal");
        var idxModo = BuscarColumna(header, "modo");
        var idxPrecio = BuscarColumna(header, "precio");
        var idxCantidad = BuscarColumna(header, "cantidad");

        if (idxZona < 0 || idxModo < 0 || idxPrecio < 0 || idxCantidad < 0)
        {
            ErrorMensaje = "No se encontraron las columnas requeridas. " +
                           "El archivo debe contener: Zona/Sucursal, Modo de uso, Precio, Cantidad.";
            return;
        }

        var ventas = new List<Venta>();
        var maxIdx = new[] { idxZona, idxModo, idxPrecio, idxCantidad }.Max();

        for (int i = 1; i < lineas.Length; i++)
        {
            var campos = ParsearLineaCSV(lineas[i]);
            if (campos.Length <= maxIdx) continue;

            var zona = campos[idxZona].Trim().Trim('"');
            var modo = campos[idxModo].Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(zona) || string.IsNullOrWhiteSpace(modo)) continue;

            ventas.Add(new Venta
            {
                Zona = zona,
                ModoUso = modo,
                Precio = ParsearDecimal(campos[idxPrecio]),
                Cantidad = ParsearEntero(campos[idxCantidad])
            });
        }

        Ventas = ventas;
    }

    // ============================================================
    // Parseo de Excel (ClosedXML)
    // ============================================================

    private async Task CargarExcel(IBrowserFile archivo)
    {
        try
        {
            using var memStream = new MemoryStream();
            await archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memStream);
            memStream.Position = 0;

            using var workbook = new ClosedXML.Excel.XLWorkbook(memStream);
            var hoja = workbook.Worksheets.First();
            var filas = hoja.RowsUsed().ToList();

            if (filas.Count < 2)
            {
                ErrorMensaje = "La hoja de cálculo está vacía.";
                return;
            }

            // Leer encabezados desde la primera fila
            var headerCells = filas[0].CellsUsed().ToList();
            var headers = headerCells.Select(c => c.GetString().ToLowerInvariant()).ToList();

            var idxZona = BuscarColumnaLista(headers, "zona", "sucursal");
            var idxModo = BuscarColumnaLista(headers, "modo");
            var idxPrecio = BuscarColumnaLista(headers, "precio");
            var idxCantidad = BuscarColumnaLista(headers, "cantidad");

            if (idxZona < 0 || idxModo < 0 || idxPrecio < 0 || idxCantidad < 0)
            {
                ErrorMensaje = "No se encontraron las columnas requeridas en la hoja de cálculo.";
                return;
            }

            var ventas = new List<Venta>();
            for (int i = 1; i < filas.Count; i++)
            {
                var fila = filas[i];
                var zona = fila.Cell(idxZona + 1).GetString().Trim();
                var modo = fila.Cell(idxModo + 1).GetString().Trim();
                if (string.IsNullOrWhiteSpace(zona) || string.IsNullOrWhiteSpace(modo)) continue;

                ventas.Add(new Venta
                {
                    Zona = zona,
                    ModoUso = modo,
                    Precio = ParsearDecimal(fila.Cell(idxPrecio + 1).GetString()),
                    Cantidad = ParsearEntero(fila.Cell(idxCantidad + 1).GetString())
                });
            }

            Ventas = ventas;
        }
        catch (Exception ex)
        {
            ErrorMensaje = $"Error al leer Excel: {ex.Message}. Intente convertir a CSV.";
        }
    }

    // ============================================================
    // Helpers de parseo
    // ============================================================

    private static string[] ParsearLineaCSV(string linea)
    {
        // Detectar delimitador: punto y coma (común en Argentina) o coma
        var delimiter = linea.Contains(';') ? ';' : ',';
        return linea.Split(delimiter);
    }

    private static int BuscarColumna(string[] header, params string[] keywords)
    {
        for (int i = 0; i < header.Length; i++)
        {
            var h = header[i].ToLowerInvariant().Trim().Trim('"');
            foreach (var kw in keywords)
                if (h.Contains(kw)) return i;
        }
        return -1;
    }

    private static int BuscarColumnaLista(List<string> headers, params string[] keywords)
    {
        for (int i = 0; i < headers.Count; i++)
            foreach (var kw in keywords)
                if (headers[i].Contains(kw)) return i;
        return -1;
    }

    private static decimal ParsearDecimal(string valor)
    {
        valor = valor.Trim().Trim('"').Replace("$", "").Replace("ARS", "").Trim();

        // Formato argentino: 1.250.000 (puntos como separador de miles)
        if (valor.Count(c => c == '.') > 1)
            valor = valor.Replace(".", "");

        // Coma como separador decimal
        valor = valor.Replace(",", ".");

        if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return 0;
    }

    private static int ParsearEntero(string valor)
    {
        valor = valor.Trim().Trim('"').Replace(".", "").Replace(",", "");
        if (int.TryParse(valor, out var result))
            return result;
        return 0;
    }
}
