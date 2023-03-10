// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    public class MenuItemDto
    {
        public string? Name { get; set; }

        public int Price { get; set; }

        public bool Visible { get; set; } = true;
    }
}
