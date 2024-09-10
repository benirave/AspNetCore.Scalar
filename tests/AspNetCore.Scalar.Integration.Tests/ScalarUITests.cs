using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
#if NET5_0
using Microsoft.AspNetCore.Mvc;
#endif

namespace AspNetCore.Scalar.Integration.Tests
{

    public class ScalarUITests
    {
        private const string ScalarUIPath = "scalar-api-docs";
        private const string DefaultSwaggerDocPath = "swagger/v1/swagger.json";

        [Fact]
        public async Task UseScalar_ShouldRenderScalarUI()
        {
            // Arrange
            using var client = BuildServer().CreateClient();

            // Act
            var swaggerDocResponse = await client.GetAsync(DefaultSwaggerDocPath);
            var healthResponse = await client.GetAsync("health");
            var scalarResponse = await client.GetAsync($"{ScalarUIPath}/index.html");

            // Assert
            Assert.True(healthResponse.IsSuccessStatusCode, "Api did not start");

            Assert.True(swaggerDocResponse.IsSuccessStatusCode, "Swagger doc did not load");
            Assert.True(DoesEndpointExistsInSwaggerJson(await swaggerDocResponse.Content.ReadAsStringAsync()), "Endpoint does not exists in swagger json");

            Assert.True(scalarResponse.IsSuccessStatusCode, "Scalar UI did not render");


            // Verify here that the initial html has the good swagger data-url and the configuration is set correctly

            // and test that the UI and everything is working with Playwright as an E2E test solution?
        }

        private static bool DoesEndpointExistsInSwaggerJson(string swaggerJson)
        {
            var doc = JsonSerializer.Deserialize<JsonElement>(swaggerJson);

            var path = doc.GetProperty("paths");
            return path.TryGetProperty("/health", out _);
        }

        private static TestServer BuildServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
#if NET6_0_OR_GREATER
   
                    services.AddEndpointsApiExplorer();
#endif
#if NET5_0
                    services.AddMvcCore()
                            .AddApiExplorer();
#endif
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
#if NET5_0
                        endpointBuilder.MapControllers();
#endif
#if NET6_0_OR_GREATER
                        endpointBuilder.MapGet("/health", () =>
                        {

                            return "OK";
                        });
#endif
                    });
                })
                .UseTestServer();

            return new TestServer(builder);
        }
    }
}

#if NET5_0

[ApiController]
public class TestController
{
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return new OkResult();
    }
}
#endif