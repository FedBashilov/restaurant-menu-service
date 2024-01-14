// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    public class RabbitMqSettings
    {
        public string? UserName { get; set; }

        public string? UserPassword { get; set; }

        public string? HostName { get; set; }

        public int HostPort { get; set; }

        public string? ExchangeName { get; set; }

        public string? QueueName { get; set; }
    }
}
