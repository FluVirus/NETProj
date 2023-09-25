using AutoMapper;
using Identity.Application.Login.Commands;
using Identity.Application.Refresh.Commands;
using Identity.Application.Registration.Commands;
using Identity.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountsController: ControllerBase<AccountsController>
{
    public AccountsController
    (
        ILogger<AccountsController> logger,
        IMediator mediator,
        IMapper mapper
    ):
    base(logger, mediator, mapper)  { }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpViewModel signUpViewModel)
    {       
        RegisterNewUserCommand command = _mapper.Map<RegisterNewUserCommand>(signUpViewModel);

        await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromForm] SignInViewModel signInViewModel)
    {
        LoginUserCommand command = _mapper.Map<LoginUserCommand>(signInViewModel);

        await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Refresh([FromForm] RefreshViewModel refreshViewModel)
    {
        RefreshSignInCommand command = _mapper.Map<RefreshSignInCommand>(refreshViewModel);

        await _mediator.Send(command, HttpContext.RequestAborted);

        return Ok();
    }

    [HttpPost]
    public Task<IActionResult> SignOut()
    {
        //TODO: Handle SignOut need redirect. Where to redirect?
        throw new NotImplementedException();
    }
}
