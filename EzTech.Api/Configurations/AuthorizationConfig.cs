using EzTech.Api.Authentication.Policies;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EzTech.Api.Configurations;

public static class AuthorizationConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new IsAdminRequirement(UserRoles.Admin));
            });
            options.AddPolicy("IsUser", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new IsUserRequirement(UserRoles.User));
            });
        });
    }
}