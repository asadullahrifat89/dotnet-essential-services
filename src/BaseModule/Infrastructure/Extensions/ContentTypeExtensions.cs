namespace BaseModule.Infrastructure.Extensions
{
    public static class ContentTypeExtensions
    {
        private static readonly Dictionary<string, string> ContentTypes = new Dictionary<string, string>
        {
            { ".txt", "text/plain" },
            { ".pdf", "application/pdf" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            // Add more mappings as needed
        };

        public static string GetContentType(string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentException("File extension cannot be null, empty, or whitespace.", nameof(fileExtension));

            string normalizedExtension = fileExtension.Trim().ToLowerInvariant();

            if (!normalizedExtension.StartsWith("."))
                normalizedExtension = "." + normalizedExtension;

            if (ContentTypes.TryGetValue(normalizedExtension, out var contentType))
                return contentType;

            return "application/octet-stream"; // Default content type for unknown extensions.
        }
    }
}
