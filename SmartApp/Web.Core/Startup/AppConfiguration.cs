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

            app.Use(async (context, next) =>
            {
                Thread.CurrentPrincipal = context.User;
                await next(context);
            });

            app.MapControllers();

        }
    }
}
