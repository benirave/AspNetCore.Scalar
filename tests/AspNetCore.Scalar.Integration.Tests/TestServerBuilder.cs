using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

#if NET5_0
using Microsoft.AspNetCore.Mvc;
#endif

namespace AspNetCore.Scalar.Integration.Tests
{
    internal static class TestServerBuilder
    {
        public static TestServer BuildServer(ScalarOptions options)
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
                    app.UseScalar(options);

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
}