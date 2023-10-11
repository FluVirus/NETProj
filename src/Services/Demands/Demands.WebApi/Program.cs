using Demands.Application;
using Demands.Persistence.Interfaces;
using Demands.WebApi.Hubs;
using MongoDB.Bson;

namespace Demands.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = builder.Configuration;

        builder.Services
            .AddWebApiServices(configuration)
            .AddApplicationServices(configuration);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<DemandsHub>("/customers");

        app.MapControllers();

        await app.RunAsync();
    }
}