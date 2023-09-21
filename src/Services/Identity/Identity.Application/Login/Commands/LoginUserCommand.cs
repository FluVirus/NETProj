﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Login.Commands;

public class LoginUserCommand : IRequest<LoginUserResult>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}
