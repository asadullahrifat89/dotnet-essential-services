﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teams.ContentMangement.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class ProductProjectMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ProductId { get; set; } = string.Empty;

        public string ProjectId { get; set; } = string.Empty;
    }
}