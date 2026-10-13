# URL Shortener

A C#/.NET learning project that starts as a small console application and will gradually evolve into an ASP.NET Core URL shortener API.

> **Current status:** Milestones 001 and 002 passed source review. This is still a **console prototype**, not an HTTP API. It does not serve requests, redirect visitors, generate unique short codes, or persist mappings between runs.

## What it does today

The console program implements two functions:

- `IsValidCode(string code)` — accepts codes containing at least 4 characters.
- `BuildShortUrl(string domain, string code)` — checks the code and constructs a URL string such as `https://short.example/abc123`. For a code shorter than 4 characters, it returns `Invalid short code`.

The console entry point demonstrates **both** a valid code (`abc123`) and an invalid code (`ab`). It also initializes an in-memory `Dictionary<string, string>` with a short-code-to-original-URL mapping, retrieves an existing entry, safely handles a missing key using `TryGetValue`, and demonstrates rejecting a duplicate through both `TryAdd` and `ContainsKey`.

The two duplicate-handling examples are alternative teaching approaches; a practical implementation would generally choose one instead of running both.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Check your installed SDK with:

```powershell
dotnet --version
```

## Run locally

```powershell
git clone https://github.com/lucifron28/url-shortener-api.git
cd url-shortener-api
dotnet run
```

With the currently hardcoded examples, the **expected** output from `Program.cs` is (source-reviewed, not independently executed):

```text
https://google.com
Short code not found
Key already exists!
Key already exists!
https://short.example/abc123
Invalid short code
```

You can also check compilation with `dotnet build`.

## Project files

| File | Purpose |
| --- | --- |
| `Program.cs` | Console entry point, short-code validation, URL formatting, and in-memory dictionary demonstrations |
| `url-shorterner-api.csproj` | .NET 10 console project settings |
| `.gitignore` | Excludes generated build artifacts and common local files |

## Development milestones

- [x] Initialize a .NET 10 console project.
- [x] Implement basic short-code length validation.
- [x] Construct a formatted short URL from a domain and code.
- [x] Reject too-short codes in the URL-building function.
- [x] Demonstrate both valid and invalid codes in the console and review the implementation (source-reviewed; run locally to verify output).
- [x] Introduce in-memory short-code-to-original-URL mappings and demonstrate existing, missing, and duplicate-code behavior (source-reviewed).
- [ ] Develop the mappings into real application behavior with HTTP endpoints and redirects.
- [ ] Convert the console prototype into an ASP.NET Core API.
- [ ] Implement actual HTTP redirects.
- [ ] Add SQL persistence with Entity Framework Core.
- [ ] Introduce appropriate validation, tests, security, and deployment.

These are learning milestones, **not claims of implemented features**. The project will evolve incrementally rather than introducing unnecessary architecture upfront.

## Learning approach

This repository stores the **application source code**. Lesson notes, conceptual explanations, assessments, and progress are tracked separately in the [engineering learning vault](https://github.com/lucifron28/engineering-learning-vault).

The goal is to understand and implement each concept independently, commit working increments, and review the actual code before introducing the next feature.
