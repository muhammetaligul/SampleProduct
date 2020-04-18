using System;
using System.Collections.Generic;

namespace SampleProduct.ORM.Models.DB
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public virtual Category Category { get; set; }
    }
}
