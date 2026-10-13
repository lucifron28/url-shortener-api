# URL Shortener

A learning project built with **C#, .NET 10, and ASP.NET Core Minimal APIs**. It began as a console prototype and is being expanded one backend concept at a time.

> **Current status:** Milestones 001–003 passed source review. The app now handles HTTP GET requests and redirects a known short code. It **does not yet create links through HTTP**, generate short codes, or store data permanently.

## Current behavior

The server listens at `http://localhost:5001`.

| Request | Expected response | Notes |
| --- | --- | --- |
| `GET /health` | `200 OK`, body `OK` | Simple health endpoint |
| `GET /abc123` | `302 Found` | `Location: https://google.com` |
| `GET /missing` | `404 Not Found` | No matching dictionary key |

The application uses an in-memory `Dictionary<string, string>` to map short codes to original URLs:

```csharp
["abc123"] = "https://google.com"
```

The route `/{code}` uses `TryGetValue` and **returns** `Results.Redirect(value)` or `Results.NotFound()`. The earlier `IsValidCode` and `BuildShortUrl` helper methods remain in `Program.cs` as learning artifacts but are not yet used by the HTTP endpoints.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## Run locally

```powershell
git clone https://github.com/lucifron28/url-shortener-api.git
cd url-shortener-api
dotnet run
```

Use a separate terminal to test the server:

```powershell
curl.exe -i http://localhost:5001/health
curl.exe -i http://localhost:5001/abc123
curl.exe -i http://localhost:5001/missing
```

The second response should have status `302 Found` and a `Location: https://google.com` header. The third should have status `404 Not Found`. These results are inferred from the source; actual execution was not independently verified during the code review.

**Note:** `curl.exe -I` sends an HTTP **HEAD** request, not GET. The application currently registers only GET routes, so use `-i` rather than `-I` for this exercise.

## Project files

| File | Purpose |
| --- | --- |
| `Program.cs` | Minimal API setup, in-memory URL mappings, health route, redirect route |
| `url-shorterner-api.csproj` | .NET 10 ASP.NET Core Web SDK project (original filename retained) |
| `.gitignore` | Ignores local/generated files |

## Learning milestones

- [x] 001 — Console prototype: validate and format short codes
- [x] 002 — Use a dictionary for URL mappings, safe lookup, and duplicate-key handling
- [x] 003 — Run an ASP.NET Core server with `GET /health`, `GET /{code}`, 302 redirects and 404 responses
- [ ] 004 — Accept new URL mappings through an HTTP POST endpoint
- [ ] Add persistence with SQL and Entity Framework Core
- [ ] Add appropriate validation, tests, security, and deployment

This is a learning roadmap, not a feature list for a production-ready service. Source reviews do not replace local build and runtime tests.

Conceptual lessons and course progress are maintained separately in the [engineering-learning-vault](https://github.com/lucifron28/engineering-learning-vault).
