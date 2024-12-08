using Data.Identity;
using Logic.Identity;
using Logic.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Identity;
using System.Text;

namespace Web.Core.Startup
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder, string corsPolicy, string? identityConnectionString)
        {
            builder.Services.AddDbContext<IdentityDbContext>(opt =>
            {
                if (identityConnectionString == null)
                {
                    throw new ArgumentNullException(nameof(identityConnectionString));
                }

                opt.UseMySQL(identityConnectionString);
            });

            builder.Services.AddHttpContextAccessor();

            //  services.AddScoped<IConfigurationResolver, ConfigurationResolver>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();

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
            var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

            return (jwtIssuer, jwtKey);
        }

        private static void ConfigureOptions(WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtData>(builder.Configuration.GetSection("JwtSettings"));
        }
    }
}
