// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Exceptions
{
    public class UploadFileException : Exception
    {
        public UploadFileException()
        {
        }

        public UploadFileException(string message)
            : base(message)
        {
        }

        public UploadFileException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
