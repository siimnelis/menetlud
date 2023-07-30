using System.ServiceModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xtee.Teavitus.Connector;
using Xtee.Teavitus.Connector.Configuration;
using Xtee.Teavitus.Connector.TeavitusTeenus;
using Xtee.Teavitus.Connector.XRoad;

var builder = Host.CreateApplicationBuilder(args);

var env = Environment.GetEnvironmentVariable("CONSOLE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env}.json", true)
    .Build();

builder.Services.AddHostedService<XteeNotificationPublisher>();
builder.Services.AddSingleton<ITeavitusTeenus>(serviceProvider =>
{
    var teavitusTeenusConfiguration= serviceProvider.GetService<IOptions<TeavitusTeenusConfiguration>>()!.Value;

    var channelFactory = new ChannelFactory<ITeavitusTeenus>(new BasicHttpBinding(),
        new EndpointAddress(teavitusTeenusConfiguration.Url));
    
    channelFactory.Endpoint.EndpointBehaviors.Add(new XRoadEndpointBehaviour());
    
    var channel = channelFactory.CreateChannel(new EndpointAddress(teavitusTeenusConfiguration.Url));

    return channel;

});

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection(
    key: nameof(RabbitMqConfiguration)));
builder.Services.Configure<TeavitusTeenusConfiguration>(builder.Configuration.GetSection(
    key: nameof(TeavitusTeenusConfiguration)));

var app = builder.Build();

await app.RunAsync();
    