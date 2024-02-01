// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Exceptions
{
    public class MenuItemNotFoundException : Exception
    {
        public MenuItemNotFoundException()
        {
        }

        public MenuItemNotFoundException(string message)
            : base(message)
        {
        }

        public MenuItemNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
