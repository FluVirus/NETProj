using AutoMapper;
using Identity.Application.Login.Commands;
using Identity.Application.Registration.Commands;
using Identity.Domain.Entities;
using Identity.WebApi.InMemory;
using Identity.WebApi.Models;
using System.Buffers;

namespace Identity.WebApi.Mapping;

internal class AccountMappings: Profile
{
    public AccountMappings() 
    {
        CreateMap<SignUpModel, RegisterNewUserCommand>()
            .ForMember(command => command.UserName, options => options.MapFrom(model => model.UserName))
            .ForMember(command => command.Email, options => options.MapFrom(model => model.Email))
            .ForMember(command => command.Password, options => options.MapFrom(model => model.Password))
            .ForMember(command => command.Roles, options => options.MapFrom(model => InMemoryResources.SingleCustomer));

        CreateMap<SignInModel, LoginUserCommand>()
            .ForMember(command => command.Email, options => options.MapFrom(model => model.Email))
            .ForMember(command => command.Password, options => options.MapFrom(model => model.Password));
    }
}
