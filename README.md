# URL Shortener

A C#/.NET learning project that starts as a small console application and will gradually evolve into an ASP.NET Core URL shortener API.

> **Current status:** Milestone 001 — console prototype in progress. This is **not yet an HTTP API**: it does not serve requests, redirect visitors, generate unique short codes, or store links.

## What it does today

The console program implements two functions:

- `IsValidCode(string code)` — accepts codes containing at least 4 characters.
- `BuildShortUrl(string domain, string code)` — checks the code and constructs a URL string such as `https://short.example/abc123`. For a code shorter than 4 characters, it returns `Invalid short code`.

The current console entry point demonstrates a **valid** code. The invalid-code demonstration is still to be added as part of Milestone 001.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)

Check your installed SDK with:

```powershell
dotnet --version
```

## Run locally

```powershell
git clone https://github.com/lucifron28/url-shorterner-api.git
cd url-shorterner-api
dotnet run
```

With the currently hardcoded valid example, the output is:

```text
https://short.example/abc123
```

You can also check compilation with `dotnet build`.

## Project files

| File | Purpose |
| --- | --- |
| `Program.cs` | Console entry point, short-code validation, and URL formatting |
| `url-shorterner-api.csproj` | .NET 10 console project settings |
| `.gitignore` | Excludes generated build artifacts and common local files |

## Development milestones

- [x] Initialize a .NET 10 console project.
- [x] Implement basic short-code length validation.
- [x] Construct a formatted short URL from a domain and code.
- [x] Reject too-short codes in the URL-building function.
- [ ] Demonstrate both valid and invalid codes in the console and review the implementation.
- [ ] Store multiple original-URL/short-code mappings in memory.
- [ ] Convert the console prototype into an ASP.NET Core API.
- [ ] Implement actual HTTP redirects.
- [ ] Add SQL persistence with Entity Framework Core.
- [ ] Introduce appropriate validation, tests, security, and deployment.

These are learning milestones, **not claims of implemented features**. The project will evolve incrementally rather than introducing unnecessary architecture upfront.

## Learning approach

This repository stores the **application source code**. Lesson notes, conceptual explanations, assessments, and progress are tracked separately in the [engineering learning vault](https://github.com/lucifron28/engineering-learning-vault).

The goal is to understand and implement each concept independently, commit working increments, and review the actual code before introducing the next feature.
