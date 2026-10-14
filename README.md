# URL Shortener

A learning project in **C#, .NET 10, and ASP.NET Core Minimal APIs**. This began as a console exercise and is being expanded one backend concept at a time.

> **Current status:** Milestones 001–004 and the concurrent-collection exercise passed source review. The API creates in-memory mappings using HTTP POST and redirects known codes using HTTP GET. The learner reported manually verifying that a second identical POST returns 409. The reviewer inspected source, not the application's runtime; this is **not a production-ready URL shortener**.

## API behavior

The server listens at `http://localhost:5001`.

| Request | Expected response | Details |
| --- | --- | --- |
| `GET /health` | `200 OK` | Body: `OK` |
| `GET /abc123` | `302 Found` | `Location: https://google.com` (seeded mapping) |
| `GET /missing` | `404 Not Found` | Unknown code |
| `POST /links` with a new, valid code | `201 Created` | `Location: /{code}`; JSON response echoes submitted request |
| `POST /links` with a short or blank/missing code | `400 Bad Request` | Basic code validation |
| `POST /links` with a missing, relative or non-HTTP(S) destination | `400 Bad Request` | Parsed absolute URI with HTTP/HTTPS-only scheme policy |
| `POST /links` with an existing code | `409 Conflict` | Does not replace the existing mapping |

Example JSON body for `POST /links`:

```json
{
  "code": "dev123",
  "url": "https://github.com"
}
```

The new short link is accessible at **`GET /dev123`**, which redirects to the submitted URL.

`IsValidUrl(string? url)` uses `Uri.TryCreate(url, UriKind.Absolute, out Uri? parsed)` and allows only `http:` and `https:` schemes. The validator is used by `POST /links` before a mapping is inserted. This blocks unsupported URI schemes, **not** links to malicious HTTPS websites; broader redirect-abuse prevention remains future work. The short code `health` is now reserved case-insensitively because literal GET `/health` takes precedence over dynamic GET `/{code}`. The code `links` is not reserved yet because only POST `/links` is registered literally.

Mappings are held in an in-memory **`ConcurrentDictionary<string, string>`**:

```csharp
ConcurrentDictionary<string, string> links = new()
{
    ["abc123"] = "https://google.com"
};
```

The API's direct `TryAdd` and `TryGetValue` calls are thread-safe for concurrent access to this dictionary. It does **not** persist across process restarts or coordinate data across multiple running server processes. It also doesn't automatically make multi-step application workflows atomic.

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
curl.exe -i http://localhost:5001/abc123
curl.exe -i http://localhost:5001/missing
```

The `api.http` file contains example POST requests for both valid and invalid destinations (such as scheme-less, `javascript:`, and `file:` URLs). The learner reported testing these plus temporary console checks for null and empty strings; the reviewer did not independently run the application. You can send the valid one twice **without restarting the server** to check for 409 Conflict on the second attempt; submit the short-code example to check the 400 branch. After adding a code, check `GET /dev123` for the redirect. `curl.exe -I` sends **HEAD**, not GET; use `-i` for these GET examples.

## Main files

| File | Purpose |
| --- | --- |
| `Program.cs` | Minimal API, request record, in-memory concurrent mappings, validation, HTTP routes |
| `api.http` | Manual REST Client request examples (not automated tests) |
| `url-shorterner-api.csproj` | .NET 10 ASP.NET Core Web SDK project (original filename retained) |

## Learning milestones

- [x] 001 — Console prototype: validate and format short codes
- [x] 002 — Dictionary URL mappings, safe lookup and duplicate handling
- [x] 003 — Minimal API `GET /health`, redirect and 404 responses
- [x] 004 — JSON-bound `POST /links` with 201, 400 and 409 responses (source-reviewed; manual duplicate check user-reported)
- [x] 005 — Concurrency exercise: use `ConcurrentDictionary` for safe concurrent link insertion and lookup (source-reviewed)
- [x] 006 — Validate destinations as absolute HTTP(S) URIs before insertion (source-reviewed; manual tests learner-reported)
- [x] 007 — Reject reserved short code `health` case-insensitively (source-reviewed)
- [ ] Revisit additional reserved names if new GET routes are added; address phishing/redirect-abuse risks
- [ ] Persist mappings using SQL and Entity Framework Core
- [ ] Add automated HTTP tests, logging, secure configuration and deployment

Future work should include URL validation and abuse prevention before exposure to untrusted clients. Automated tests and independent runtime/load verification are not claimed here.

Conceptual lessons and course progress are tracked in the [engineering-learning-vault](https://github.com/lucifron28/engineering-learning-vault).
