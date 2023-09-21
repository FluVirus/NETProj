using Duende.IdentityServer.Services;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Application;

public class UserProfileService : ProfileService<User>
{
    public UserProfileService(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsFactory) : base(userManager, claimsFactory)
    {

    }

    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, User user)
    {
        ClaimsPrincipal principal = await GetUserClaimsAsync(user);
        ClaimsIdentity id = (ClaimsIdentity)principal.Identity!;

        context.AddRequestedClaims(principal.Claims);
    }

}
