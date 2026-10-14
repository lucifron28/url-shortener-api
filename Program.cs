using System.Collections.Concurrent;

bool IsValidCode(string code)
{
    return !string.IsNullOrWhiteSpace(code) && code.Length >= 4;
}

string BuildShortUrl(string domain, string code)
{
    if (IsValidCode(code))
    {
        return $"{domain}/{code}";
    } else
    {
        return "Invalid short code";
    }
}

bool IsValidUrl(string? url)
{
    if (Uri.TryCreate(url, UriKind.Absolute, out Uri? parsed))
    {
        if (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps)
        {
            return true;
        }
        return false;
    }
    return false;

}
Console.WriteLine(IsValidUrl("https://github.com"));

// returns false
Console.WriteLine(IsValidUrl("github.com"));
Console.WriteLine(IsValidUrl("javascript:alert(1)"));

ConcurrentDictionary<string, string> links = new()
{
    ["abc123"] = "https://google.com"
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "OK");

app.MapGet("/{code}", (string code) =>
{
    Console.WriteLine(code);
    if (links.TryGetValue(code, out string? value))
    {
        return Results.Redirect(value);
    } 
    return Results.NotFound();
});

app.MapPost("/links", (CreateLinkRequest request) =>
{
    if (!IsValidCode(request.Code))
    {
        return Results.BadRequest();
    }

    if (links.TryAdd(request.Code, request.Url))
    {
        return Results.Created($"/{request.Code}", request);
    } else
    {
        return Results.Conflict();
    }
});

app.Run("http://localhost:5001");
record CreateLinkRequest(string Code, string Url);