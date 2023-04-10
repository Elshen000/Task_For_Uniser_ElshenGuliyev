using System.Collections.Generic;

namespace Task_For_Uniser_ElshenGuliyev.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
