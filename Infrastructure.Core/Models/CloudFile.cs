// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Core.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CloudFile
    {
        [Key]
        public int Id { get; set; }

        public string? Url { get; set; }

        public string? ResourceType { get; set; }

        public string? PublicId { get; set; }
    }
}
