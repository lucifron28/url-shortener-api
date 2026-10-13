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

string url = "https://short.example";
string code = "abc123";

Console.WriteLine(BuildShortUrl(url, code));
