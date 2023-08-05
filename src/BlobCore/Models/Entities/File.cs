using BaseCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobCore.Models.Entities
{
    public class File : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        public string BucketObjectId { get; set; } = string.Empty;
    }
}
