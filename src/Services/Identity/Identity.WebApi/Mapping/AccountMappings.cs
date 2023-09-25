using AutoMapper;
using Identity.Application.Login.Commands;
using Identity.Application.Refresh.Commands;
using Identity.Application.Registration.Commands;
using Identity.WebApi.InMemory;
using Identity.WebApi.Models;

namespace Identity.WebApi.Mapping;

internal class AccountMappings: Profile
{
    public AccountMappings() 
    {
        CreateMap<SignUpViewModel, RegisterNewUserCommand>()
            .ForMember(command => command.UserName, options => options.MapFrom(model => model.UserName))
            .ForMember(command => command.Email, options => options.MapFrom(model => model.Email))
            .ForMember(command => command.Password, options => options.MapFrom(model => model.Password))
            .ForMember(command => command.Roles, options => options.MapFrom(model => InMemoryResources.SingleCustomer));

        CreateMap<SignInViewModel, LoginUserCommand>()
            .ForMember(command => command.Email, options => options.MapFrom(model => model.Email))
            .ForMember(command => command.Password, options => options.MapFrom(model => model.Password));

        CreateMap<RefreshViewModel, RefreshSignInCommand>()
            .ForMember(command => command.UserId, options => options.MapFrom(model => model.UserId));
    }
}
