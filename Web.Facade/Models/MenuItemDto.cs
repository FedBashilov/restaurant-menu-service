// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class MenuItemDto
    {
        public string? Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
    }
}
