using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class ProductSearchCriteriaMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ProductId { get; set; } = string.Empty;

        public string SearchCriteriaId { get; set; } = string.Empty;
    }
}
