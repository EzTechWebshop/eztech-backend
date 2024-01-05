using AutoMapper;
using EzTech.Api.Authentication;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.UserApiModels;
using EzTech.Data.ApiModels.UserApiModels.AuthApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseUserController
{
    private readonly JwtHelper _jwtHelper;

    [HttpGet]
    [Route("validate-user")]
    public async Task<IActionResult> ValidateToken()
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        return Ok("Valid token");
    }

    [HttpPost]
    [Route("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (user.Hash != PasswordHelper.HashPassword(request.OldPassword, user.Salt))
        {
            return BadRequest("Incorrect password");
        }

        var salt = PasswordHelper.GenerateSalt();
        user.Salt = salt;
        user.Hash = PasswordHelper.HashPassword(request.NewPassword, salt);
        await DbContext.SaveChangesAsync();
        return Ok("Successfully updated password");
    }

    [HttpPost]
    [Route("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            return BadRequest("Email is not registered");
        }

        if (!user.IsEmailVerified)
        {
            return BadRequest("Email is not verified");
        }

        var hashedPassword = PasswordHelper.HashPassword(request.Password, user.Salt);
        if (hashedPassword != user.Hash)
        {
            return BadRequest("Incorrect password");
        }

        var token = _jwtHelper.GenerateToken(user);
        var response = new SignInResponse
        {
            AccessToken = token,
        };
        return Ok(response);
    }

    [HttpPost]
    [Route("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user != null)
        {
            return BadRequest("Email is already registered");
        }

        var newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            PostalCode = request.PostalCode,
            Salt = PasswordHelper.GenerateSalt(),
        };
        newUser.Hash = PasswordHelper.HashPassword(request.Password, newUser.Salt);
        newUser.VerificationToken = Guid.NewGuid().ToString();
        await DbContext.Users.AddAsync(newUser);
        await DbContext.SaveChangesAsync();
        return Ok("Successfully signed up");
    }

    [HttpGet]
    [Route("verify-email/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .FirstOrDefaultAsync(u => u.VerificationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid token");
        }

        if (user.IsEmailVerified)
        {
            return BadRequest("Email already verified");
        }

        user.IsEmailVerified = true;
        await DbContext.SaveChangesAsync();
        return Ok("Successfully verified email");
    }

    public AuthController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager, JwtHelper jwtHelper) :
        base(mapper, dbContext, emailManager)
    {
        _jwtHelper = jwtHelper;
    }
}