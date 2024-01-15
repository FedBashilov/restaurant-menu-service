// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record MenuItemDTO
    {
        [Required(ErrorMessage = "The Name param is required")]
        public string? Name { get; init; }

        public string? Description { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "The Price param must be positive")]
        public int Price { get; init; }

        public bool Visible { get; init; } = true;

        public byte[]? Image { get; init; }
    }
}
