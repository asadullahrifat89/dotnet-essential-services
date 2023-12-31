﻿using Base.Domain.Entities;
using System.Text.Json.Serialization;

namespace Teams.ContentMangement.Domain.Entities
{
    public class Product : EntityBase
    {
        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Details description of the product. Escaped rich text is expected here.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount of manpower available for this product.
        /// </summary>
        public int ManPower { get; set; } = 0;

        /// <summary>
        /// Amount of experience the manpower has for working on this product in years.
        /// </summary>
        public int Experience { get; set; } = 0;

        /// <summary>
        /// Empolyment types available for this product
        /// </summary>
        public EmploymentType[] EmploymentTypes { get; set; } = new EmploymentType[] { };

        /// <summary>
        /// Cost indication for this product.
        /// </summary>
        public ProductCostType ProductCostType { get; set; }

        /// <summary>
        /// Icon image url of this product.
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Publishing status of the product in the cms.
        /// </summary>
        public PublishingStatus PublishingStatus { get; set; } = PublishingStatus.Published;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        Contractual
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductCostType
    {
        Low,
        Medium,
        High
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PublishingStatus
    {
        Published,
        Unpublished,
    }
}
