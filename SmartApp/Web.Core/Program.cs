using Microsoft.EntityFrameworkCore;
using Web.Core.Startup;

var identityContext = "IdentityContext";
var corsPolicy = "policy";

var builder = WebApplication.CreateBuilder(args);

var identityConnectionString = builder.Configuration.GetConnectionString(identityContext) ?? null;

ServiceConfiguration.ConfigureServices(builder, corsPolicy, identityConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
AppConfiguration.ConfigureApp(app, corsPolicy);

app.Run();
