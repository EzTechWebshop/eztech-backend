using AutoMapper;
using EzTech.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzTech.Api.Controllers;

[Authorize(Policy = "IsAdmin")]
public class BaseAdminController : BaseController
{
    public BaseAdminController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}