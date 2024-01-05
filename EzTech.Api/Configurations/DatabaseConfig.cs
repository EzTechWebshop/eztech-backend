using EzTech.Data;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Configurations;

public static class DatabaseConfig
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        // Database: MySql
        services.AddDbContext<EzTechDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 29))));
    }
}