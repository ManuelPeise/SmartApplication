using Data.AppContext;
using Data.Identity;
using Logic.Administration;
using Logic.Identity;
using Logic.Identity.Interfaces;
using Logic.Interfaces;
using Logic.Shared;
using Logic.Shared.Clients;
using Logic.Shared.Interfaces;
using Logic.Shared.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Identity;
using System.Text;

namespace Web.Core.Startup
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder, string corsPolicy, string? identityContextConnectionString, string? applicationContextConnectionString)
        {
            builder.Services.AddDbContext<IdentityDbContext>(opt =>
            {
                if (identityContextConnectionString == null)
                {
                    throw new ArgumentNullException(nameof(identityContextConnectionString));
                }

                opt.UseMySQL(identityContextConnectionString);
            });

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            {
                if (applicationContextConnectionString == null)
                {
                    throw new ArgumentNullException(nameof(applicationContextConnectionString));
                }

                opt.UseMySQL(applicationContextConnectionString);
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddScoped<ILogMessageService, LogMessageService>();
            builder.Services.AddScoped<IAdministrationUnitOfWork, AdministrationUnitOfWork>();
            builder.Services.AddScoped<IEmailClient, EmailClient>();
            builder.Services.AddScoped<IEmailAccountCleanupModule, EmailAccountCleanupModule>();
            ConfigureOptions(builder);

            ConfigureJwt(builder);

            // Add services to the container.
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(corsPolicy, x =>
                {
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                    x.AllowAnyOrigin();
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        private static void ConfigureJwt(WebApplicationBuilder builder)
        {
            var (issuer, key) = GetJwtDataFromConfig(builder);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
        }

        private static (string? jwtIssuer, string? jwtKey) GetJwtDataFromConfig(WebApplicationBuilder builder)
        {
            var jwtIssuer = builder.Configuration.GetSection("JwtSettings:issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("JwtSettings:key").Get<string>();

            return (jwtIssuer, jwtKey);
        }

        private static void ConfigureOptions(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtData>(builder.Configuration.GetSection("JwtSettings"));
        }
    }
}
