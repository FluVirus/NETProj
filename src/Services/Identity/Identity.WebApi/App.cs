using Duende.IdentityServer.EntityFramework.DbContexts;
using Identity.Application;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.WebApi;

public class App
{   
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.AddServerHeader = false; 
        });

        IConfiguration configuration = builder.Configuration;

        builder.Services
            .AddApplicationServices(configuration)
            .AddWebApiServices(configuration);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseIdentityServer();

        app.MapControllers();

        await app.RunAsync();
    }
}