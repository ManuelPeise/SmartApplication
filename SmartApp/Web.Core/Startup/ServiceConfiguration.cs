using Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.Core.Startup
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, string corsPolicy, string? identityConnectionString)
        {
            services.AddDbContext<IdentityDbContext>(opt =>
            {
                if (identityConnectionString == null)
                {
                    throw new ArgumentNullException(nameof(identityConnectionString));
                }

                opt.UseMySQL(identityConnectionString);
            });

            // Add services to the container.
            services.AddCors(opt =>
            {
                opt.AddPolicy(corsPolicy, x =>
                {
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                    x.AllowAnyOrigin();
                });
            });

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
