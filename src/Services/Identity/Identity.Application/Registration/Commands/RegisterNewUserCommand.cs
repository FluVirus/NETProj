using Identity.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Registration.Commands;

public class RegisterNewUserCommand: IRequest<Unit>
{
    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string UserName { get; init; }

    public required IEnumerable<Role> Roles { get; init; }
}
