using AutoMapper;
using EzTech.Data;
using Microsoft.AspNetCore.Mvc;

namespace EzTech.Api.Controllers;

/// <summary>
/// Used for all controllers
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