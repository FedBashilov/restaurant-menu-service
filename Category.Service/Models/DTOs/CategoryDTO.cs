// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Category.Service.Models.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public record CategoryDTO
    {
        [Required(ErrorMessage = "The Name param is required")]
        public string? Name { get; init; }

        public bool Visible { get; init; }
    }
}
