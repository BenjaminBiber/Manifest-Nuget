# BenjaminBiber.Manifest

Minimalistisches **CMS-Client**-Paket mit leichtgewichtigem **In-Memory-Cache** für .NET.  
Bietet `ICmsService`/`CmsService` zum Abrufen von JSON-Ressourcen und die DTO-Hüllen `ResponseItem<T>` und `ApiResult<T>`.

---

## Installation

~~~bash
dotnet add package BenjaminBiber.Manifest
~~~

---

## Schnellstart

**Program.cs**
~~~csharp
var builder = WebApplication.CreateBuilder(args);

// Registriert ICmsService (HttpClient mit BaseAddress) und einen Default-ICacheService
builder.Services.AddCms(
    baseUrl: "https://backente.benjaminbiber.de",
    apiPrefix: "/api" // optional, Default: "/api"
);

var app = builder.Build();
app.Run();
~~~

**Wichtig:** `baseUrl` + `apiPrefix` werden zu einer **BaseAddress** kombiniert  
(z. B. `https://host/api/`). Übergib später **relative** URLs **ohne führenden Slash**  
(z. B. `"articles?page=1"` statt `"/articles?page=1"`).

---

## Verwendung

~~~csharp
public sealed class DuckController(ICmsService cms) : ControllerBase
{
    [HttpGet("ducks")]
    public async Task<ResponseItem<DuckCategoryDto>> Get(CancellationToken ct)
    {
        var key = "ducks:categories:p1";           // Key fürs Caching frei wählen
        return await cms.GetItems<DuckCategoryDto>(key, "duck-categories?page=1", ct);
    }
}
~~~

`ResponseItem<T>` entspricht dem üblichen Paging-Envelope:
~~~csharp
public sealed class ResponseItem<T>
{
    public List<T> Data { get; init; } = [];
    public int CurrentPage { get; init; }
    public int LastPage { get; init; }
    public int From { get; init; }
    public int To { get; init; }
    public int Total { get; init; }
    public int PerPage { get; init; }
}
~~~

## Lizenz

MIT (falls nicht anders im Paket angegeben).
