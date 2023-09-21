using AutoMapper;
using Duende.IdentityServer.EntityFramework.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;

[ApiController]
[Obsolete]
[Route("test")]
public class PrototypeController: ControllerBase<PrototypeController>
{
    public PrototypeController(ILogger<PrototypeController> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
    {

    }

    [Route("ensurecreated")]
    [HttpPost]
    public IResult TestAction([FromServices] PersistedGrantDbContext pgDB, [FromServices] ConfigurationDbContext cfDB)
    {
        pgDB.Database.EnsureDeleted();
        pgDB.Database.EnsureCreated();

        cfDB.Database.EnsureDeleted();
        cfDB.Database.EnsureCreated();

        return Results.NotFound();
    }
}
