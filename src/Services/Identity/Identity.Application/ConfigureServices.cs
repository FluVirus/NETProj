using Identity.Application.Registration.Commands;
using Identity.Domain.Entities;
using Identity.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<RegisterNewUserHandler>();
        });
        return services;
    }
}
