// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    public record MenuItemResponse
    {
        public int Id { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }

        public int Price { get; init; }

        public bool Visible { get; init; } = true;

        public string? ImageUrl { get; set; }

        public IEnumerable<int>? Categories { get; init; }
    }
}
