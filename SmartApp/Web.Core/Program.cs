using Microsoft.EntityFrameworkCore;
using Web.Core.Startup;

var identityContext = "IdentityContext";
var applicationContext = "ApplicationContext";

var corsPolicy = "policy";

try
{
    var builder = WebApplication.CreateBuilder(args);

    var identityContextConnectionString = builder.Configuration.GetConnectionString(identityContext) ?? null;
    var applicationContextConnectionString = builder.Configuration.GetConnectionString(applicationContext) ?? null;

    ServiceConfiguration.ConfigureServices(builder, corsPolicy, identityContextConnectionString, applicationContextConnectionString);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    AppConfiguration.ConfigureApp(app, corsPolicy);

    app.Run();

}catch(Exception ex)
{
    var error = ex.Message;
}
