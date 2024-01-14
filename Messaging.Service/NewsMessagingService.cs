// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using System.Text;
    using System.Text.Json;

    public class NewsMessagingService : INewsMessagingService
    {
        private readonly IModel channel;
        private readonly RabbitMqSettings rbMqSettings;

        public NewsMessagingService(IOptions<RabbitMqSettings> rbMqSettings) 
        {
            this.rbMqSettings = rbMqSettings.Value;

            var factory = new ConnectionFactory { 
                HostName = this.rbMqSettings.HostName,
                UserName = this.rbMqSettings.UserName,
                Password = this.rbMqSettings.UserPassword,
                Port = this.rbMqSettings.HostPort,
            };
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

        public void SendMessage<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            this.channel.BasicPublish(this.rbMqSettings.ExchangeName, "", null, body);
        }
    }
}