using Identity.Application.Extensions;
using Identity.Domain.Entities;
using Identity.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Registration.Commands;

internal class RegisterNewUserHandler : IRequestHandler<RegisterNewUserCommand, Unit>
{
    private UserManager<User> _userManager;

    public RegisterNewUserHandler(UserManager<User> userManager) 
    {
        _userManager = userManager;
    }
    
    public async Task<Unit> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
    {   
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
        };

        IdentityResult creationResult = await _userManager.CreateAsync(user, request.Password);
        creationResult.ThrowIfErrors();

        IdentityResult updateSecurityStampResult = await _userManager.UpdateSecurityStampAsync(user);
        updateSecurityStampResult.ThrowIfErrors();

        IdentityResult roleResult = await _userManager.AddToRolesAsync(user, request.Roles.Select(role => role.Name!));
        roleResult.ThrowIfErrors();

        if (!(creationResult.Succeeded && roleResult.Succeeded && updateSecurityStampResult.Succeeded))
        {
            throw new Exception("Internal Server Error");
        }

        return Unit.Value;
    }
}
