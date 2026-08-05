# Dashboard Estadistico Tigre II
# Descripcion del proyecto

--- 

# Introduccion
Se nos solicita hacer un dashboard dinamico estadistico con la estructura que se definira a continuacion. Somos estudiantes de informatica, el proyecto corresponde a finales del cursado de Estadistica II, nos interesa hacer una implementacion de dashboard con enfasis en procesos estadisticos definidos en la cursada. Este dashboard consiste en una web app interactiva con un archivo de entrada con un dataset; la web app ejecuta calculos estadisticos con base a este dataset, y los representa de la forma solicitada a lo largo de esta descripcion, siempre con simplicidad visual y buenos principios de UI/UX.

Si bien deseamos mantener una calidad en el software que producimos, el enfoque es la estadistica, la facil representacion e interpretacion de procesos estadisticos, NO SOBREINGENIAR LA IMPLEMENTACION, y poder tener una implementacion final que refleje lo cursado a lo largo del cuatrimestre.

# Stack tecnologico
Usamos el framework blazor, en particular blazor-wasm. Estamos familiarizados con el uso de la plataforma .NET y el lenguaje C#, buscamos usar una cantidad minima de bibliotecas pesadas, usamos solo lo minimo e indispensable para reducir el peso del proyecto.

Usamos blazor-wasm para poder hostear la web en github pages, nuestro proyecto ya posee un comando *deploy.sh* para elevar automaticamente los ultimos avances en el proyecto al gh-pages. blazor-wasm nos permite correr codigo directamente en el cliente, por lo que no nos acomplejamos con intraccion cliente-servidor.

Tambien buscamos interactividad con la entrada de un dataset cargado en un archivo CSV/EXCEL por lo que necesitamos alguna forma de adjuntar el archivo de forma intuitiva (preferiblemente una pequena interfaz virtual que sea visible, con buenos principios de UI/UX) para que toda la web app trabaje alrededor de la informacion del dataset.

Buscamos que el front end se apegue a la identidad de la empresa.

# Contexto del proyecto
Gran parte de este contexto fue parte de un proyecto a mediados de cuatrimestre bajo la misma materia (Estadistica II), en este primer proyecto nos situamos en una empresa ficticia de monopatines electricos, la consigna dictaba lo siguiente: 

> "Somos parte de una empresa de fabricación y venta de monopatines eléctricos de nombre Monopatines Voltio. Actualmente hay dos sucursales: la principal en Rosario y otra nueva en Córdoba"

Por lo que nos interesa que el front end de la web app refleje la identidad de esta empresa.

Este proyecto es la segunda parte de ese proyecto realizado a mediados de cuatrimestre, por lo que seguimos la misma linea contextual.

Somos parte del equipo informatico de una empresa ficticia llamada *Monopatines Electricos Voltio*, tenemos que desarrollar una web app en forma de dashboard que sea de utilidad para equipo gerencial y para analistas tecnicos de la empresa, por lo que habra una division en el contenido y usuario target de cada division. El dashboard esta pensado para ser actualizado semanalmente con los datos cargados en el archivo CSV/EXCEL.

Se nos solicita en consigna seleccionar variables a analizar (que estaran incluidas en el CSV/EXCEl), nosotros seleccionamos las siguientes:

**Cualitativas:**
1. Sucursal / Zona : Cordoba Capital/ Zona Limitrofe/ Ciudades Medias
2. Modo de uso (respecto al transporte con el monopatin): Transporte principal / Distancias cortas/ Entretenimiento (juguete)

**Cuantitativas:** (Lo mas obvio para nuestro contexto)
1. Precio de venta
2. Cantidad de ventas

El dashboard se divide en dos paginas:

## Pagina 1
Módulo descriptivo y Visual (Enfoque Muestral y que sea de interpretación simple).
Esta página debe estar diseñada para un perfil directivo/gerencial. El enfoque es puramente visual y descriptivo, basado en el comportamiento de los datos observados en la muestra semanal y que el gerente tenga una primera vista y análisis simple. Es la primera pagina que se muestra al abrir el dashboard (para acceso rapido para los directivos).

Dentro de esta pagina principal, deben haber dos modulos:

### Modulo cualitativo (Asociacion u Homogeneidad o Independencia segun lo que mejor se adapte al dataset)

**Visualizaciones minimas requeridas**
- Tabla de contingencia cruzada interactiva que muestre las frecuencias observadas actuales y marginales por fila o y/o columnas.
- Gráficos visuales atractivos (ej. gráficos de barras agrupadas, barras apiladas al 100% o diagramas de mosaico) que muestren a golpe de vista la distribución de una variable según las categorías de la otra.
- Indicador dinámico del estadístico de la muestra con un deslizador de variable del nivel de significancia y el p-valor. No debe tener Conclusiones , eso estara en la pagina 2.

### Modulo cuantitativo (Relacion y Regresion de la Muestra):
Selección de Variables: Definir dos variables cuantitativas de interés (ej. Gasto semanal en publicidad [“x”] y Volumen de ventas semanal [“y”]; o Horas de capacitación de operarios [“X ”] y Tasa de error operativo [“Y”]]). (Nosotros ya definimos nuestras variables arriba!!!)

**Visualizaciones e Indicadores**
- Gráfico de dispersión (Scatter Plot) interactivo que muestre la nube de puntos muestrales actuales.
- Línea de tendencia (recta de regresión muestral) trazada sobre el gráfico de dispersión.
- Tarjetas de KPI que resalten el Coeficiente de Correlación de Pearson de la muestra ($r$) y el Coeficiente de Determinación ($R^2$), con una breve interpretación dinámica de su significado según los datos cargados.

## Pagina 2
Esta página está pensada para el equipo técnico o analistas de datos de la empresa. Aquí se analiza si los comportamientos observados en la muestra de la Página 1 se pueden generalizar a toda la población del negocio bajo condiciones de rigor estadístico.

### Modulo de inferencia a la poblacion del analisis de variables cualitativas
Debe contener:
- Prueba de hipotesis de independencia u homogeneidad (dependiendo de las variables del CSV/EXCEL)
- Tabla de frecuencias esperadas y frecuencias diferenciales relativas
- Cumplimiento o no de supuestos y robustez de la prueba

### Modulo de analisis cuantitativo
Debe contener:
- Prueba de Hipótesis para la Pendiente (pendiente o coeficiente de correlacion ): El tablero debe calcular de forma dinámica la prueba de hipótesis para determinar si la variable “ x ” influye significativamente sobre “y” a nivel poblacional Debe mostrar claramente el estadístico de prueba “ t”, los grados de libertad y el p-valor resultante con una conclusión simple pero en el contexto.
- Intervalos de confianza : Mostrar los intervalos de confianza (con un deslizador de confianza entre 90 y 99%) para los parámetros poblacionales de la ordenada al origen pendiente o coeficiente de correlación.
- Predicción avanzada : Incorporar una calculadora técnica que permita al analista ingresar un valor de la variable independiente y muestre dinámicamente:

1. El intervalo de confianza para el valor esperado promedio de “Y”.
2. El intervalo de predicción para una observación individual de “Y”(siempre más amplio).
3. Validación Técnica de Supuestos Estadísticos: Para asegurar que el modelo de regresión lineal poblacional es válido y no un artefacto estadístico, esta página debe incluir gráficos técnicos de diagnóstico de residuos: Gráfico de Residuos vs. Valores Ajustados (para evaluar Linealidad y
Homocedasticidad). Gráfico de Probabilidad Normal (Q-Q Plot) de los residuos o un histograma de
residuos (para verificar el supuesto de Normalidad).

# Especificaciones del archivo CSV/EXCEL de entrada
Visualmente, el archivo CSV/EXCEL contiene una tabla con un material como este:

| Sucursal / Zona | Modo de uso | Precio de venta (ARS) | Cantidad de ventas |
|-----------------|-------------|-----------------------:|-------------------:|
| Córdoba Capital | Transporte principal | 1.250.000 | 48 |
| Zona Limítrofe | Distancias cortas | 980.000 | 35 |
| Ciudades Medias | Entretenimiento | 760.000 | 22 |
| Córdoba Capital | Distancias cortas | 1.180.000 | 41 |
| Zona Limítrofe | Transporte principal | 1.320.000 | 29 |
| Ciudades Medias | Distancias cortas | 890.000 | 31 |
| Córdoba Capital | Entretenimiento | 840.000 | 19 |
| Zona Limítrofe | Distancias cortas | 950.000 | 27 |

## Integración de carga de CSV/Excel

1. Agregar un componente `<InputFile>` en `Dashboard.razor`.
2. Permitir archivos `.csv` y `.xlsx`.
3. Crear un método `CargarArchivo`.
4. Detectar la extensión del archivo.
5. Si es `.csv`, leer el archivo y convertir cada fila en un objeto `Venta`.
6. Si es `.xlsx`, usar `ClosedXML` para leer la primera hoja y convertir cada fila en un objeto `Venta`.
7. Guardar los registros en `List<Venta>`.
8. Actualizar el estado del componente con `StateHasChanged()`.
9. Hacer que todas las tablas, gráficos y estadísticas usen `List<Venta>` como fuente de datos.
10. Validar que el archivo tenga las columnas esperadas: `Zona`, `ModoUso`, `Precio`, `Cantidad`.
11. Mostrar un mensaje de error si el formato es inválido.
12. No guardar el archivo en disco; procesarlo completamente en memoria.

# Pautas de UI/UX

* Layout estable: sidebar fija + contenido principal.
* Grid consistente (12 columnas o equivalente).
* Espaciado uniforme (16–24 px).
* Tarjetas con bordes suaves y sombra sutil.
* Paleta simple: fondo claro, grises + un color primario.
* Tipografía consistente; jerarquía clara (título, sección, contenido).
* Componentes con alturas y márgenes uniformes.
* Alineación estricta entre tarjetas, tablas y gráficos.
* Máximo 2 gráficos por fila.
* Mucho espacio en blanco; evitar saturación.
* Estados visuales claros (hover, activo, deshabilitado).
* Transiciones cortas (150–250 ms).
* Responsive para escritorio y tablet.
* Mantener el mismo estilo visual en todas las páginas.
* Priorizar claridad y consistencia sobre complejidad.

---

# y sobre todo, codigo facil de leer y mantener.
