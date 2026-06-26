# Azure Functions Clean Architecture Template

Plantilla de Azure Functions con arquitectura limpia, CQRS con Cortex.Mediator, FluentValidation, OpenTelemetry y Redis cache. Ideal para empezar proyectos nuevos con una base sólida y buenas prácticas.

---

## 🚀 Quick Start

### Como template de GitHub

1. Andá a [github.com/sergioabgomez/TempalteAzureFunction](https://github.com/sergioabgomez/TempalteAzureFunction)
2. Click en **"Use this template"** → **"Create a new repository"**
3. Clonás tu nuevo repo y ya tenés el código

### Como `dotnet new` template

```bash
# Instalar el template
dotnet new install TempalteAzureFunction

# Crear un proyecto nuevo
dotnet new func-clean -n MiNuevaApi

# Con framework custom
dotnet new func-clean -n MiNuevaApi --Framework net9.0

# Sin restore automático
dotnet new func-clean -n MiNuevaApi --SkipRestore true
```

| Parámetro | Default | Descripción |
|---|---|---|
| `-n` / `--name` | (obligatorio) | Nombre del proyecto |
| `--Framework` | `net10.0` | `net10.0` o `net9.0` |
| `--SkipRestore` | `false` | Saltea `dotnet restore` post-creación |

> El template se instala de forma permanente. Para desinstalarlo:
> ```bash
> dotnet new uninstall TempalteAzureFunction
> ```

### Como template de proyecto en Visual Studio

1. Cloná el repo o descargalo a tu máquina local
2. Instalá el template desde la terminal:
   ```bash
   dotnet new install "C:\ruta\completa\TempalteAzureFunction"
   ```
3. Abrí **Visual Studio 2026** (o 2022+)
4. **Create a new project**
5. Buscá **"Azure Functions Clean Architecture"**
6. Seleccioná la plantilla, asignále nombre y ubicación
7. Hacé click en **Create**

> El template queda registrado en tu sistema y aparece como opción en el diálogo "New Project" de Visual Studio.

---

## 🏗️ Estructura del Proyecto

```
├── FunctionTemplate.Host/              ← Entry point (Azure Functions)
│   ├── Functions/                      ← HTTP Triggers
│   │   ├── SampleGetFunction.cs        ← GET sample
│   │   └── SamplePostFunction.cs       ← POST sample
│   ├── Installers/                     ← DI module registration
│   │   ├── AzureInstaller.cs           ← Azure services
│   │   ├── InstalleHttpClient.cs       ← HTTP clients
│   │   ├── InstallerOptions.cs         ← IOptions pattern
│   │   ├── InstallerCache.cs           ← Redis/Memory cache
│   │   ├── FluentValidationInstaller.cs
│   │   ├── MediatorRegisterInstaller.cs
│   │   ├── ServicesInstaller.cs        ← Auto-registro de servicios
│   │   └── SwaggerInstaller.cs
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs  ← RFC 9457 error handling
│   └── Program.cs
│
├── FunctionTemplate.Application/       ← Lógica de aplicación
│   ├── Handlers/
│   │   ├── Commands/CreateSample/      ← Command + Handler + Validator
│   │   └── Queries/Sample/             ← Query + Handler + Validator
│   └── Models/
│       ├── Requests/
│       └── Responses/
│
├── FunctionTemplate.Domain/            ← Modelos de dominio
│   └── DammyDomain.cs                  ← Assembly marker
│
└── FunctionTemplate.Infrastructure/    ← Infraestructura
    ├── Attributes/                     ← FromQuery, FromHeader, FromBodyJson
    ├── Exceptions/AppException.cs      ← RFC 9457
    ├── Extensions/                     ← Auto-registro DI
    ├── Helpers/BindObject.cs           ← Model binding attribute-driven
    └── Services/                       ← IServiceScoped, ISingleton, ITransient
```

### Responsabilidad de cada capa

| Capa | Responsabilidad | Depende de |
|---|---|---|
| **Host** | Entry point, Functions, middlewares, DI | Application, Infrastructure |
| **Application** | Handlers CQRS, validators, DTOs | Domain, Infrastructure |
| **Domain** | Modelos puros, sin dependencias | — |
| **Infrastructure** | Binding, atributos, excepciones, DI helpers | — |

---

## 🔄 Flujo de Ejecución

```
HTTP Request
     ↓
Azure Function (HttpTrigger)
     ↓
BindObject.BindAsync<T>()    ← Attribute-driven binding ([FromQuery], [FromHeader], [FromBodyJson])
     ↓
Cortex.Mediator               ← CQRS dispatcher
     ↓
FluentValidation              ← Validación automática via pipeline behavior
     ↓
Handler                       ← QueryHandler / CommandHandler
     ↓
Response                      ← DTO de respuesta
```

### GET sample

```
GET /api/SampleGet?name=Mundo&uppercase=true
  → SampleGetFunction
    → BindObject → SampleQuery { Name = "Mundo", Uppercase = true }
      → FluentValidation (Name no vacío)
        → SampleQueryHandler
          → SampleResponse { Message = "HELLO, MUNDO!" }
```

### POST sample

```
POST /api/SamplePost  { "name": "MiItem", "description": "..." }
  → SamplePostFunction
    → BindObject → CreateSampleCommand { Name = "MiItem", Description = "..." }
      → FluentValidation (Name requerido, max 100 chars)
        → CreateSampleCommandHandler
          → CreateSampleResponse { Id = guid, Name = "MiItem", CreatedAt = utc }
```

---

## 📦 Stack Tecnológico

| Tecnología | Versión | Propósito |
|---|---|---|
| .NET | 10.0 | Target framework |
| Azure Functions | v4 (Isolated Worker) | Serverless host |
| Cortex.Mediator | 3.1.2 | CQRS / Mediator pattern |
| FluentValidation | 12.1.1 | Validación de commands/queries |
| Newtonsoft.Json | 13.0.4 | Serialización |
| OpenTelemetry | 1.16.0 | Telemetría y monitoreo |
| Azure Monitor OpenTelemetry | 1.8.1 | Export a Azure Monitor |
| Scalar | 2.16.2 | UI de referencia OpenAPI |
| StackExchange.Redis | 2.13.17 | Caché distribuida (opcional) |
| Microsoft.Extensions.Caching | 10.0.9 | MemoryCache + Redis |

---

## 💡 Samples Incluidos

El template viene con dos endpoints funcionales que muestran el patrón completo:

### `GET /api/SampleGet`

Ejemplo de **Query** con parámetros de query string y validación.

- **Query**: `SampleQuery` — `Name` (requerido), `Uppercase` (opcional)
- **Validator**: `SampleQueryValidator` — Name not empty + max 100
- **Handler**: `SampleQueryHandler` — devuelve mensaje, opcionalmente en mayúsculas
- **Response**: `SampleResponse { Message }`

### `POST /api/SamplePost`

Ejemplo de **Command** con body JSON y validación.

- **Command**: `CreateSampleCommand` — `Name` (requerido), `Description` (opcional)
- **Validator**: `CreateSampleCommandValidator` — Name not empty + max 100, Description max 500
- **Handler**: `CreateSampleCommandHandler` — genera ID y timestamp
- **Response**: `CreateSampleResponse { Id, Name, Description, CreatedAt }`

---

## 🧩 Arquitectura

### CQRS con Mediator

Commands y Queries se separan en carpetas distintas (`Commands/`, `Queries/`). Cada uno tiene su propio handler y validator. Cortex.Mediator se encarga del dispatch automático.

```
Query  → IQuery<TResponse>  → IQueryHandler<TQuery, TResponse>
Command → ICommand<TResponse> → ICommandHandler<TCommand, TResponse>
```

### Patrón Installers

La DI se organiza en módulos (installers) que implementan `IInstallerServiceCollection` y se auto-descubren via reflección en `Program.cs`. Cada installer es responsable de una categoría de servicios.

```csharp
public class MiInstaller : IInstallerServiceCollection
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMiService, MiService>();
    }
}
```

### Model Binding (BindObject)

`BindObject.BindAsync<T>()` resuelve cada propiedad de tu DTO según su atributo:

| Atributo | Fuente |
|---|---|
| `[FromQuery]` | Query string |
| `[FromHeader("X-Token")]` | HTTP header |
| `[FromBodyJson]` | Body JSON |
| *(sin atributo)* | Body primero, fallback a query |

Los nombres en query string se convierten automáticamente a `snake_case`.

### Auto-Registro de Servicios

Las interfaces marcadoras (`IServiceScoped`, `IServiceSingleton`, `IServiceTransient`) permiten registrar servicios automáticamente sin configuración manual:

```csharp
public class MiService : IMiService, IServiceScoped { }
```

El `ServicesInstaller` escanea los assemblies y registra todo lo que implementa estas interfaces con el lifetime correspondiente.

### Exception Handling

`ExceptionHandlingMiddleware` captura excepciones y las convierte a respuestas **RFC 9457 Problem Details**:

| Excepción | HTTP Status | Formato |
|---|---|---|
| `AppException` | Custom (vía StatusCode) | `{ type, title, status, detail }` |
| `ValidationException` | 400 Bad Request | `{ errors: [...] }` |
| `Exception` | 500 Internal Server Error | `{ trace: stack }` |

---

## 🛠️ Personalización

### Cómo agregar un nuevo endpoint (GET)

```csharp
// 1. Query
public class ListItemsQuery : IQuery<ListItemsResponse>
{
    [FromQuery] public string Filter { get; init; }
}

// 2. Handler
public class ListItemsQueryHandler : IQueryHandler<ListItemsQuery, ListItemsResponse>
{
    public Task<ListItemsResponse> Handle(ListItemsQuery query, CancellationToken ct)
    {
        // tu lógica acá
    }
}

// 3. Validator (opcional)
public class ListItemsQueryValidator : AbstractValidator<ListItemsQuery>
{
    public ListItemsQueryValidator() => RuleFor(x => x.Filter).NotEmpty();
}

// 4. Response
public record ListItemsResponse(List<string> Items);

// 5. Function
public class ListItemsFunction
{
    [Function("ListItems")]
    [OpenApiOperation(...)]
    public async Task<HttpResponseData> Run(...)
    {
        var query = await BindObject.BindRequiredAsync<ListItemsQuery>(req);
        var result = await mediator.SendQueryAsync<ListItemsQuery, ListItemsResponse>(query);
        // ...
    }
}
```

### Cómo agregar un nuevo endpoint (POST)

Mismo patrón pero con `ICommand<T>`, `ICommandHandler<T1, T2>`, atributos `[FromBodyJson]`, y `mediator.SendCommandAsync(...)`.

### Cómo agregar un servicio

```csharp
public interface IWeatherService : IServiceScoped { Task<int> GetTemperatureAsync(); }
public class WeatherService : IWeatherService { /* impl */ }
```

Se registra automáticamente por `ServicesInstaller`. No hace falta tocarlo.

---

## 📄 Licencia

MIT
