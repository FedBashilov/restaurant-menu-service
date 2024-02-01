// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Exceptions
{
    public class CreateMenuItemFailedException : Exception
    {
        public CreateMenuItemFailedException()
        {
        }

        public CreateMenuItemFailedException(string message)
            : base(message)
        {
        }

        public CreateMenuItemFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
