using System.Net;
using TeavitusTeenus.Host;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Parse("0.0.0.0"), 5273);
    options.AllowSynchronousIO = true;
});

var app = builder.Build();
app.UseMiddleware<XRoadMiddleware>();

app.Run();