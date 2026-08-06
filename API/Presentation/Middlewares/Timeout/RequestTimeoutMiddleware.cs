using Microsoft.Extensions.Options;
using API.Application.Common;
using System.Text.Json;
using System.Net;

namespace API.Presentation.Middlewares.Timeout;

public sealed class RequestTimeoutMiddleware(RequestDelegate next, IOptions<RequestTimeoutOptions> options)
{
    private readonly RequestTimeoutOptions _options = options.Value;

    public async Task InvokeAsync(HttpContext context)
    {
        using var timeoutCancellationTokenSource = new CancellationTokenSource(_options.Timeout);
        using var linkedCancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted,
                timeoutCancellationTokenSource.Token);

        try
        {
            context.RequestAborted = linkedCancellationTokenSource.Token;
            await next(context);
        }
        catch (OperationCanceledException) when (timeoutCancellationTokenSource.IsCancellationRequested)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.Fail(new ApiError("The request timed out.", "request_timeout"));
            var json = JsonSerializer.Serialize(response,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json, CancellationToken.None);
        }
    }
}