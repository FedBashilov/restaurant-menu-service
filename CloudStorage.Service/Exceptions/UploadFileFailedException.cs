// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Exceptions
{
    public class UploadFileFailedException : Exception
    {
        public UploadFileFailedException()
        {
        }

        public UploadFileFailedException(string message)
            : base(message)
        {
        }

        public UploadFileFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
