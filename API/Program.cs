using System.Text.Json.Serialization;
using API.Data;
using API.Infrastructure.Data;
using API.Presentation.Middlewares.Exception;
using API.Presentation.Middlewares.Timeout;
using API.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddRequestTimeout(builder.Configuration);
builder.Services.AddScoped<ExceptionMiddleware>();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

app.UseRequestTimeout();
app.UseMiddleware<ExceptionMiddleware>();

app.MapIdentityEndpoints();

app.UseCors(request => request
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .WithOrigins("https://localhost:4000"));

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedMembers(context);
}
catch (Exception exception)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(exception, "An error occured during migration");
}

app.Run();