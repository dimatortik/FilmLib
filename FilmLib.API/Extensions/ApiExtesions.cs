using System.Text;
using Amazon.S3;
using FilmLib.Application.Auth;
using FilmLib.Application.Interfaces;
using FilmLib.Domain.Enums;
using FilmLib.Infrastructure.Auth;
using FilmLib.Infrastructure.CloudStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace FilmLib.API.Extensions;

public static class ApiExtensions
{

    public static void AddCustomAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings!.Issuer,
                    ValidAudience = settings!.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings!.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["cosy"];
                        return Task.CompletedTask;
                    } 
                };
            });
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<AuthService>();
        services.AddScoped<IPermissionsService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

                policy.Requirements.Add(new PermissionRequirement([Permission.Create]));
                policy.Requirements.Add(new PermissionRequirement([Permission.Read]));
                policy.Requirements.Add(new PermissionRequirement([Permission.Delete]));
                policy.Requirements.Add(new PermissionRequirement([Permission.Update]));
                

            });
            options.AddPolicy("UserPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

                policy.Requirements.Add(new PermissionRequirement([Permission.Read]));
            });

        });
        
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<AuthorizationOptions>(configuration.GetSection("AuthorizationOptions"));
        
    }

    public static void AddCloudStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var accessKey = configuration["AWS:AccessKey"];
        var secretKey = configuration["AWS:SecretKey"];
        var serviceUrl = configuration["AWS:ServiceURL"];
        
        services.AddScoped<IAmazonS3>(sp => new AmazonS3Client(accessKey, secretKey, new AmazonS3Config
        {
            ServiceURL = serviceUrl
        }));
        
        services.AddScoped<ICloudStorageService, CloudStorageService>();
    }
    
}