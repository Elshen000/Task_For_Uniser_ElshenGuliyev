namespace Task_For_Uniser_ElshenGuliyev.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
