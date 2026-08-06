# Contexto Autogenerado — Dashboard Estadístico Tigre II

> **Generado automáticamente por IA asistente (Antigravity — Claude Opus 4.6)**  
> **Fecha de generación:** 5 de agosto de 2026  
> **Proyecto:** Dashboard Estadístico para la materia Estadística II  
> **Repositorio:** Dashboard_EstadisticaII

---

## Índice

1. [Descripción general del proyecto](#1-descripción-general-del-proyecto)
2. [Stack tecnológico y decisiones técnicas](#2-stack-tecnológico-y-decisiones-técnicas)
3. [Cronología del proyecto (historial de commits)](#3-cronología-del-proyecto)
4. [Estado actual del código](#4-estado-actual-del-código)
5. [Estructura de archivos](#5-estructura-de-archivos)
6. [Prompts y acciones realizadas con IA](#6-prompts-y-acciones-realizadas-con-ia)
7. [Documento de referencia principal](#7-documento-de-referencia-principal)
8. [Trabajo pendiente](#8-trabajo-pendiente)

---

## 1. Descripción general del proyecto

El proyecto consiste en un **dashboard estadístico interactivo** desarrollado como trabajo final de la materia **Estadística II**. El contexto ficticio se enmarca en la empresa **Monopatines Eléctricos Voltio**, con sucursales en la provincia de Córdoba, Argentina.

El dashboard recibe un archivo **CSV/Excel** como entrada con datos de ventas y ejecuta cálculos estadísticos que se representan visualmente. Está dividido en dos páginas principales:

- **Página 1 — Módulo Descriptivo y Visual** (perfil directivo/gerencial): análisis muestral con tabla de contingencia, gráficos cualitativos, scatter plot con regresión, y KPIs de correlación.
- **Página 2 — Módulo de Inferencia** (perfil técnico/analistas): pruebas de hipótesis, intervalos de confianza, predicción avanzada, y diagnóstico de residuos.

Las variables analizadas son:
- **Cualitativas:** Sucursal/Zona (Córdoba Capital, Zona Limítrofe, Ciudades Medias) y Modo de uso (Transporte principal, Distancias cortas, Entretenimiento)
- **Cuantitativas:** Precio de venta (ARS) y Cantidad de ventas

> 📄 Todo el detalle de requisitos se encuentra en [`modelo-del-problema.md`](file:///home/valentinomende/Desktop/Dashboard_EstadisticaII/consigna/modelo-del-problema.md)

---

## 2. Stack tecnológico y decisiones técnicas

| Componente | Tecnología | Justificación |
|---|---|---|
| Framework | **Blazor WebAssembly** (.NET 10) | Familiaridad con C#/.NET; ejecución 100% en cliente sin servidor |
| Hosting | **GitHub Pages** | Gratuito; deploy automático con `deploy.sh` |
| Gráficos | **Blazor-ApexCharts** (v7.0.0) | Librería liviana para gráficos interactivos en Blazor |
| Cálculos estadísticos | **MathNet.Numerics** (v5.0.0) | Librería robusta para estadística y álgebra lineal en .NET |
| CSS | **Bootstrap** (incluido en wwwroot) + CSS personalizado | Framework CSS familiar, ya incluido en el template de Blazor |
| Lectura de Excel | Pendiente — se planifica usar **ClosedXML** | Para parseo de archivos `.xlsx` |

### Paquetes NuGet instalados (según `.csproj`)

```xml
<PackageReference Include="Blazor-ApexCharts" Version="7.0.0" />
<PackageReference Include="MathNet.Numerics" Version="5.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.10" />
```

### Decisiones de diseño clave

- **Sin servidor:** Blazor WASM corre enteramente en el browser, no hay backend.
- **Procesamiento en memoria:** El CSV/Excel se procesa en el cliente, no se guarda en disco.
- **Minimalismo en dependencias:** Se busca el mínimo de paquetes para reducir el tamaño del bundle.
- **Deploy automatizado:** Script `deploy.sh` publica automáticamente a GitHub Pages (rama `gh-pages`).

---

## 3. Cronología del proyecto

Historial de commits (del más antiguo al más reciente):

| Fecha | Commit | Descripción |
|---|---|---|
| 30/07/2026 14:12 | `d1234f0` | **Primer commit** — Proyecto Blazor WASM inicializado con el template por defecto |
| 30/07/2026 14:48 | `1c5ec7c` | **feature/deploy.sh y .gitignore** — Agregado script de deploy a GitHub Pages y configuración de gitignore |
| 30/07/2026 14:49 | `ec1f0e6` | **Deploy to GitHub Pages** — Primer deploy exitoso a gh-pages |
| 02/08/2026 00:35 | `b711b07` | **consigna agregada** — Agregados archivos de consigna (PDFs e imagen del modelo) |
| 03/08/2026 15:10 | `b270160` | **Modelo del problema** — Agregado `modelo-del-problema.md` con las especificaciones detalladas |
| 05/08/2026 18:50 | `15f4448` | **modelado del problema** — Actualización del modelo del problema (HEAD actual) |

### Archivos de consigna agregados:
- `consigna/descripcion general.pdf` — Descripción general de la consigna de la materia
- `consigna/descripcion puntual.pdf` — Descripción puntual/detallada del trabajo
- `consigna/Modelo del problema TIGRE II.png` — Diagrama visual del modelo del problema
- `consigna/modelo-del-problema.md` — **Documento principal** con todas las especificaciones técnicas y funcionales

---

## 4. Estado actual del código

### Resumen: El proyecto se encuentra en estado de **template inicial de Blazor WASM**, sin implementación funcional del dashboard todavía.

#### Archivos del template (sin modificar):
- **`App.razor`** — Router por defecto de Blazor con manejo de rutas y página NotFound
- **`Program.cs`** — Entry point estándar de Blazor WASM, registra `HttpClient` como servicio
- **`_Imports.razor`** — Usings globales estándar de Blazor
- **`Pages/Home.razor`** — Página de inicio por defecto ("Hello, world!")
- **`Pages/Counter.razor`** — Ejemplo de contador del template de Blazor
- **`Pages/Weather.razor`** — Ejemplo de tabla con datos de clima (fetch de JSON)
- **`Pages/NotFound.razor`** — Página 404 simple
- **`Layout/MainLayout.razor`** — Layout principal con sidebar + contenido
- **`Layout/NavMenu.razor`** — Menú de navegación lateral con links a Home, Counter, Weather
- **`wwwroot/css/app.css`** — Estilos globales del template de Blazor
- **`wwwroot/index.html`** — HTML raíz con Bootstrap y loading spinner

#### Archivos personalizados/funcionales:
- **`deploy.sh`** — Script de deploy a GitHub Pages (personalizado, funcional)
- **`.gitignore`** — Configuración de git ignore para .NET/Blazor
- **`DashboardEstadisticaII.csproj`** — Modificado para incluir `Blazor-ApexCharts` y `MathNet.Numerics`

### Lo que falta implementar (todo):
- ❌ Modelo de datos (`Venta`)
- ❌ Carga/parseo de CSV/Excel
- ❌ Página 1: Módulo cualitativo (tabla de contingencia, gráficos de barras, chi-cuadrado)
- ❌ Página 1: Módulo cuantitativo (scatter plot, regresión, KPIs r y R²)
- ❌ Página 2: Inferencia cualitativa (prueba de hipótesis, frecuencias esperadas, supuestos)
- ❌ Página 2: Inferencia cuantitativa (prueba t para pendiente, intervalos de confianza, predicción)
- ❌ Diagnóstico de residuos (gráfico residuos vs ajustados, Q-Q Plot)
- ❌ UI/UX con identidad de Monopatines Voltio
- ❌ Eliminar páginas de ejemplo (Counter, Weather)

---

## 5. Estructura de archivos

```
Dashboard_EstadisticaII/
├── App.razor                          # Router principal de Blazor
├── Program.cs                         # Entry point de la aplicación
├── _Imports.razor                     # Usings globales
├── DashboardEstadisticaII.csproj      # Archivo de proyecto (.NET 10)
├── deploy.sh                          # Script de deploy a GitHub Pages
├── .gitignore                         # Archivos ignorados por git
│
├── consigna/                          # Documentación de la consigna
│   ├── modelo-del-problema.md         # ⭐ Especificaciones principales
│   ├── descripcion general.pdf        # Consigna general de la materia
│   ├── descripcion puntual.pdf        # Consigna detallada
│   ├── Modelo del problema TIGRE II.png  # Diagrama del modelo
│   └── contexto-autogenerado.md       # 📌 Este archivo
│
├── Pages/                             # Páginas/Rutas de Blazor
│   ├── Home.razor                     # Página de inicio (template)
│   ├── Counter.razor                  # Ejemplo del template (a eliminar)
│   ├── Weather.razor                  # Ejemplo del template (a eliminar)
│   └── NotFound.razor                 # Página 404
│
├── Layout/                            # Layout y navegación
│   ├── MainLayout.razor               # Layout principal (sidebar + contenido)
│   ├── MainLayout.razor.css           # Estilos del layout
│   ├── NavMenu.razor                  # Menú de navegación lateral
│   └── NavMenu.razor.css              # Estilos del menú
│
├── wwwroot/                           # Archivos estáticos
│   ├── index.html                     # HTML raíz de la SPA
│   ├── favicon.png                    # Ícono del sitio
│   ├── icon-192.png                   # Ícono para PWA
│   ├── css/
│   │   └── app.css                    # Estilos globales
│   ├── lib/
│   │   └── bootstrap/                 # Bootstrap CSS
│   └── sample-data/
│       └── weather.json               # Datos de ejemplo (del template)
│
├── Properties/                        # Configuración de launch
├── bin/                               # Binarios compilados
├── obj/                               # Archivos intermedios de build
└── release/                           # Output del publish para deploy
```

---

## 6. Prompts y acciones realizadas con IA

### Sesión 2 — 6 de agosto de 2026

#### Prompt del usuario:
> *"quiero que continues con el trabajo donde quedo la ultima vez, puedes ver todos los avances en consigna/contexto-autogenerado.md y la consigna pura en modelo-del-problema.md, ve retroalimentando documentacion de lo que vayas haciendo en el contexto-autogenerado.md para ir progresando"*

#### Acciones realizadas:
1. **Revisión del estado del proyecto** — Se verificó el build del proyecto y se detectó que faltaba el soporte para leer archivos Excel.
2. **Integración de la arquitectura funcional** — Se registraron los servicios de datos y estadística en el arranque de Blazor WASM.
3. **Implementación del dashboard principal** — Se reemplazó la página inicial por un tablero interactivo con carga de archivo CSV/XLSX, resumen de datos, tabla de contingencia y análisis estadístico.
4. **Implementación de análisis cualitativo y cuantitativo** — Se incorporaron cálculos de chi-cuadrado, regresión lineal simple, interpretación dinámica, intervalos de confianza y gráficos de diagnóstico.
5. **Actualización de la UI** — Se simplificó la navegación y se aplicó una identidad visual más acorde al contexto de Monopatines Voltio.
6. **Documentación del progreso** — Se actualizó este documento para registrar el avance realizado.

#### Conclusiones de la sesión:
- El proyecto ya cuenta con una base funcional visible desde la interfaz.
- La aplicación permite cargar datasets en CSV/XLSX y ejecutar cálculos estadísticos básicos en memoria.
- La arquitectura queda preparada para ampliar con más visualizaciones o refinamientos de UX.

### Sesión 1 — 5 de agosto de 2026

#### Prompt del usuario:
> *"Quiero que en la carpeta de consigna hagas un archivo `contexto-autogenerado.md` en el que documentes los prompts y lo que fuiste haciendo a lo largo del proyecto, es un proyecto de la universidad y quiero tener todo bien armado. En la carpeta consigna, lee el archivo `modelo-del-problema.md`, ahí se encuentran las instrucciones y el contexto de todo lo que se tiene que hacer, refiérete a ese archivo cuando necesites contexto de algo."*

#### Acciones realizadas:
1. **Exploración de la estructura del proyecto** — Se listó el contenido de todas las carpetas principales (`/`, `consigna/`, `Pages/`, `Layout/`, `wwwroot/`, `wwwroot/css/`).
2. **Lectura del documento de consigna** — Se leyó completamente `modelo-del-problema.md` (135 líneas) para entender todos los requisitos del proyecto.
3. **Lectura de todos los archivos de código** — Se leyeron los 15 archivos del proyecto para evaluar el estado actual de la implementación:
   - `App.razor`, `Program.cs`, `_Imports.razor`, `DashboardEstadisticaII.csproj`
   - `Pages/Home.razor`, `Pages/Counter.razor`, `Pages/Weather.razor`, `Pages/NotFound.razor`
   - `Layout/MainLayout.razor`, `Layout/NavMenu.razor`
   - `wwwroot/index.html`, `wwwroot/css/app.css`
   - `deploy.sh`, `.gitignore`
4. **Análisis del historial de git** — Se revisaron todos los commits (6 en total) para construir la cronología del proyecto.
5. **Generación de este documento** — Se creó `contexto-autogenerado.md` con toda la documentación del proceso.

#### Conclusiones de la sesión:
- El proyecto está en fase **muy temprana**: solo se tiene el template de Blazor WASM con las dependencias de NuGet agregadas (`Blazor-ApexCharts`, `MathNet.Numerics`).
- El `deploy.sh` ya está funcional y se hizo un primer deploy exitoso a GitHub Pages el 30/07/2026.
- La consigna está completamente documentada en `modelo-del-problema.md`.
- **No hay código funcional del dashboard implementado aún** — todas las páginas son del template por defecto.

---

## 7. Documento de referencia principal

Toda la especificación funcional del proyecto se encuentra en:

📄 **[`consigna/modelo-del-problema.md`](file:///home/valentinomende/Desktop/Dashboard_EstadisticaII/consigna/modelo-del-problema.md)**

Este archivo contiene:
- Introducción y contexto de la empresa (Monopatines Eléctricos Voltio)
- Stack tecnológico elegido y justificación
- Variables seleccionadas para el análisis (cualitativas y cuantitativas)
- Especificación detallada de **Página 1** (módulo descriptivo/visual)
- Especificación detallada de **Página 2** (módulo de inferencia)
- Formato del archivo CSV/Excel de entrada
- Pasos de integración para la carga de archivos
- Pautas de UI/UX

---

## 8. Trabajo pendiente

### Prioridad Alta — Estructura base
- [ ] Crear modelo de datos `Venta` (Zona, ModoUso, Precio, Cantidad)
- [ ] Implementar componente de carga de CSV/Excel (`<InputFile>`)
- [ ] Parseo de CSV (nativo) y Excel (con ClosedXML — agregar paquete NuGet)
- [ ] Validación de columnas del archivo de entrada
- [ ] Eliminar páginas de ejemplo del template (Counter, Weather)

### Prioridad Alta — Página 1 (Módulo Descriptivo)
- [ ] Tabla de contingencia cruzada interactiva (frecuencias observadas + marginales)
- [ ] Gráfico de barras agrupadas / barras apiladas al 100%
- [ ] Indicador dinámico de chi-cuadrado con slider de nivel de significancia y p-valor
- [ ] Scatter plot interactivo con nube de puntos
- [ ] Línea de regresión muestral sobre el scatter plot
- [ ] Tarjetas KPI: coeficiente de correlación de Pearson (r) y determinación (R²)

### Prioridad Alta — Página 2 (Módulo de Inferencia)
- [ ] Prueba de hipótesis de independencia/homogeneidad (chi-cuadrado)
- [ ] Tabla de frecuencias esperadas y diferenciales relativas
- [ ] Verificación de supuestos y robustez
- [ ] Prueba de hipótesis para la pendiente (estadístico t, g.l., p-valor)
- [ ] Intervalos de confianza con slider (90%–99%) para pendiente/ordenada al origen
- [ ] Calculadora de predicción (IC para valor medio e IP para observación individual)
- [ ] Gráfico de residuos vs. valores ajustados
- [ ] Q-Q Plot / histograma de residuos

### Prioridad Media — UI/UX
- [ ] Diseñar identidad visual de Monopatines Voltio
- [ ] Layout con sidebar fija + contenido principal
- [ ] Paleta de colores consistente (fondo claro + color primario de la marca)
- [ ] Tarjetas con bordes suaves y sombra sutil
- [ ] Tipografía consistente con jerarquía clara
- [ ] Responsive para escritorio y tablet
- [ ] Transiciones cortas (150–250 ms)

### Prioridad Baja — Refinamiento
- [ ] Interpretaciones dinámicas contextualizadas (no genéricas)
- [ ] Estados visuales claros (hover, activo, deshabilitado)
- [ ] Optimización del tamaño del bundle
- [ ] Testing y validación con datasets reales

---

> **Nota:** Este documento se actualizará conforme avance el desarrollo del proyecto. Cada sesión de trabajo con IA agregará su sección correspondiente en el punto 6 (Prompts y acciones).
