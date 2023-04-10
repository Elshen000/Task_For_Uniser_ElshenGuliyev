
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_For_Uniser_ElshenGuliyev.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public IFormFile[]? Photos { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
