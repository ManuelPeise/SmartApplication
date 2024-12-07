namespace Web.Core.Startup
{
    public static class AppConfiguration
    {
        public static void ConfigureApp(WebApplication app, string corsPolicy)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(corsPolicy);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

        }
    }
}
