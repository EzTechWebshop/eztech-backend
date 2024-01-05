using AutoMapper;
using EzTech.Data;
using Microsoft.AspNetCore.Mvc;

namespace EzTech.Api.Controllers;

/// <summary>
/// Used for all controller
/// Right now unnecessary, but can be used for common methods in the future
/// </summary>
public class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;
    protected readonly EzTechDbContext DbContext;
    public BaseController(IMapper mapper, EzTechDbContext dbContext)
    {
        Mapper = mapper;
        DbContext = dbContext;
    }
}