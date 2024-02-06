// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Exceptions
{
    public class RemoveFileFailedException : Exception
    {
        public RemoveFileFailedException()
        {
        }

        public RemoveFileFailedException(string message)
            : base(message)
        {
        }

        public RemoveFileFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
