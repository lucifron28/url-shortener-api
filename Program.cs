using Microsoft.Extensions.Options;

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

Dictionary<string, string> links = new()
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
        return Results.Created($"links/{request.Code}", request);
    } else
    {
        return Results.Conflict();
    }
});

app.Run("http://localhost:5001");
record CreateLinkRequest(string Code, string Url);