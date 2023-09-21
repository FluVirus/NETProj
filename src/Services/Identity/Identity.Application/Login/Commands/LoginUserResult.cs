using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Login.Commands;

public class LoginUserResult
{
    public required bool Succeeded { get; init; }
    
    public required bool RequiresTwoFactor { get; init; }

    public required bool IsNotAllowed { get; init; }

    public required bool IsLockedOut { get; init; }
}
