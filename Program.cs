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

// test for valid code
string validCode = "abc123";
Console.WriteLine(BuildShortUrl(url, validCode));

// test for invalid code
string invalidCode = "ab";
Console.WriteLine(BuildShortUrl(url, invalidCode));

