// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Responses
{
    public record ErrorResponse
    {
        public ErrorResponse(string message)
        {
            this.Message = message;
        }

        public string? Message { get; init; }
    }
}
