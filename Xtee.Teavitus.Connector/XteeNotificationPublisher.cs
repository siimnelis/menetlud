using System.Text;
using System.Text.Json;
using Menetlus.External.Contracts;
using Menetlus.External.Contracts.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xtee.Teavitus.Connector.Extensions;
using Xtee.Teavitus.Connector.TeavitusTeenus;
using Xtee.Teavitus.Connector.Configuration;

namespace Xtee.Teavitus.Connector;

public class XteeNotificationPublisher : IHostedService
{
    private IHostApplicationLifetime HostApplicationLifetime { get; }
    private ILogger<XteeNotificationPublisher> Logger { get; }
    private RabbitMqConfiguration RabbitMqConfiguration { get; }
    private ITeavitusTeenus TeavitusTeenus { get; }

    public XteeNotificationPublisher(IHostApplicationLifetime hostApplicationLifetime, 
        ILogger<XteeNotificationPublisher> logger, 
        IOptions<RabbitMqConfiguration> rabbitMqConfigurationOptions,
        ITeavitusTeenus teavitusTeenus
        )
    {
        HostApplicationLifetime = hostApplicationLifetime;
        Logger = logger;
        RabbitMqConfiguration = rabbitMqConfigurationOptions.Value;
        TeavitusTeenus = teavitusTeenus;
    }

    private IConnection? Connection { get; set; }
    private IModel? Channel { get; set; }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = RabbitMqConfiguration.HostName
        };

        Connection = factory.CreateConnection();
        Channel = Connection.CreateModel();

        var exchangeName = "notifications";
        var deadLetterExchangeName = "notifications-dead-letter";
        
        Channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
        Channel.ExchangeDeclare(exchange: deadLetterExchangeName, type: ExchangeType.Direct, durable: true);

        var queueName = "xtee-connector";
        var deadLetterQueueName = queueName + "-dead-letter";

        Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                {
                    "x-dead-letter-exchange", deadLetterExchangeName
                }
            });
        
        Channel.QueueDeclare(deadLetterQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: nameof(MenetlusLoodudEvent));
        Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: nameof(VoetiUlevaatamiseleEvent));
        Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: nameof(VoetiMenetlusseEvent));
        Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: nameof(MenetlusLoppesEvent));
        
        Channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: nameof(MenetlusLoodudEvent));
        Channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: nameof(VoetiUlevaatamiseleEvent));
        Channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: nameof(VoetiMenetlusseEvent));
        Channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: nameof(MenetlusLoppesEvent));
        Channel.QueueBind(queue: deadLetterQueueName, exchange: deadLetterExchangeName, routingKey: queueName);

        var consumer = new EventingBasicConsumer(Channel);
        
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var envelope = JsonSerializer.Deserialize<Envelope>(message)!;

            try
            {
                var teavitaRequest = new TeavitaRequest
                {
                    Teavita = new Teavita
                    {
                        Teavitus = envelope.Map(ea.BasicProperties.MessageId)
                    },
                    service = new XRoadServiceIdentifierType
                    {
                        serviceCode = "TeavitusTeenus",
                        xRoadInstance = "ee",
                        memberClass = "7000500",
                        subsystemCode = "teavitused",
                        serviceVersion = "v1"
                    },
                    client = new XRoadClientIdentifierType
                    {
                        xRoadInstance = "ee",
                        memberClass = "public",
                        memberCode = "7000600",
                        subsystemCode = "menetlus"
                    },
                    id = ea.BasicProperties.MessageId,
                    protocolVersion = "4.0",
                    userId = envelope.GetUserId()
                };
                
                await Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (_, ts, retryCount, _) =>
                    {
                        Logger.LogWarning($"Sending failed, retring {retryCount} time. Waiting for {ts.Seconds} seconds.");
                    }).ExecuteAsync(async () =>
                    {
                        await TeavitusTeenus.TeavitaAsync(teavitaRequest);
                    });
                
                Logger.LogInformation($"Succesfully sent notification: {message}");
                Channel.BasicAck(deliveryTag: ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Channel.BasicNack(deliveryTag: ea.DeliveryTag, false, false);
                Logger.LogError(ex, "Calling TeavitusTeenus failed. Sent notification to dead letter queue.");
            }

            
        };

        Channel.BasicQos(0, 1, false);
        Channel.BasicConsume(queue: queueName,
            autoAck: false,
            consumer: consumer);
        
        Logger.LogInformation("Waiting for messages.");
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Connection?.Dispose();
        Channel?.Dispose();
        Logger.LogInformation("Stopped receiving messages.");
        
        return Task.CompletedTask;
    }

}