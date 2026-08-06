namespace API.Presentation.Extensions;

using API.Application.Authentication.UseCases;
using API.Application.Authentication.Ports;
using API.Application.Account.UseCases;
using API.Application.Members.UseCases;
using API.Infrastructure.Repositories;
using System.Text.Json.Serialization;
using API.Application.Account.Ports;
using Microsoft.EntityFrameworkCore;
using API.Application.Common.Ports;
using API.Infrastructure.Services;
using API.Infrastructure.Data;
using API.Infrastructure;
using Asp.Versioning;
using Minio;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddHttpContextAccessor();

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddSingleton<IFileStorage>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var endpoint = configuration["MinIO:Endpoint"]
                ?? throw new InvalidOperationException("MinIO Endpoint is missing");

            var accessKey = configuration["MinIO:AccessKey"]
                ?? throw new InvalidOperationException("MinIO AccessKey is missing");

            var secretKey = configuration["MinIO:SecretKey"]
                ?? throw new InvalidOperationException("MinIO SecretKey is missing");

            var bucketName = configuration["MinIO:BucketName"]
                ?? throw new InvalidOperationException("MinIO BucketName is missing");

            var client = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();

            return new MinioFileStorage(client, bucketName);
        });

        services.AddScoped<GetMemberPhotosUseCase>();
        services.AddScoped<GetCurrentUserUseCase>();
        services.AddScoped<UpdatePasswordUseCase>();
        services.AddScoped<UpdateAccountUseCase>();
        services.AddScoped<RefreshTokenUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<GetMembersUseCase>();
        services.AddScoped<LoginUserUseCase>();
        services.AddScoped<GetMemberUseCase>();
        services.AddScoped<UploadPhotoUseCase>();
        services.AddScoped<DeletePhotoUseCase>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}