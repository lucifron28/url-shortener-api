bool IsValidCode(string code)
{
    return code.Length >= 4;
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
    ["abc123"] = "google.com"
};

// get value
Console.WriteLine(links["abc123"]);

// get value of a key that doesnt exist
if (links.TryGetValue("missing", out string? value))
{
    Console.WriteLine(value);
} else
{
    Console.WriteLine("Short code not found");
}

// add duplicate key
links.Add("abc123", "facebook.com");