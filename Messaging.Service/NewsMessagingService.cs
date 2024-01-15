// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using System.Text;
    using System.Text.Json;

    public class NewsMessagingService : INewsMessagingService
    {
        private readonly IModel? channel;
        private readonly RabbitMqSettings rbMqSettings;
        private readonly ILogger<NewsMessagingService> logger;

        public NewsMessagingService(
            IOptions<RabbitMqSettings> rbMqSettings, 
            ILogger<NewsMessagingService> logger)
        {
            this.rbMqSettings = rbMqSettings.Value;
            this.logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = this.rbMqSettings.HostName,
                UserName = this.rbMqSettings.UserName,
                Password = this.rbMqSettings.UserPassword,
                Port = this.rbMqSettings.HostPort,
            };

            try
            {
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.ExchangeDeclare(this.rbMqSettings.ExchangeName, ExchangeType.Fanout);
                channel.QueueDeclare(
                    this.rbMqSettings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);
                channel.QueueBind(this.rbMqSettings.QueueName, this.rbMqSettings.ExchangeName, "");
                this.channel = channel;
            }
            catch
            {
                this.logger.LogError("RabbitMQ initialization failed!");
            }
        }

        public void SendMessage<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            try
            {
                this.channel.BasicPublish(this.rbMqSettings.ExchangeName, "", null, body);
                this.logger.LogInformation($"RabbitMQ message published! Exchange={this.rbMqSettings.ExchangeName}, Message={body}");
            }
            catch
            {
                this.logger.LogError($"RabbitMQ message publishing failed! Exchange={this.rbMqSettings.ExchangeName}, Message={body}");
            }
}
    }
}