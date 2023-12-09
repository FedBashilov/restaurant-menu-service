// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Messaging.Service
{
    public interface INewsMessagingService
    {
        public void SendMessage<T>(T message);
    }
}