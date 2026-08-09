using DashboardEstadisticaII.Models;
using DashboardEstadisticaII.Services;

namespace DashboardEstadisticaII.Tests;

public class EstadisticaServiceTests
{
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

    [Theory]
    [InlineData(1.0)]
    [InlineData(-1.0)]
    [InlineData(0.0)]
    public void Intervalo_Correlacion_Con_Valores_Limite_No_Debe_Arrojar_NaN(double r)
    {
        var servicio = new EstadisticaService();
        var (limInf, limSup) = servicio.IntervaloConfianzaCorrelacion(r, 50, 95);

        Assert.True(double.IsFinite(limInf));
        Assert.True(double.IsFinite(limSup));
        Assert.False(double.IsNaN(limInf));
        Assert.False(double.IsNaN(limSup));
    }
}
