namespace API.Presentation.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Asp.Versioning;
using System.Text;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Token key does not found");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuer = false,
            };
        });

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();

                document.Components.SecuritySchemes =
                    new Dictionary<string, IOpenApiSecurityScheme>
                    {
                        {
                            "Bearer",
                            new OpenApiSecurityScheme
                            {
                                Description = "Enter JWT token",
                                Type = SecuritySchemeType.Http,
                                BearerFormat = "JWT",
                                Scheme = "bearer",
                            }
                        }
                    };

                document.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecuritySchemeReference("Bearer", document, null),
                            new List<string>()
                        }
                    }
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static WebApplication MapIdentityEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1, 0))
                .Build();

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
                    options.AddPreferredSecuritySchemes("Bearer")
                        .WithTitle(".NET Clean Architecture")
                        .WithTheme(ScalarTheme.BluePlanet))
                .WithSummary("Interactive API documentation")
                .WithRequestTimeout(TimeSpan.FromSeconds(30))
                .WithDescription(".NET Clean Architecture")
                .WithDisplayName("Scalar API Reference")
                .WithHttpLogging(HttpLoggingFields.All)
                .WithApiVersionSet(apiVersionSet)
                .WithTags("Scalar", "OpenAPI")
                .WithMetadata(apiVersionSet)
                .WithGroupName("v1")
                .WithOrder(1);
        }

        return app;
    }
}