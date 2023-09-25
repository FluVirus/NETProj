using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Refresh.Commands;

public class RefreshSignInHandler : IRequestHandler<RefreshSignInCommand, Unit>
{
    private SignInManager<User> _signInManager;
    private UserManager<User> _userManager;

    public RefreshSignInHandler(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RefreshSignInCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is null)
        {
            throw new Exception("Internal server error: cannot find user to refresh");
        }

        await _signInManager.RefreshSignInAsync(user);

        return Unit.Value;
    }
}
