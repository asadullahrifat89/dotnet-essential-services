using BaseModule.Domain.Entities;

namespace BlobModule.Domain.Entities
{
    public class BlobFile : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public string BucketObjectId { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;
    }
}
