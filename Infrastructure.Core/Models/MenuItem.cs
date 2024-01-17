// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public bool Visible { get; set; } = true;

        public string? ImageUrl { get; set; }

        public IEnumerable<MenuItemCategory>? MenuItemCategories { get; set; }
    }
}
