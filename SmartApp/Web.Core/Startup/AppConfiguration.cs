using Data.Databases;
using Microsoft.EntityFrameworkCore;

namespace Web.Core.Startup
{
    public static class AppConfiguration
    {
        public static void ConfigureApp(WebApplication app, string corsPolicy)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(corsPolicy);

            app.UseAuthorization();
            app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                Thread.CurrentPrincipal = context.User;
                await next(context);
            });

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var pendingMigrations = applicationContext.Database.GetPendingMigrations();

                if (pendingMigrations.Any())
                {
                    applicationContext.Database.Migrate();
                }

                var userIdentityContext = scope.ServiceProvider.GetRequiredService<UserIdentityContext>();

                var pendingUserIdentityMigrations = userIdentityContext.Database.GetPendingMigrations();

                if (pendingUserIdentityMigrations.Any())
                {
                    userIdentityContext.Database.Migrate();
                }
            }

        }
    }
}
