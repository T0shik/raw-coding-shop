using System;

namespace RawCoding.S3
{
    internal static class ContentTypeFactory
    {
        private const string Favicon = "image/x-icon";
        private const string Png = "image/png";
        private const string Jpg = "image/jpg";
        private const string Jpeg = "image/jpeg";
        private const string Mp4 = "video/mp4";

        internal static string ResolveContentType(string fileExtension) =>
            fileExtension.Replace(".", "").ToLower() switch
            {
                "ico" => Favicon,
                "png" => Png,
                "jpg" => Jpg,
                "jpeg" => Jpeg,
                "mp4" => Mp4,
                _ => throw new ArgumentException(nameof(fileExtension)),
            };

        internal static string ResolveContentType(ContentType contentType) =>
            contentType switch
            {
                ContentType.Favicon => Favicon,
                ContentType.Png => Png,
                ContentType.Jpg => Jpg,
                ContentType.Jpeg => Jpeg,
                ContentType.Mp4 => Mp4,
                _ => throw new ArgumentException(nameof(contentType)),
            };
    }
}