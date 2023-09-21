using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using FluentValidation;
using Identity.Application;
using Identity.Domain.Entities;
using Identity.Persistence;
using Identity.WebApi.InMemory;
using Identity.WebApi.Mapping;
using Identity.WebApi.Models;
using Identity.WebApi.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        services.AddValidatorsFromAssemblyContaining<SignUpModelValidator>();

        services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
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
        /*.AddConfigurationStore(csOptions =>
        {
            csOptions.ConfigureDbContext = csBuilder => csBuilder.UseSqlServer
            (
                configuration.GetConnectionString("ISConfigurational"),
                sqlServerOptions => sqlServerOptions.MigrationsAssembly(assemblyName: typeof(IdentityServerContext).Assembly.FullName)
            );
        })*/
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
            //options.LoginPath =
            //options.LogoutPath =
        });

        services.AddControllers();

        return services;
    }
}
