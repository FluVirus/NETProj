using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Refresh.Commands;

public class RefreshSignInCommand: IRequest<Unit>
{
    public int UserId { get; init; }
}
