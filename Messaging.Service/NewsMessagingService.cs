// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    using RabbitMQ.Client;
    using System.Text;
    using System.Text.Json;

    public class NewsMessagingService : INewsMessagingService
    {
        public IModel channel;

        public NewsMessagingService() {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("menu_to_news_exchange", ExchangeType.Fanout);
            channel.QueueDeclare(
                "menu_to_news_queue",
                durable: true,
                exclusive: false,
                autoDelete: false);
            channel.QueueBind("menu_to_news_queue", "menu_to_news_exchange", "");
            this.channel = channel;
        }
        public void SendMessage<T>(T message)
        {
            //отправить в очередь
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            this.channel.BasicPublish("menu_to_news_exchange", "", null, body);
        }
    }
}