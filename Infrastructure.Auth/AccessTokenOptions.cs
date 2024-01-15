// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Auth
{
    public record AccessTokenOptions
    {
        public string? SecretKey { get; init; }

        public string? Issuer { get; init; }

        public string? Audience { get; init; }
    }
}
