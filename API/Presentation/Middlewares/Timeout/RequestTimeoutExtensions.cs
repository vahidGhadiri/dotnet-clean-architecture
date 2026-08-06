using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace API.Presentation.Middlewares.Timeout;

public static class RequestTimeoutExtensions
{
    public static IServiceCollection AddRequestTimeout(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RequestTimeoutOptions>(configuration.GetSection("RequestTimeout"));
        return services;
    }

    public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestTimeoutMiddleware>();
    }
}
