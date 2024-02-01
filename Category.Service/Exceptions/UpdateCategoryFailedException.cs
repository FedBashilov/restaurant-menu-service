// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service.Exceptions
{
    public class UpdateCategoryFailedException : Exception
    {
        public UpdateCategoryFailedException()
        {
        }

        public UpdateCategoryFailedException(string message)
            : base(message)
        {
        }

        public UpdateCategoryFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
