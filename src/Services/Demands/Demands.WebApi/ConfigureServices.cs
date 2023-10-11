using Demands.WebApi.Hubs;
using Demands.WebApi.Hubs.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;

namespace Demands.WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Authority:URL"];
            options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        });

        services.AddStackExchangeRedisCache(options => {
            options.Configuration = configuration["Redis:Configuration"];
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddAuthorization();

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        })
        .AddHubOptions<DemandsHub>(options =>
        {
            options.AddFilter<DemandsExceptionFilter>();
        });

        return services;
    }
}
