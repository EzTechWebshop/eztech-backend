using EzTech.Api.Authentication;
using EzTech.Api.Authentication.Policies;
using EzTech.Api.Services;
using EzTech.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzTech.Api.Configurations;

public static class ServicesConfig
{
    public static void Configure(IServiceCollection services)
    {
        // Authorization handlers
        AuthorizationHandlers(services);
        // Services
        ServiceClasses(services);
        services.AddSingleton<JwtHelper>();
        
        // Auto Mapper Configurations
        services.AddAutoMapper(typeof(MappingProfile));
        
    }

    private static void ServiceClasses(IServiceCollection services)
    {
        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IEmailManager, EmailService>();
    }

    private static void AuthorizationHandlers(IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, IsUserHandler>();
        services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
    }
}