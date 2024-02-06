// Copyright (c) Fedor Bashilov. All rights reserved.

namespace CloudStorage.Service.Extentions
{
    using CloudinaryDotNet.Actions;

    public static class StringExtensions
    {
        public static ResourceType ToResourceType(this string value)
        {
            return value switch
            {
                "image" => ResourceType.Image,
                "video" => ResourceType.Video,
                "raw" => ResourceType.Raw,
                "auto" => ResourceType.Auto,
                _ => throw new ArgumentException("Invalid resource type value"),
            };
        }
    }
}
