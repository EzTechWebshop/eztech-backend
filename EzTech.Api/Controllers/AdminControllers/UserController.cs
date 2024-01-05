using AutoMapper;
using EzTech.Api.Authentication;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.UserApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class UserController : BaseAdminController
{
    private readonly IEmailManager _emailManager;
    
    // Not implemented yet, but is useful to add in the future
    [HttpPatch]
    [Route("/{userId:int}/change-users-password")]
    public async Task<IActionResult> ChangeUsersPassword(int userId, [FromBody] ChangeUsersPasswordRequest request)
    {
        var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        user.Salt = PasswordHelper.GenerateSalt();
        user.Hash = PasswordHelper.HashPassword(request.NewPassword, user.Salt);
        await DbContext.SaveChangesAsync();
        var emailSent = await _emailManager.SendEmail(user.Email, "Password changed",
            "Your password has been changed by an admin");
        if (emailSent)
        {
            return Ok("Successfully updated password and sent email");
        }
        return Ok("Successfully updated password, but failed to send email");
        
    }
    public UserController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper, dbContext)
    {
        _emailManager = emailManager;
    }
}