namespace API.Presentation.Middlewares.Exception;

using API.Application.Common;
using System.Text.Json;
using System.Net;

public class ExceptionMiddleware(
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment env
) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception exception)
        {
            logger.LogError(exception, "{message}", exception.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = env.IsDevelopment() || env.IsStaging()
                ? ApiResponse<object>.Fail(new ApiError(exception.Message, "internal_error", exception.ToString()))
                : ApiResponse<object>.Fail(new ApiError("An unexpected error occurred", "internal_error"));

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}