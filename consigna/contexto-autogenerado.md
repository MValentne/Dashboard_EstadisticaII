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
| Gráficos | **SVG personalizado + Blazor-ApexCharts (instalado)** | Permite mostrar visualizaciones simples de forma rápida y mantener flexibilidad para futuras mejoras |
| Cálculos estadísticos | **MathNet.Numerics** (v5.0.0) | Librería robusta para estadística y álgebra lineal en .NET |
| CSS | **Bootstrap** (incluido en wwwroot) + CSS personalizado | Framework CSS familiar, ya incluido en el template de Blazor |
| Lectura de Excel | **ClosedXML** | Permite procesar archivos `.xlsx` de forma directa en memoria |

### Paquetes NuGet instalados (según `.csproj`)

```xml
<PackageReference Include="Blazor-ApexCharts" Version="7.0.0" />
<PackageReference Include="ClosedXML" Version="0.104.2" />
<PackageReference Include="MathNet.Numerics" Version="5.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.10" />
```

### Decisiones de diseño clave

- **Sin servidor:** Blazor WASM corre enteramente en el browser, no hay backend.
- **Procesamiento en memoria:** El CSV/Excel se procesa en el cliente, no se guarda en disco.
- **Minimalismo en dependencias:** Se prioriza claridad y mantenibilidad sobre sobreingeniería.
- **Deploy automatizado:** Script `deploy.sh` publica automáticamente a GitHub Pages (rama `gh-pages`).
- **Enfoque pedagógico:** El dashboard busca demostrar conceptos estadísticos de forma visual, accesible y verificable.

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

### Resumen: El proyecto ya no se encuentra en estado de template inicial; ahora cuenta con una implementación funcional de un dashboard estadístico en Blazor WASM.

#### Componentes implementados
- **Modelo de datos** — Se creó `Venta` y los objetos auxiliares para resultados de análisis estadístico.
- **Carga de archivos** — La aplicación permite cargar datasets desde CSV o Excel en memoria mediante `InputFile`.
- **Página 1 — Módulo descriptivo** — Se incorporó una vista de análisis con tabla de contingencia, indicadores de chi-cuadrado, regresión lineal simple, correlación y gráficos básicos de dispersión.
- **Página 2 — Módulo de inferencia** — Se agregó una segunda vista para pruebas de hipótesis, intervalos de confianza, predicción avanzada y diagnóstico de residuos.
- **Servicios de negocio** — Se implementaron `DatosService` y `EstadisticaService` para separar carga y cálculo estadístico.
- **Estilo visual base** — Se agregó una apariencia más coherente para el dashboard con tarjetas, métricas y un look más profesional.
- **Verificación** — El proyecto compila correctamente con `dotnet build`.

#### Archivos principales del estado actual
- **`Program.cs`** — Registra los servicios de datos y estadística.
- **`Pages/Home.razor`** — Vista principal del dashboard descriptivo.
- **`Pages/Inferencia.razor`** — Vista de inferencia estadística.
- **`Services/DatosService.cs`** — Parseo y validación de archivos CSV/XLSX.
- **`Services/EstadisticaService.cs`** — Cálculos de chi-cuadrado, regresión, residuos e intervalos.
- **`Models/Venta.cs`** — Modelo de datos y resultados estadísticos.
- **`wwwroot/css/app.css`** — Estilos base del dashboard.

### Objetivos cumplidos hasta el momento
- ✅ Modelo de datos para ventas y resultados estadísticos.
- ✅ Carga de archivos CSV/Excel en memoria.
- ✅ Tabla de contingencia y prueba chi-cuadrado.
- ✅ Regresión lineal simple con r y R².
- ✅ Intervalos de confianza y predicción para la pendiente.
- ✅ Gráficos de residuos y Q-Q plot.
- ✅ Navegación entre módulos descriptivo e inferencial.
- ✅ Vista inicial con resumen del archivo cargado y métricas rápidas.
- ✅ Implementación de una estructura de dashboard más clara para el usuario final.

### Refinamientos pendientes
- 🔄 Pulir la interfaz visual para que se acerque más a una presentación final académica.
- 🔄 Mejorar los gráficos para que sean más claros, más legibles y más “de dashboard”.
- 🔄 Eliminar o adaptar páginas de ejemplo del template si se desea un producto más limpio.
- 🔄 Afinar textos explicativos y conclusiones para una exposición más sólida.
- 🔄 Mejorar el encabezado y la barra de navegación para reforzar la identidad visual del proyecto.

---

## 5. Estructura de archivos

```
Dashboard_EstadisticaII/
├── App.razor                          # Router principal de Blazor
├── Program.cs                         # Registro de servicios y componentes raíz
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
├── Models/                            # Modelos de negocio
│   └── Venta.cs                       # Clase Venta y resultados estadísticos
│
├── Pages/                             # Páginas/Rutas de Blazor
│   ├── Home.razor                     # Dashboard descriptivo principal
│   ├── Inferencia.razor               # Módulo de inferencia estadística
│   ├── Counter.razor                  # Ejemplo del template (pendiente de limpieza)
│   ├── Weather.razor                  # Ejemplo del template (pendiente de limpieza)
│   └── NotFound.razor                 # Página 404
│
├── Services/                          # Lógica de negocio y cálculo
│   ├── DatosService.cs                # Carga y parseo de archivos
│   └── EstadisticaService.cs          # Cálculos estadísticos
│
├── Layout/                            # Layout y navegación
│   ├── MainLayout.razor               # Layout principal (sidebar + contenido)
│   ├── MainLayout.razor.css           # Estilos del layout
│   ├── NavMenu.razor                  # Menú de navegación lateral
│   └── NavMenu.razor.css              # Estilos del menú
│
├── wwwroot/                           # Archivos estáticos
│   ├── index.html                     # HTML raíz de la SPA
│   ├── css/
│   │   └── app.css                    # Estilos globales del dashboard
│   └── lib/
│       └── bootstrap/                 # Bootstrap CSS
│
├── Properties/                        # Configuración de launch
├── bin/                               # Binarios compilados
├── obj/                               # Archivos intermedios de build
└── release/                           # Output del publish para deploy
```

---

## 6. Prompts y acciones realizadas con IA

### Resumen ejecutivo de objetivos cumplidos

En esta etapa del proyecto se logró avanzar desde una base de template de Blazor hacia una aplicación funcional que ya permite:
- cargar archivos CSV/XLSX,
- procesar datos de ventas en memoria,
- presentar un dashboard descriptivo con tabla de contingencia y regresión,
- mostrar un módulo de inferencia con intervalos, predicción y diagnóstico de residuos,
- y dejar el proyecto en un estado compilable y verificable.

Este avance es importante porque cubre los bloques centrales de la consigna: análisis descriptivo, inferencia estadística y carga de datos desde un archivo de entrada.

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

### Sesión 4 — 5 de agosto de 2026 (refinamiento visual)

#### Prompt del usuario:
> *"sigue mejorando el proyecto y haciendo lo que haga falta"*

#### Acciones realizadas:
1. **Refinamiento visual del layout** — Se mejoró la barra superior, la identidad visual del sidebar y el fondo general de la aplicación.
2. **Mejora de tarjetas y métricas** — Se ajustaron los estilos de las tarjetas para que la información se perciba con mayor jerarquía y claridad.
3. **Pulido de la experiencia de navegación** — Se reemplazó el contenido genérico del encabezado por un texto más alineado con el tema del proyecto.
4. **Verificación del estado del producto** — Se compiló el proyecto nuevamente para garantizar que los cambios no rompieran la aplicación.

#### Conclusiones de la sesión:
- El dashboard quedó con una presentación más coherente y profesional para una entrega académica.
- El enfoque ahora está en pulir la experiencia del usuario y reforzar la identidad del proyecto.
- El producto ya se encuentra en un estado mucho más cercano a la versión final esperada.

### Sesión 3 — 5 de agosto de 2026 (continuación)

#### Prompt del usuario:
> *"nuevamente refierete a contexto-autogenerado.md y actualiza lo que se ha logrado, despues de dar retroalimentaciones continua con lo que falte"*

#### Acciones realizadas:
1. **Se amplió la experiencia del dashboard** — Se agregó una segunda página dedicada a inferencia estadística para cerrar el flujo entre análisis descriptivo y análisis técnico.
2. **Se incorporó la navegación entre vistas** — Ahora el usuario puede moverse entre la vista inicial y la vista de inferencia desde la barra lateral.
3. **Se completó el módulo de inferencia** — Se añadieron pruebas de hipótesis, intervalos de confianza, predicción avanzada y diagnóstico de residuos en una vista coherente.
4. **Se consolidó la documentación** — Se actualizó `contexto-autogenerado.md` con el avance alcanzado y el estado del proyecto.
5. **Se reforzó la estructura visual del producto** — Se incorporaron tarjetas, métricas y una organización más clara de bloques de contenido para que la experiencia sea más intuitiva.

#### Conclusiones de la sesión:
- El proyecto ya posee una estructura de dashboard más completa y alineada con la consigna.
- Se cubre tanto el análisis descriptivo como el módulo inferencial, que eran los bloques principales del trabajo.
- El siguiente paso natural es pulir la interfaz visual y refinar algunos detalles estadísticos para acercarse más al producto final.

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

### Estado actual de cumplimiento

La mayor parte de los objetivos funcionales principales ya se encuentran implementados. Los puntos que siguen pendientes son principalmente de refinamiento y cierre visual.

### Prioridad Media — Estructura y funcionalidad base
- [x] Crear modelo de datos `Venta` (Zona, ModoUso, Precio, Cantidad)
- [x] Implementar componente de carga de CSV/Excel (`<InputFile>`)
- [x] Parseo de CSV (nativo) y Excel (con ClosedXML)
- [x] Validación de columnas del archivo de entrada
- [ ] Eliminar o limpiar páginas de ejemplo del template (Counter, Weather)

### Prioridad Media — Página 1 (Módulo Descriptivo)
- [x] Tabla de contingencia cruzada con frecuencias observadas y marginales
- [x] Indicador dinámico de chi-cuadrado con nivel de significación y p-valor
- [x] Scatter plot con línea de regresión muestral
- [x] Tarjetas KPI con correlación de Pearson (r) y determinación (R²)
- [ ] Mejorar visualmente los gráficos y la disposición de la información

### Prioridad Media — Página 2 (Módulo de Inferencia)
- [x] Prueba de hipótesis de independencia/homogeneidad (chi-cuadrado)
- [x] Tabla de frecuencias esperadas y diferenciales relativas
- [x] Verificación de supuestos y robustez
- [x] Prueba de hipótesis para la pendiente (estadístico t, g.l., p-valor)
- [x] Intervalos de confianza y calculadora de predicción
- [x] Gráfico de residuos vs. valores ajustados
- [x] Q-Q Plot de residuos

### Prioridad Baja — UI/UX y refinamiento
- [ ] Diseñar identidad visual más fuerte de Monopatines Voltio
- [ ] Mejorar el diseño responsive y jerarquía visual
- [ ] Pulir textos explicativos y conclusiones estadísticas
- [ ] Optimizar la experiencia de uso para una entrega más polished
- [ ] Ajustar la barra superior y la navegación para una imagen más profesional

---

> **Nota:** Este documento se actualizará conforme avance el desarrollo del proyecto. El estado actual ya no coincide con el inicio del trabajo y refleja una versión mucho más avanzada del dashboard.
