# URL Shortener

A learning project in **C#, .NET 10, and ASP.NET Core Minimal APIs**. This began as a console exercise and is being expanded one backend concept at a time.

> **Current status (2026-10-18):** Backend lessons 001–014 are recorded as completed, including the first meaningful xUnit HTTP integration test. The current source contains **one automated test**: `POST /links` with the reserved short code `health` must return **400 Bad Request**. The learner independently ran `dotnet test urlShortener.slnx` and provided output showing **1 passed, 0 failed, 0 skipped** on 2026-10-18; the reviewer inspected the pushed test source but **did not independently execute the application or test suite**. Other API branches are not yet covered by automated tests. This is **not a production-ready URL shortener**.

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

## Automated integration testing (first real test completed)

`UrlShortener.slnx` includes the ASP.NET Core API and `tests/UrlShortener.Tests/UrlShortener.Tests.csproj`. The test project references the API and uses xUnit with `Microsoft.AspNetCore.Mvc.Testing` to exercise the application's HTTP pipeline in-process using `WebApplicationFactory<Program>`. The API project excludes `tests/**/*.cs` from its compilation, and `Program.cs` exposes `public partial class Program { }` to support the integration test host.

The existing test lives in **`tests/UrlShortener.Tests/LinkCreationTests.cs`**:

- **Arrange:** Create an anonymous JSON payload with `code = "health"` and a valid HTTPS destination.
- **Act:** Send `POST /links` with `PostAsJsonAsync` using the factory's `HttpClient`.
- **Assert:** Compare `res.StatusCode` with `HttpStatusCode.BadRequest` (400).

The original generated `UnitTest1.cs` (which contained an empty test) was **removed**; it is not the current automated test.

Run the .NET solution tests using PowerShell or a terminal:

```powershell
dotnet test UrlShortener.slnx
```

Alternatively, open the solution in VS Code with C# Dev Kit and run the test from the Testing view.

**Verification boundary:** Learner-provided local output for this meaningful test reported **1 passed, 0 failed, 0 skipped**, with a successful .NET 10 build. The output also included seven `True`/`False` lines printed by temporary URL validation probes in `Program.cs`; these are **not seven additional tests**. The reviewer inspected the committed test source and supplied output, but did **not independently run** the tests or verify a deployed server.

**Next coverage:** Test valid link creation (**201 Created** and its `Location` header), duplicates (409), missing codes (404), redirects (302), and invalid destinations (400), using isolated test data. When testing redirects through the factory client, disable automatic redirect following if the assertion targets the initial **302** and `Location` response. A later **Robot Framework API-testing module** is planned, but no Robot tests or dependencies have been added yet.

## Main files

| File | Purpose |
| --- | --- |
| `Program.cs` | Minimal API, request record, in-memory concurrent mappings, validation, HTTP routes |
| `api.http` | Manual REST Client request examples (not automated tests) |
| `url-shorterner-api.csproj` | .NET 10 ASP.NET Core Web SDK project (original filename retained) |
| `UrlShortener.slnx` | Solution grouping API and xUnit test projects |
| `tests/UrlShortener.Tests/LinkCreationTests.cs` | xUnit HTTP integration test of reserved-code rejection (400) |

## Learning milestones

- [x] 001 — Console prototype: validate and format short codes
- [x] 002 — Dictionary URL mappings, safe lookup and duplicate handling
- [x] 003 — Minimal API `GET /health`, redirect and 404 responses
- [x] 004 — JSON-bound `POST /links` with 201, 400 and 409 responses (source-reviewed; manual duplicate check user-reported)
- [x] 005 — Concurrency exercise: use `ConcurrentDictionary` for safe concurrent link insertion and lookup (source-reviewed)
- [x] 006 — Validate destinations as absolute HTTP(S) URIs before insertion (source-reviewed; manual tests learner-reported)
- [x] 007 — Reject reserved short code `health` case-insensitively (source-reviewed)
- [ ] Revisit additional reserved names if new GET routes are added; address phishing/redirect-abuse risks
- [x] 008 — Set up xUnit project and solution; verify test discovery and runner operation
- [x] 009 — Replace the empty sample test with a meaningful HTTP integration test: reserved `health` → 400 (source-reviewed; learner ran tests successfully)
- [ ] Add more focused automated HTTP regression cases, starting with a valid 201 and `Location` response
- [ ] Learn Robot Framework for external API testing (planned; not implemented)
- [ ] Persist mappings using SQL and Entity Framework Core
- [ ] Add logging, secure configuration and deployment

Destination scheme validation is implemented; broader redirect-abuse prevention is still needed before exposure to untrusted clients. **Only one automated HTTP scenario has been tested.** Independent execution of the full application, concurrency/load testing, and deployment verification are not claimed here.

Conceptual lessons and course progress are tracked in the [engineering-learning-vault](https://github.com/lucifron28/engineering-learning-vault).
