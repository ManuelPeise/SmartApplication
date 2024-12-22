using Microsoft.EntityFrameworkCore;
using Web.Core.Startup;

var identityContext = "IdentityContext";
var applicationContext = "ApplicationContext";

var corsPolicy = "policy";


var builder = WebApplication.CreateBuilder(args);

var identityContextConnectionString = builder.Configuration.GetConnectionString(identityContext) ?? null;
var applicationContextConnectionString = builder.Configuration.GetConnectionString(applicationContext) ?? null;

ServiceConfiguration.ConfigureServices(builder, corsPolicy, identityContextConnectionString, applicationContextConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
AppConfiguration.ConfigureApp(app, corsPolicy);

var baseUrl = builder.Configuration.GetRequiredSection("BaseUrl").Value ?? null;

if(baseUrl != null)
{
    Scheduler.ExecuteScheduler(baseUrl);
}

app.Run();

