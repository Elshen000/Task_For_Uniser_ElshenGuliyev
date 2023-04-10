using System;
using System.ComponentModel.DataAnnotations.Schema;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreateTime { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
    }
}
