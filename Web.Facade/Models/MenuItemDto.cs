// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MenuItemDto
    {
        [Required(ErrorMessage = "The Name param is required")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The Price param must be positive")]
        public int Price { get; set; }

        public bool Visible { get; set; } = true;
    }
}
