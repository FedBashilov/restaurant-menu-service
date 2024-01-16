// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class MenuItemCategory
    {
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
