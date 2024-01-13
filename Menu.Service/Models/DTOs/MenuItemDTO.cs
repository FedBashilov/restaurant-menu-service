// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Menu.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class MenuItemDTO
    {
        [Required(ErrorMessage = "The Name param is required")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Price param must be positive")]
        public int Price { get; set; }

        public bool Visible { get; set; } = true;

        public byte[]? Image { get; set; }
    }
}
