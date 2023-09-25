using Identity.Application;
using Identity.Domain.Entities;
using Identity.Persistence;
using Identity.WebApi.InMemory;
using Identity.WebApi.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddAutoMapper(configuration => 
        {
            configuration.AddProfile<AccountMappings>();
        });

        services.AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityCookies();
        
        services.AddAuthorization();

        services
        .AddIdentityServer(isOptions =>
        {
            isOptions.Authentication.CookieAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            isOptions.Authentication.CookieSlidingExpiration = true;
            isOptions.Authentication.CookieLifetime = TimeSpan.FromMinutes(20);
        })
        .AddDefaultEndpoints()
        .AddJwtBearerClientAuthentication()
        .AddInMemoryClients(InMemoryConfiguration.Clients)
        .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
        .AddInMemoryApiResources(InMemoryConfiguration.Apis)
        .AddDeveloperSigningCredential()
        .AddOperationalStore(osOptions =>
        {
            osOptions.ConfigureDbContext = csBuilder => csBuilder.UseSqlServer
            (
                configuration.GetConnectionString("ISOperational"),
                sqlServerOptions => sqlServerOptions.MigrationsAssembly(assemblyName: typeof(IdentityServerContext).Assembly.FullName)
            );
        })
        .AddAspNetIdentity<User>()
        .AddProfileService<UserProfileService>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "IdentityCookie";
        });

        services.AddControllers();

        return services;
    }
}
