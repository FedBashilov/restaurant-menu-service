// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service
{
    public record CloudinarySettings
    {
        public string? Cloud { get; init; }

        public string? ApiKey { get; init; }

        public string? ApiSecret { get; init; }
    }
}
