using DashboardEstadisticaII.Models;
using DashboardEstadisticaII.Services;

namespace DashboardEstadisticaII.Tests;

public class EstadisticaServiceTests
{
    [Fact]
    public void TablaDeContingencia_Debe_Contar_Cada_Fila_Como_Una_Venta()
    {
        var servicio = new EstadisticaService();
        var ventas = new List<Venta>
        {
            new() { Zona = "Centro", ModoUsoPreferido = "Traslado", VentaTotal = 1200000m, AntiguedadVendedor = 2 },
            new() { Zona = "Centro", ModoUsoPreferido = "Traslado", VentaTotal = 980000m, AntiguedadVendedor = 4 },
            new() { Zona = "Norte", ModoUsoPreferido = "Recreativo", VentaTotal = 760000m, AntiguedadVendedor = 6 }
        };

        var tabla = servicio.CalcularTablaContingencia(ventas);

        Assert.Equal(3, tabla.Total);
        Assert.Equal(2, tabla.Frecuencias[tabla.Filas.IndexOf("Centro"), tabla.Columnas.IndexOf("Traslado")]);
        Assert.Equal(1, tabla.MarginalesFila[tabla.Filas.IndexOf("Norte")]);
    }

    [Fact]
    public void Intervalos_De_Regresion_Con_Datos_Degenerados_Deben_Ser_Finitos()
    {
        var servicio = new EstadisticaService();
        var ventas = new List<Venta>
        {
            new() { VentaTotal = 1000000m, AntiguedadVendedor = 2 },
            new() { VentaTotal = 1200000m, AntiguedadVendedor = 2 }
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
                ModoUsoPreferido = i % 3 == 0 ? "Recreativo" : "Traslado",
                VentaTotal = 800000m + i * 25000m,
                AntiguedadVendedor = 1 + i
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

    [Fact]
    public void PruebasDeSupuestos_Deben_Devolver_Resultados_Finitos()
    {
        var servicio = new EstadisticaService();
        var ventas = Enumerable.Range(1, 16).Select(i => new Venta
        {
            Zona = i % 2 == 0 ? "Centro" : "Norte",
            ModoUsoPreferido = i % 3 == 0 ? "Entretenimiento" : "Traslado",
            AntiguedadVendedor = i,
            VentaTotal = 700000m + 45000m * i + (i % 2 == 0 ? 12000m : -8000m)
        }).ToList();

        var regresion = servicio.CalcularRegresion(ventas);
        var normalidad = servicio.ProbarNormalidadResiduos(regresion);
        var homoscedasticidad = servicio.ProbarHomoscedasticidad(regresion);

        Assert.True(normalidad.EsAplicable);
        Assert.True(homoscedasticidad.EsAplicable);
        Assert.True(double.IsFinite(normalidad.Estadistico));
        Assert.InRange(normalidad.PValor, 0, 1);
        Assert.True(double.IsFinite(homoscedasticidad.Estadistico));
        Assert.InRange(homoscedasticidad.PValor, 0, 1);
    }
}
