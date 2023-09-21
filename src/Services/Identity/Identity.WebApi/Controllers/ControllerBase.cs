using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;

namespace Identity.WebApi.Controllers;

[ApiController]
public class ControllerBase<T> : ControllerBase
{
    protected ILogger<T> _logger;
    protected IMediator _mediator;
    protected IMapper _mapper;

    public ControllerBase(ILogger<T> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }
}
