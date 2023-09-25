using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Login.Commands;

public class LoginUserHandler: IRequestHandler<LoginUserCommand, Unit>
{
    private readonly UserManager<User>  _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginUserHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new EmailNotFoundException("Email not found", request.Email);
        }

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        return Unit.Value;
    }
}
