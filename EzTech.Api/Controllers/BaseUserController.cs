using AutoMapper;
using EzTech.Api.Authentication;
using EzTech.Api.Services;
using EzTech.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzTech.Api.Controllers;

[Authorize(Policy = "IsUser")]
public class BaseUserController : BaseController
{
    protected readonly IEmailManager EmailManager;

    // Taken from internship code
    // Gets the currently logged user from the token given in the header
    protected UserPrincipal UserPrincipal
    {
        get
        {
            var currentlyLoggedUser = JwtHelper.GetUser(this.User);
            return currentlyLoggedUser;
        }
    }

    public BaseUserController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper,
        dbContext)
    {
        EmailManager = emailManager;
    }
}