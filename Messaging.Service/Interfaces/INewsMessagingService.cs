// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service.Interfaces
{
    public interface INewsMessagingService
    {
        public void SendMessage<T>(T message);
    }
}