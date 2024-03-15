using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetCore.Scalar
{
    public static class ScalarBuilderExtensions
    {
        public static IApplicationBuilder UseScalar(this IApplicationBuilder app, ScalarOptions options)
        {
            return app.UseMiddleware<ScalarMiddleware>(options);
        }

        public static IApplicationBuilder UseScalar(
            this IApplicationBuilder app,
            Action<ScalarOptions> setupAction = null)
        {
            ScalarOptions options;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ScalarOptions>>().Value;
                setupAction?.Invoke(options);
            }

            return app.UseScalar(options);
        }
    }
}