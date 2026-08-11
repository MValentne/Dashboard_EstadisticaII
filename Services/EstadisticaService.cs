namespace DashboardEstadisticaII.Services;

using DashboardEstadisticaII.Models;
using MathNet.Numerics.Distributions;

/// <summary>
/// Servicio con todos los cálculos estadísticos del dashboard.
/// Dividido en: análisis cualitativo (chi-cuadrado) y cuantitativo (regresión).
/// </summary>
public class EstadisticaService
{
    // ============================================================
    // ANÁLISIS CUALITATIVO — Tabla de contingencia y Chi-Cuadrado
    // ============================================================

    /// <summary>
    /// Construye la tabla de contingencia cruzada (Zona × modo de uso preferido).
    /// Cada fila del archivo representa una venta individual y aporta frecuencia 1.
    /// </summary>
    public TablaContingenciaResult CalcularTablaContingencia(List<Venta> ventas)
    {
        var filas = ventas.Select(v => v.Zona).Distinct().OrderBy(z => z).ToList();
        var columnas = ventas.Select(v => v.ModoUsoPreferido).Distinct().OrderBy(m => m).ToList();
        int nF = filas.Count, nC = columnas.Count;

        var freq = new int[nF, nC];
        foreach (var v in ventas)
        {
            int i = filas.IndexOf(v.Zona);
            int j = columnas.IndexOf(v.ModoUsoPreferido);
            if (i >= 0 && j >= 0) freq[i, j]++;
        }

        // Calcular marginales
        var margFilas = new int[nF];
        var margCols = new int[nC];
        int total = 0;

        for (int i = 0; i < nF; i++)
            for (int j = 0; j < nC; j++)
            {
                margFilas[i] += freq[i, j];
                margCols[j] += freq[i, j];
                total += freq[i, j];
            }

        return new TablaContingenciaResult
        {
            Filas = filas,
            Columnas = columnas,
            Frecuencias = freq,
            MarginalesFila = margFilas,
            MarginalesColumna = margCols,
            Total = total
        };
    }

    /// <summary>
    /// Calcula el estadístico Chi-Cuadrado, frecuencias esperadas,
    /// diferencias relativas, p-valor y validación de supuestos.
    /// </summary>
public ChiCuadradoResult CalcularChiCuadrado(TablaContingenciaResult tabla, double alfa = 0.05)
{
    if (tabla.Total <= 0)
    {
        return new ChiCuadradoResult
        {
            Estadistico = 0,
            GradosLibertad = 0,
            ValorCritico = 0,
            PValor = 1.0,
            FrecuenciasEsperadas = new double[0, 0],
            DiferenciasRelativas = new double[0, 0],
            CumpleSupuestos = false,
            MensajeSupuestos = "⚠️ No hay datos suficientes para ejecutar la prueba chi-cuadrado."
        };
    }

    int nF = tabla.Filas.Count;
    int nC = tabla.Columnas.Count;
    var esperadas = new double[nF, nC];
    var difRelativas = new double[nF, nC];
    double chi2 = 0;
    int celdasBajas = 0;

    for (int i = 0; i < nF; i++)
    {
        for (int j = 0; j < nC; j++)
        {
            // E_ij = (total_fila_i × total_col_j) / N
            esperadas[i, j] = (double)tabla.MarginalesFila[i] * tabla.MarginalesColumna[j] / tabla.Total;

            if (esperadas[i, j] > 0)
            {
                double dif = tabla.Frecuencias[i, j] - esperadas[i, j];
                chi2 += (dif * dif) / esperadas[i, j];
                difRelativas[i, j] = dif / esperadas[i, j];
            }

            if (esperadas[i, j] < 5) celdasBajas++;
        }
    }

    int gl = (nF - 1) * (nC - 1);
    double pValor = gl > 0 ? 1.0 - ChiSquared.CDF(gl, Math.Max(0, chi2)) : 1.0;
    pValor = Math.Clamp(pValor, 0.0, 1.0);
    double valorCritico = gl > 0 ? ChiSquared.InvCDF(gl, 1.0 - alfa) : 0;

    // Evaluar supuestos
    int totalCeldas = nF * nC;
    double propBajas = totalCeldas > 0 ? (double)celdasBajas / totalCeldas : 0;

    string mensaje;
    if (celdasBajas == 0)
        mensaje = "✅ Todas las frecuencias esperadas son ≥ 5. La prueba se puede interpretar con confianza.";
    else if (propBajas <= 0.20)
        mensaje = $"⚠️ {celdasBajas} de {totalCeldas} celdas ({propBajas * 100:F0}%) tienen frecuencia esperada < 5. " +
                  "La prueba puede ser razonablemente robusta, pero conviene interpretar con cautela.";
    else
        mensaje = $"❌ {celdasBajas} de {totalCeldas} celdas ({propBajas * 100:F0}%) tienen frecuencia esperada < 5. " +
                  "La prueba puede no ser confiable. Considere combinar categorías.";

    return new ChiCuadradoResult
    {
        Estadistico = chi2,
        GradosLibertad = gl,
        ValorCritico = valorCritico,
            PValor = pValor,
            FrecuenciasEsperadas = esperadas,
            DiferenciasRelativas = difRelativas,
            CumpleSupuestos = celdasBajas == 0,
            MensajeSupuestos = mensaje
        };
    }

    // ============================================================
    // ANÁLISIS CUANTITATIVO — Regresión lineal simple
    // ============================================================

    /// <summary>
    /// Calcula todos los parámetros de la regresión lineal simple.
    /// X = antigüedad del vendedor (variable independiente),
    /// Y = venta total en ARS (variable dependiente).
    /// </summary>
    public RegresionResult CalcularRegresion(List<Venta> ventas)
    {
        int n = ventas.Count;
        if (n < 3)
        {
            double mediaX0 = ventas.Count > 0 ? ventas.Average(v => (double)v.AntiguedadVendedor) : 0;
            double mediaY0 = ventas.Count > 0 ? ventas.Average(v => (double)v.VentaTotal) : 0;
            return new RegresionResult
            {
                B0 = 0,
                B1 = 0,
                R = 0,
                R2 = 0,
                ErrorEstandar = 0,
                ErrorEstandarB1 = 0,
                ErrorEstandarB0 = 0,
                EstadisticoT = 0,
                PValorT = 1.0,
                GradosLibertad = 0,
                N = n,
                MediaX = mediaX0,
                MediaY = mediaY0,
                SumaCuadradosX = 0,
                SumaX2 = 0,
                Residuos = new List<double>(),
                ValoresAjustados = new List<double>(),
                ValoresX = ventas.Select(v => (double)v.AntiguedadVendedor).ToList(),
                ValoresY = ventas.Select(v => (double)v.VentaTotal).ToList()
            };
        }

        var x = ventas.Select(v => (double)v.AntiguedadVendedor).ToArray();
        var y = ventas.Select(v => (double)v.VentaTotal).ToArray();

        double mediaX = x.Average();
        double mediaY = y.Average();

        // Sumas de cuadrados y productos cruzados
        double sxx = 0, syy = 0, sxy = 0, sumaX2 = 0;
        for (int i = 0; i < n; i++)
        {
            double dx = x[i] - mediaX;
            double dy = y[i] - mediaY;
            sxx += dx * dx;
            syy += dy * dy;
            sxy += dx * dy;
            sumaX2 += x[i] * x[i];
        }

        // Coeficientes de regresión: ŷ = b0 + b1·x
        double b1 = sxx > 0 ? sxy / sxx : 0;
        double b0 = mediaY - b1 * mediaX;

        // Correlación de Pearson y R²
        double r = (sxx > 0 && syy > 0) ? sxy / Math.Sqrt(sxx * syy) : 0;
        double r2 = r * r;

        // Residuos y error estándar
        var residuos = new List<double>(n);
        var ajustados = new List<double>(n);
        double sce = 0; // Suma de cuadrados del error

        for (int i = 0; i < n; i++)
        {
            double yhat = b0 + b1 * x[i];
            double resid = y[i] - yhat;
            residuos.Add(resid);
            ajustados.Add(yhat);
            sce += resid * resid;
        }

        int gl = n - 2;
        double se = gl > 0 ? Math.Sqrt(sce / gl) : 0;

        // Errores estándar de los coeficientes
        double seb1 = (sxx > 0 && gl > 0) ? se / Math.Sqrt(sxx) : 0;
        double seb0 = (sxx > 0 && gl > 0) ? se * Math.Sqrt(sumaX2 / (n * sxx)) : 0;

        // Prueba t para la pendiente: H0: β1 = 0
        double tStat = seb1 > 0 ? b1 / seb1 : 0;
        double pValor = gl > 0 && double.IsFinite(tStat) && seb1 > 0
            ? 2.0 * (1.0 - StudentT.CDF(0, 1, gl, Math.Abs(tStat)))
            : 1.0;
        pValor = Math.Clamp(pValor, 0.0, 1.0);

        return new RegresionResult
        {
            B0 = b0, B1 = b1,
            R = r, R2 = r2,
            ErrorEstandar = se,
            ErrorEstandarB1 = seb1,
            ErrorEstandarB0 = seb0,
            EstadisticoT = tStat,
            PValorT = pValor,
            GradosLibertad = gl,
            N = n,
            MediaX = mediaX, MediaY = mediaY,
            SumaCuadradosX = sxx,
            SumaX2 = sumaX2,
            Residuos = residuos,
            ValoresAjustados = ajustados,
            ValoresX = x.ToList(),
            ValoresY = y.ToList()
        };
    }

    // ============================================================
    // INTERVALOS DE CONFIANZA Y PREDICCIÓN
    // ============================================================

    /// <summary>
    /// Intervalo de confianza genérico: estimador ± t_crítico × error estándar.
    /// </summary>
    public (double limInf, double limSup) IntervaloConfianza(
        double estimador, double errorEstandar, int gl, double nivelConfianza)
    {
        if (gl <= 0 || errorEstandar <= 0 || !double.IsFinite(errorEstandar))
            return (estimador, estimador);

        double alfa = 1.0 - nivelConfianza / 100.0;
        double tCrit = StudentT.InvCDF(0, 1, gl, 1.0 - alfa / 2.0);
        double margen = tCrit * errorEstandar;
        return (estimador - margen, estimador + margen);
    }

    /// <summary>
    /// IC para la respuesta media E[Y | X = x0].
    /// </summary>
    public (double yEst, double limInf, double limSup) IntervaloConfianzaMedia(
        RegresionResult reg, double x0, double nivelConfianza)
    {
        double yEst = reg.B0 + reg.B1 * x0;
        if (reg.GradosLibertad <= 0 || reg.ErrorEstandar <= 0 || reg.SumaCuadradosX <= 0 || !double.IsFinite(yEst))
            return (yEst, yEst, yEst);

        double alfa = 1.0 - nivelConfianza / 100.0;
        double tCrit = StudentT.InvCDF(0, 1, reg.GradosLibertad, 1.0 - alfa / 2.0);

        double factor = Math.Sqrt(
            1.0 / reg.N + Math.Pow(x0 - reg.MediaX, 2) / reg.SumaCuadradosX);
        double margen = tCrit * reg.ErrorEstandar * factor;

        return (yEst, yEst - margen, yEst + margen);
    }

    /// <summary>
    /// Intervalo de predicción para una observación individual Y | X = x0.
    /// Siempre más amplio que el IC para la media.
    /// </summary>
    public (double yEst, double limInf, double limSup) IntervaloPrediccion(
        RegresionResult reg, double x0, double nivelConfianza)
    {
        double yEst = reg.B0 + reg.B1 * x0;
        if (reg.GradosLibertad <= 0 || reg.ErrorEstandar <= 0 || reg.SumaCuadradosX <= 0 || !double.IsFinite(yEst))
            return (yEst, yEst, yEst);

        double alfa = 1.0 - nivelConfianza / 100.0;
        double tCrit = StudentT.InvCDF(0, 1, reg.GradosLibertad, 1.0 - alfa / 2.0);

        double factor = Math.Sqrt(
            1.0 + 1.0 / reg.N + Math.Pow(x0 - reg.MediaX, 2) / reg.SumaCuadradosX);
        double margen = tCrit * reg.ErrorEstandar * factor;

        return (yEst, yEst - margen, yEst + margen);
    }

    /// <summary>
    /// IC para el coeficiente de correlación poblacional usando la transformación z de Fisher.
    /// </summary>
    public (double limInf, double limSup) IntervaloConfianzaCorrelacion(
        double r, int n, double nivelConfianza)
    {
        if (n <= 3) return (r, r);
        double rClamped = Math.Clamp(r, -0.99999999, 0.99999999);
        double z = Math.Atanh(rClamped);
        double se = 1.0 / Math.Sqrt(n - 3);
        double alfa = 1.0 - nivelConfianza / 100.0;
        double zCrit = Normal.InvCDF(0, 1, 1.0 - alfa / 2.0);

        double zLow = z - zCrit * se;
        double zHigh = z + zCrit * se;

        return (Math.Tanh(zLow), Math.Tanh(zHigh));
    }

    // ============================================================
    // DIAGNÓSTICO DE RESIDUOS
    // ============================================================

    /// <summary>
    /// Genera los puntos para el gráfico Q-Q Plot de normalidad de los residuos.
    /// Eje X: cuantiles teóricos N(0,1). Eje Y: residuos ordenados.
    /// </summary>
    public List<PuntoXY> CalcularQQPlot(List<double> residuos)
    {
        var sorted = residuos.OrderBy(r => r).ToList();
        int n = sorted.Count;
        var puntos = new List<PuntoXY>(n);

        for (int i = 0; i < n; i++)
        {
            double p = (i + 0.5) / n;
            double zTeorico = Normal.InvCDF(0, 1, p);
            puntos.Add(new PuntoXY
            {
                X = (decimal)Math.Round(zTeorico, 4),
                Y = (decimal)Math.Round(sorted[i], 2)
            });
        }

        return puntos;
    }

    /// <summary>
    /// Prueba Jarque-Bera para la normalidad de los residuos. H0: los residuos
    /// siguen una distribución normal. Se usa una aproximación chi-cuadrado con 2 GL.
    /// </summary>
    public PruebaSupuestoResult ProbarNormalidadResiduos(RegresionResult reg, double alfa = 0.05)
    {
        var residuos = reg.Residuos;
        int n = residuos.Count;
        if (n < 8)
            return new PruebaSupuestoResult { Nombre = "Jarque-Bera", EsAplicable = false, Mensaje = "Se requieren al menos 8 residuos para evaluar normalidad." };

        double media = residuos.Average();
        double m2 = residuos.Average(r => Math.Pow(r - media, 2));
        if (m2 <= 0 || !double.IsFinite(m2))
            return new PruebaSupuestoResult { Nombre = "Jarque-Bera", EsAplicable = false, Mensaje = "Los residuos no presentan variabilidad suficiente para evaluar normalidad." };

        double m3 = residuos.Average(r => Math.Pow(r - media, 3));
        double m4 = residuos.Average(r => Math.Pow(r - media, 4));
        double asimetria = m3 / Math.Pow(m2, 1.5);
        double curtosis = m4 / (m2 * m2);
        double jb = n / 6.0 * (asimetria * asimetria + Math.Pow(curtosis - 3, 2) / 4.0);
        double p = Math.Clamp(1 - ChiSquared.CDF(2, jb), 0, 1);
        return new PruebaSupuestoResult
        {
            Nombre = "Jarque-Bera", Estadistico = jb, GradosLibertad = 2, PValor = p, EsAplicable = true,
            Mensaje = p < alfa ? $"Se rechaza H₀ (p = {p:0.0000}): hay evidencia de no normalidad en los residuos." : $"No se rechaza H₀ (p = {p:0.0000}): no hay evidencia de apartamiento de la normalidad."
        };
    }

    /// <summary>
    /// Prueba Breusch-Pagan para homocedasticidad. H0: la varianza de los residuos
    /// es constante respecto de la antigüedad del vendedor.
    /// </summary>
    public PruebaSupuestoResult ProbarHomoscedasticidad(RegresionResult reg, double alfa = 0.05)
    {
        int n = reg.Residuos.Count;
        if (n < 4 || reg.ValoresX.Count != n)
            return new PruebaSupuestoResult { Nombre = "Breusch-Pagan", EsAplicable = false, Mensaje = "No hay datos suficientes para evaluar igualdad de varianzas." };

        var x = reg.ValoresX;
        var z = reg.Residuos.Select(r => r * r).ToArray();
        double mx = x.Average(), mz = z.Average();
        double sxx = x.Sum(v => Math.Pow(v - mx, 2));
        double szz = z.Sum(v => Math.Pow(v - mz, 2));
        double sxz = x.Zip(z, (vx, vz) => (vx - mx) * (vz - mz)).Sum();
        if (sxx <= 0 || szz <= 0)
            return new PruebaSupuestoResult { Nombre = "Breusch-Pagan", EsAplicable = false, Mensaje = "No hay variabilidad suficiente para evaluar igualdad de varianzas." };

        double r2Aux = Math.Clamp((sxz * sxz) / (sxx * szz), 0, 1);
        double bp = n * r2Aux;
        double p = Math.Clamp(1 - ChiSquared.CDF(1, bp), 0, 1);
        return new PruebaSupuestoResult
        {
            Nombre = "Breusch-Pagan", Estadistico = bp, GradosLibertad = 1, PValor = p, EsAplicable = true,
            Mensaje = p < alfa ? $"Se rechaza H₀ (p = {p:0.0000}): hay evidencia de varianza no constante (heterocedasticidad)." : $"No se rechaza H₀ (p = {p:0.0000}): no hay evidencia de varianzas desiguales (homocedasticidad)."
        };
    }

    // ============================================================
    // INTERPRETACIONES DINÁMICAS
    // ============================================================

    public string InterpretarCorrelacion(double r)
    {
        double absR = Math.Abs(r);
        string direccion = r >= 0 ? "positiva" : "negativa";
        string fuerza = absR switch
        {
            >= 0.9 => "muy fuerte",
            >= 0.7 => "fuerte",
            >= 0.5 => "moderada",
            >= 0.3 => "débil",
            _ => "muy débil o nula"
        };

        return $"Correlación lineal {fuerza} y {direccion}. " +
               $"En este conjunto, la antigüedad del vendedor se relaciona con la venta total " +
               $"de forma {(absR >= 0.5 ? "visible" : "leve")}.";
    }

    public string InterpretarR2(double r2)
    {
        return $"El {r2 * 100:F1}% de la variabilidad observada en la venta total " +
               $"se asocia con la relación lineal con la antigüedad del vendedor. " +
               $"El {(1 - r2) * 100:F1}% restante puede deberse a otros factores no incluidos en este modelo.";
    }

    public string InterpretarPruebaT(RegresionResult reg, double alfa)
    {
        bool rechaza = reg.PValorT < alfa;
        string decision = rechaza
            ? $"Se rechaza H₀ al nivel α = {alfa:0.00}."
            : $"No se rechaza H₀ al nivel α = {alfa:0.00}.";

        string conclusion = rechaza
            ? "Existe evidencia estadística suficiente para sostener una relación lineal entre antigüedad y venta total."
            : "No hay evidencia suficiente para afirmar una relación lineal entre antigüedad y venta total.";

        return $"{decision} {conclusion}";
    }

    public string InterpretarChiCuadrado(ChiCuadradoResult chi2, double alfa)
    {
        bool rechaza = chi2.PValor < alfa;
        string decision = rechaza
            ? $"Se rechaza H₀ al nivel α = {alfa:0.00}."
            : $"No se rechaza H₀ al nivel α = {alfa:0.00}.";

        string conclusion = rechaza
            ? "Existe evidencia estadística suficiente para afirmar que el modo de uso preferido se asocia con la zona."
            : "No hay evidencia suficiente para afirmar que el modo de uso preferido se asocie con la zona.";

        return $"{decision} {conclusion}";
    }
}
