// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    public record RabbitMqSettings
    {
        public string? UserName { get; init; }

        public string? UserPassword { get; init; }

        public string? HostName { get; init; }

        public int HostPort { get; init; }   

        public string? ExchangeName { get; init; }

        public string? QueueName { get; init; }
    }
}
