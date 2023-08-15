using BlobModule.Domain.Entities;

namespace BlobModule.Application.DTOs.Responses
{
    public class BlobFileResponse : BlobFile
    {
        public byte[] Bytes { get; set; } = new byte[] { };

        public static BlobFileResponse Initialize(BlobFile blobFile, byte[] bytes)
        {
            return new BlobFileResponse()
            {
                Bytes = bytes,
                Id = blobFile.Id,
                TimeStamp = blobFile.TimeStamp,
                BucketObjectId = blobFile.BucketObjectId,
                ContentType = blobFile.ContentType,
                Extension = blobFile.Extension,
                Name = blobFile.Name,
            };
        }
    }
}
