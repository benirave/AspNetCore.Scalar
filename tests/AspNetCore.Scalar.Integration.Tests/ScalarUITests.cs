using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Scalar.Integration.Tests;

public class ScalarUITests
{
    private const string ScalarUIPath = "scalar-api-docs";

    [Fact]
    public async Task UseScalar_ShouldRenderScalarUI()
    {
        // Arrange
        using var client = BuildServer().CreateClient();

        // Act
        var healthResponse = await client.GetAsync("health");
        var scalarResponse = await client.GetAsync($"{ScalarUIPath}/index.html");

        // Assert
        Assert.True(healthResponse.IsSuccessStatusCode, "Api did not start");
        Assert.True(scalarResponse.IsSuccessStatusCode, "Scalar UI did not render");
    }

    private static TestServer BuildServer()
    {
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddEndpointsApiExplorer();
                services.AddRouting();
                services.AddSwaggerGen();
            })
            .Configure(app =>
            {
                app.UseRouting();
                app.UseSwagger();
                app.UseScalar();

                app.UseEndpoints(endpointBuilder =>
                {
                    endpointBuilder.MapGet("/health", () =>
                    {
                        return "OK";
                    });
                });
            })
            .UseTestServer();

        return new TestServer(builder);
    }
}