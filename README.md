# URL Shortener

A learning project in **C#, .NET 10, and ASP.NET Core Minimal APIs**. This began as a console exercise and is being expanded one backend concept at a time.

> **Current status (2026-10-14):** Backend lessons 001–013 covering C#, HTTP, Minimal APIs, in-memory concurrent link creation, validation and reserved codes passed source review. Lesson 014 automated HTTP testing is in progress. Test project setup exists; the learner reports `dotnet test UrlShortener.slnx` builds successfully and runs **one empty generated sample test** (not an API integration test). Reviewer has not independently executed the application. This is **not a production-ready URL shortener**.

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

## Automated test project (scaffolded, not completed)

This repository includes `UrlShortener.slnx`, which groups the root API and `tests/UrlShortener.Tests/UrlShortener.Tests.csproj`. The test project references the API and uses xUnit plus `Microsoft.AspNetCore.Mvc.Testing` for future in-process HTTP integration tests. Root API compilation excludes test source files; the application exposes a partial `Program` class to its test assembly.

Run the existing test project with:

```powershell
dotnet test UrlShortener.slnx
```

**Verification boundary:** The learner's local output on 2026-10-14 shows **1 passed, 0 failed** and compiler warning `CS8321` (unused `BuildShortUrl`). The only test at the audited GitHub HEAD is `tests/UrlShortener.Tests/UnitTest1.cs`, containing an empty `[Fact]` method: it verifies xUnit test discovery, **not endpoint behavior**. The first HTTP integration test is being written separately, with the initial expected result being POST `/links` + reserved `health` code → `400 Bad Request`.

## Main files

| File | Purpose |
| --- | --- |
| `Program.cs` | Minimal API, request record, in-memory concurrent mappings, validation, HTTP routes |
| `api.http` | Manual REST Client request examples (not automated tests) |
| `url-shorterner-api.csproj` | .NET 10 ASP.NET Core Web SDK project (original filename retained) |
| `UrlShortener.slnx` | Solution grouping API and xUnit test projects |
| `tests/UrlShortener.Tests/` | xUnit testing scaffold and future HTTP integration tests |

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
- [x] 008 — Set up xUnit project and solution; local test runner executes one empty sample test (user-reported, source-reviewed)
- [ ] Write and run meaningful automated HTTP integration tests
- [ ] Add logging, secure configuration and deployment

Future work should include URL validation and abuse prevention before exposure to untrusted clients. Automated tests and independent runtime/load verification are not claimed here.

Conceptual lessons and course progress are tracked in the [engineering-learning-vault](https://github.com/lucifron28/engineering-learning-vault).
