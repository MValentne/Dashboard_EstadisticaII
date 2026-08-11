using DashboardEstadisticaII.Models;
using DashboardEstadisticaII.Services;

namespace DashboardEstadisticaII.Tests;

public class EstadisticaServiceTests
{
    [Fact]
    public void TablaDeContingencia_Debe_Sumar_UnidadesVendidas_Y_No_FilasDelArchivo()
    {
        var servicio = new EstadisticaService();
        var ventas = new List<Venta>
        {
            new() { Zona = "Centro", ModoUso = "Traslado", Cantidad = 12 },
            new() { Zona = "Centro", ModoUso = "Traslado", Cantidad = 8 },
            new() { Zona = "Norte", ModoUso = "Recreativo", Cantidad = 5 }
        };

        var tabla = servicio.CalcularTablaContingencia(ventas);

        Assert.Equal(25, tabla.Total);
        Assert.Equal(20, tabla.Frecuencias[tabla.Filas.IndexOf("Centro"), tabla.Columnas.IndexOf("Traslado")]);
        Assert.Equal(5, tabla.MarginalesFila[tabla.Filas.IndexOf("Norte")]);
    }

    [Fact]
    public void Intervalos_De_Regresion_Con_Datos_Degenerados_Deben_Ser_Finitos()
    {
        var servicio = new EstadisticaService();
        var ventas = new List<Venta>
        {
            new() { Precio = 1000000m, Cantidad = 20 },
            new() { Precio = 1000000m, Cantidad = 22 }
        };

        var regresion = servicio.CalcularRegresion(ventas);
        var intervalo = servicio.IntervaloConfianzaMedia(regresion, 1000000, 95);

        Assert.True(double.IsFinite(regresion.B0));
        Assert.True(double.IsFinite(regresion.B1));
        Assert.True(double.IsFinite(regresion.PValorT));
        Assert.True(double.IsFinite(intervalo.limInf));
        Assert.True(double.IsFinite(intervalo.limSup));
        Assert.Equal(1.0, regresion.PValorT, 10);
    }

    [Fact]
    public void PValores_Deben_Estar_En_Rango_Valido_Y_Finitos()
    {
        var servicio = new EstadisticaService();
        var ventas = Enumerable.Range(1, 12)
            .Select(i => new Venta
            {
                Zona = i % 2 == 0 ? "Centro" : "Norte",
                ModoUso = i % 3 == 0 ? "Recreativo" : "Traslado",
                Precio = 800000m + i * 25000m,
                Cantidad = 10 + i
            }).ToList();

        var tabla = servicio.CalcularTablaContingencia(ventas);
        var chi = servicio.CalcularChiCuadrado(tabla);
        var regresion = servicio.CalcularRegresion(ventas);
        var icMedia = servicio.IntervaloConfianzaMedia(regresion, 1000000, 95);
        var iPred = servicio.IntervaloPrediccion(regresion, 1000000, 95);

        Assert.InRange(chi.PValor, 0.0, 1.0);
        Assert.InRange(regresion.PValorT, 0.0, 1.0);
        Assert.True(double.IsFinite(icMedia.limInf) && double.IsFinite(icMedia.limSup));
        Assert.True(double.IsFinite(iPred.limInf) && double.IsFinite(iPred.limSup));
        Assert.True(iPred.limSup - iPred.limInf >= icMedia.limSup - icMedia.limInf);
    }
}
