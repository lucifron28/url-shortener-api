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
        var res = await client.PostAsJsonAsync($"/links", data);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }
}