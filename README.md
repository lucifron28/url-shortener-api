# URL Shortener

A learning project in **C#, .NET 10, and ASP.NET Core Minimal APIs**. This began as a console exercise and is being expanded one backend concept at a time.

> **Current status:** Milestones 001–004 passed source review. The API creates in-memory mappings using HTTP POST and redirects existing codes using HTTP GET. The learner reports manually verifying duplicate POST requests return 409. Build/runtime behavior has not been independently executed by the reviewer. This is **not a production-ready URL shortener**.

## API behavior

The server listens at `http://localhost:5001`.

| Request | Expected response | Details |
| --- | --- | --- |
| `GET /health` | `200 OK` | Body: `OK` |
| `GET /links/abc123` | `302 Found` | `Location: https://google.com` (seeded mapping) |
| `GET /links/missing` | `404 Not Found` | Unknown code |
| `POST /links` with a new, valid code | `201 Created` | `Location: /links/{code}`; JSON body echoes submitted request |
| `POST /links` with a code shorter than 4 characters or blank/missing | `400 Bad Request` | Basic code validation |
| `POST /links` with an existing code | `409 Conflict` | Does not replace the existing mapping |

Example JSON body for `POST /links`:

```json
{
  "code": "dev123",
  "url": "https://github.com"
}
```

The created resource is currently accessible at **`GET /links/dev123`**, which redirects to the stored URL. The route prefix is a deliberate current-implementation detail; the shorter `/{code}` route was used in an earlier milestone.

Mappings are held in an in-memory `Dictionary<string, string>` and are lost when the process restarts. `TryAdd` rejects existing keys during ordinary sequential requests, but it **does not make the shared dictionary thread-safe**. Concurrency control, proper URL validation and safe redirect policies are follow-up lessons.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

## Run locally

```powershell
git clone https://github.com/lucifron28/url-shortener-api.git
cd url-shortener-api
dotnet run
```

In a separate terminal:

```powershell
curl.exe -i http://localhost:5001/health
curl.exe -i http://localhost:5001/links/abc123
curl.exe -i http://localhost:5001/links/missing
```

Use `api.http` with a REST Client extension to send the JSON POST request. Sending the same valid POST twice **without restarting the server** is a simple manual check of the 409 duplicate-code branch. Sending a one-character code checks the 400 path. After creating `dev123`, check `GET /links/dev123` to inspect the redirect.

`curl.exe -I` issues **HEAD**, not GET; use `-i` for these GET examples.

## Main files

| File | Purpose |
| --- | --- |
| `Program.cs` | Minimal API, request record, in-memory mappings, validation and HTTP routes |
| `api.http` | Manual REST Client request examples |
| `url-shorterner-api.csproj` | .NET 10 ASP.NET Core Web SDK project (original filename retained) |

## Learning milestones

- [x] 001 — Console prototype: validate and format short codes
- [x] 002 — Dictionary URL mappings, safe lookup and duplicate handling
- [x] 003 — Minimal API `GET /health`, redirect and 404 responses
- [x] 004 — JSON-bound `POST /links` with 201, 400 and 409 responses (source-reviewed; manual duplicate check user-reported)
- [ ] Validate URL inputs and safe redirect behavior
- [ ] Handle concurrent dictionary access safely
- [ ] Add SQL persistence with Entity Framework Core
- [ ] Add automated HTTP tests, logging, secure configuration and deployment

The engineering-learning notes and assessed lesson history live in the [engineering-learning-vault](https://github.com/lucifron28/engineering-learning-vault). Source review does not replace local build verification or automated tests.
