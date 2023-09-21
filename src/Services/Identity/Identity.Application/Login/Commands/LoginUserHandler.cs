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

public class LoginUserHandler: IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly UserManager<User>  _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginUserHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new EmailNotFoundException("Email not found", request.Email);
        }

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);

        return new LoginUserResult
        {
            Succeeded = signInResult.Succeeded,
            IsNotAllowed = signInResult.IsNotAllowed,
            IsLockedOut = signInResult.IsLockedOut,
            RequiresTwoFactor = signInResult.RequiresTwoFactor
        };
    }
}
