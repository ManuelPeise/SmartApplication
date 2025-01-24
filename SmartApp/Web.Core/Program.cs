using Microsoft.EntityFrameworkCore;
using Shared.Models.Identity;
using Web.Core.Startup;

var identityContext = "IdentityContext";
var applicationContext = "ApplicationContext";
var aiContext = "AiContext";

var corsPolicy = "policy";


var builder = WebApplication.CreateBuilder(args);

var identityContextConnectionString = builder.Configuration.GetConnectionString(identityContext) ?? null;
var applicationContextConnectionString = builder.Configuration.GetConnectionString(applicationContext) ?? null;
var aiContextConnectionString = builder.Configuration.GetConnectionString(aiContext) ?? null;

ServiceConfiguration.ConfigureServices(builder, corsPolicy, identityContextConnectionString, applicationContextConnectionString, aiContextConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
AppConfiguration.ConfigureApp(app, corsPolicy);

var baseUrl = builder.Configuration.GetRequiredSection("BaseUrl").Value ?? null;

if(baseUrl != null)
{
    var jwtIssuer = builder.Configuration.GetSection("Security:issuer").Get<string>();
    var jwtKey = builder.Configuration.GetSection("Security:key").Get<string>();

    Scheduler.ExecuteScheduler(baseUrl, new SecurityData { Key = jwtKey, Issuer = jwtIssuer});
}

app.Run();

