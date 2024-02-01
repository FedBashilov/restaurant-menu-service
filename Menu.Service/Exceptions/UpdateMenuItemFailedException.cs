// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Exceptions
{
    public class UpdateMenuItemFailedException : Exception
    {
        public UpdateMenuItemFailedException()
        {
        }

        public UpdateMenuItemFailedException(string message)
            : base(message)
        {
        }

        public UpdateMenuItemFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
