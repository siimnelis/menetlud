using System.Net;
using System.Reflection;
using Menetlus.API.Authentication;
using Menetlus.API.Configuration;
using Menetlus.API.Extensions;
using Menetlus.API.Middleware;
using Menetlus.Domain;
using Menetlus.IdGenerator.Sequence.Postgre;
using Menetlus.Repository.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env}.json", optional:true)
    .Build();

var kestrelConfiguration = builder.Configuration.GetSection("KestrelConfiguration").Get<KestrelConfiguration>()!;

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Parse(kestrelConfiguration.Ip), kestrelConfiguration.Port);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetSection("DatabaseConnectionString").Get<string>()!;

builder.Services.AddScoped<IMenetlusService, MenetlusService>();
builder.Services.AddScoped<IMenetlusRepository, MenetlusRepository>();
builder.Services.AddSingleton<IMenetlusIdGenerator>(_ => new MenetlusIdGenerator(connectionString));
builder.Services.AddScoped(serviceProvider => new MenetlusContext(connectionString, serviceProvider.GetService<MenetlejaContext>()));

builder.Services.AddMenetlejaContext();
builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", (o) => {});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "Menetluse loomine ja lugemine tehakse anonüümse kasutajaga. " +
                      "Menetluse staatuste muutmiseks peab enneast autentima HTTP Basic Authenticationiga." +
                      "Kasutajanimi läheb menetleja isikukoodiks ja parool asutuse tunnuseks." +
                      "Menetleja andmete muutmiseks tuleb kõik browseri aknad sulgeda või avada uus aken inkognito aken."
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<UnitOfWorkMiddleware>();

var rewriteOptions = new RewriteOptions();
rewriteOptions.AddRedirect("^$", "swagger");
app.UseRewriter(rewriteOptions);

var context = app.Services.GetService<MenetlusContext>()!;

await context.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();