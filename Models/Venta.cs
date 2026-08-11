namespace DashboardEstadisticaII.Models;

/// <summary>
/// Representa una fila del dataset de ventas.
/// Columnas: Sucursal/Zona, Modo de uso preferido, Venta total y
/// Antigüedad del vendedor. Cada instancia representa una venta individual.
/// </summary>
public class Venta
{
    public string Zona { get; set; } = "";
    public string ModoUsoPreferido { get; set; } = "";
    public decimal VentaTotal { get; set; }
    public decimal AntiguedadVendedor { get; set; }
}

// ============================================================
// Resultados de análisis cualitativo
// ============================================================

/// <summary>
/// Tabla de contingencia cruzada con ventas individuales observadas y marginales.
/// </summary>
public class TablaContingenciaResult
{
    public List<string> Filas { get; set; } = new();
    public List<string> Columnas { get; set; } = new();
    public int[,] Frecuencias { get; set; } = new int[0, 0];
    public int[] MarginalesFila { get; set; } = [];
    public int[] MarginalesColumna { get; set; } = [];
    public int Total { get; set; }
}

/// <summary>
/// Resultado completo de la prueba Chi-Cuadrado.
/// </summary>
public class ChiCuadradoResult
{
    public double Estadistico { get; set; }
    public int GradosLibertad { get; set; }
    public double ValorCritico { get; set; }
    public double PValor { get; set; }
    public double[,] FrecuenciasEsperadas { get; set; } = new double[0, 0];
    public double[,] DiferenciasRelativas { get; set; } = new double[0, 0];
    public bool CumpleSupuestos { get; set; }
    public string MensajeSupuestos { get; set; } = "";
}

// ============================================================
// Resultados de análisis cuantitativo
// ============================================================

/// <summary>
/// Resultado completo del análisis de regresión lineal simple.
/// Incluye coeficientes, estadísticos de prueba, residuos, etc.
/// </summary>
public class RegresionResult
{
    public double B0 { get; set; }              // Ordenada al origen
    public double B1 { get; set; }              // Pendiente
    public double R { get; set; }               // Correlación de Pearson
    public double R2 { get; set; }              // Coeficiente de determinación
    public double ErrorEstandar { get; set; }   // Se (error estándar de la regresión)
    public double ErrorEstandarB1 { get; set; } // Sb1
    public double ErrorEstandarB0 { get; set; } // Sb0
    public double EstadisticoT { get; set; }    // t = B1 / Sb1
    public double PValorT { get; set; }         // p-valor del test t
    public int GradosLibertad { get; set; }     // n - 2
    public int N { get; set; }                  // Tamaño de muestra
    public double MediaX { get; set; }
    public double MediaY { get; set; }
    public double SumaCuadradosX { get; set; }  // Sxx = Σ(xi - x̄)²
    public double SumaX2 { get; set; }          // Σxi²
    public List<double> Residuos { get; set; } = new();
    public List<double> ValoresAjustados { get; set; } = new();
    public List<double> ValoresX { get; set; } = new();
    public List<double> ValoresY { get; set; } = new();
}

/// <summary>Resultado de una prueba de supuestos del modelo de regresión.</summary>
public class PruebaSupuestoResult
{
    public string Nombre { get; set; } = "";
    public double Estadistico { get; set; }
    public int GradosLibertad { get; set; }
    public double PValor { get; set; }
    public bool EsAplicable { get; set; }
    public string Mensaje { get; set; } = "";
}

// ============================================================
// Clases auxiliares para gráficos
// ============================================================

/// <summary>Punto genérico para gráficos XY (scatter, línea, Q-Q, residuos).</summary>
public class PuntoXY
{
    public decimal X { get; set; }
    public decimal Y { get; set; }
}

/// <summary>Dato para gráfico de barras de la tabla de contingencia.</summary>
public class DatoContingencia
{
    public string Zona { get; set; } = "";
    public string ModoUsoPreferido { get; set; } = "";
    public int Frecuencia { get; set; }
}
