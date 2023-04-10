
using Microsoft.EntityFrameworkCore;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options) 
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
