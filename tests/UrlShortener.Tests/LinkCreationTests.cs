namespace UrlShortener.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;

public class LinkCreationTests
{


    [Fact]
    public async Task ReservedHealthCode_ReturnsBadRequest()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        // Arrange
        var data = new
        {
            code = "health",
            url = "https://github.com"
        };

        // Act 
        var res = await client.PostAsJsonAsync("/links", data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task ShortCodeCreation_ReturnsCreated()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        // Arrange
        var data = new
        {
            code = "qwerty",
            url = "https://drive.google.com/drive/folders/1PIO348agGm3xpoHFLZT7arxYU3Jhh5jz?usp=drive_link" 
        };

        // Act
        var res = await client.PostAsJsonAsync("/links", data);

        // Assert
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task ShortCodeCreation_ReturnsExpectedLocationHeader()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        // Arrange
        var data = new
        {
            code = "qwerty",
            url = "https://example.com"
        };

        // Act
        var res = await client.PostAsJsonAsync("/links", data);

        // Assert

        var LocationHeader = res.Headers.Location?.ToString();

        Assert.Equal("/qwerty", LocationHeader);
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }
}