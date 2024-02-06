// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Exceptions
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException()
        {
        }

        public InvalidCategoryException(string message)
            : base(message)
        {
        }

        public InvalidCategoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
