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
    ["abc123"] = "https://google.com"
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

// add duplicate key with TryAdd
if (!links.TryAdd("abc123", "facebook.com"))
{
    Console.WriteLine("Key already exists!");
}

// add duplicate key with ContainsKey
if (links.ContainsKey("abc123"))
{
    Console.WriteLine("Key already exists!");
} else
{
    links.Add("abc123", "https://facebook.com");
}


string url = "https://short.example";

// test for valid code
string validCode = "abc123";
Console.WriteLine(BuildShortUrl(url, validCode));

// test for invalid code
string invalidCode = "ab";
Console.WriteLine(BuildShortUrl(url, invalidCode));