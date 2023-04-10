using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_For_Uniser_ElshenGuliyev.DAL;
using Task_For_Uniser_ElshenGuliyev.Extensions;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.Controllers
{
    public class ProductsController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion

        #region Index
        // GET: Products
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.CurrentPage = page;//Default page number ==1
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Products.Count() / 4);//There are 8 products on 1 page.Therefore, the number of all products is divided by 8 and the number of pages is found.Count type is integer.Accordingly, when Count is the number of products, a fraction can be obtained. Accordingly, we use Math.Ceiling (decimal) so that the whole number is obtained and the new remainder is added to a new page.
            List<Product> products = await _db.Products.OrderByDescending(p=>p.Id).Include(p => p.Category).Include(p=>p.ProductImages).Include(p=>p.ProductDetail).Skip((page - 1) * 4).Take(4).ToListAsync();
            return View(products);
        }
        #endregion

        #region ProductDetail
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = _db.Products.Include(p => p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            return View(dbProduct);


        }
        #endregion

        #region CreateProduct
        //GET
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            return View();

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, int catId)
        {
            ViewBag.Categories = _db.Categories.ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            bool exist = _db.Products.Any(p => p.Name == product.Name);
            if (exist)
            {
                ModelState.AddModelError("Name", "This Name is already exist");
                return View();
            }
            if (product.Name == null)
            {
                ModelState.AddModelError("Name", "Please Enter Name");
                return View();
            }
            #region SavePhoto
            List<ProductImage> productImages = new List<ProductImage>();
            if (product.Photos.Length != null)
            {

                foreach (IFormFile Photos in product.Photos)
                {
                    ProductImage productImage = new ProductImage();
                    if (Photos == null)
                    {
                        ModelState.AddModelError("Photos", "Şəkil seçin!");
                        return View();
                    }
                    if (!Photos.IsImage())
                    {
                        ModelState.AddModelError("Photos", "Şəkil seçin!");
                        return View();
                    }
                    if (!Photos.Max5Mb())
                    {
                        ModelState.AddModelError("Photos", "Şəkilin həcmi 5MB-dan çoxdur!");
                        return View();
                    }
                    string folder = Path.Combine(_env.WebRootPath,  "img");
                    productImage.Image = await Photos.SaveFileAsync(folder);
                    productImages.Add(productImage);
                }
            }

            #endregion
            product.ProductDetail.CreateTime = DateTime.UtcNow.AddHours(4);
            product.CategoryId = catId;
            product.ProductImages=productImages;
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion

        #region ProductUpdate
        // GET: Products/Update/id
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _db.Products.Include(x => x.ProductDetail).Include(p => p.ProductImages).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _db.Categories.ToList();
            return View(product);
        }

        // POST: Products/Update/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product, int catId)
        {
            ViewBag.Categories = _db.Categories.ToList();
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            Product dbProduct = await _db.Products.Include(x=>x.ProductDetail).Include(p => p.ProductImages).Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbProduct==null)
            {
                return NotFound();
            }
           
            bool exist2 = _db.Categories.Any(x => x.Name == product.Name && x.Id! == id);
            if (exist2)
            {
                ModelState.AddModelError("Name", "This Name is exist");
                return View(dbProduct);
            }
            #region SavePhoto
            List<ProductImage> productImages = new List<ProductImage>();
            if (product.Photos != null)
            {

                foreach (IFormFile Photos in product.Photos)
                {
                    ProductImage productImage = new ProductImage();
                    if (Photos == null)
                    {
                        ModelState.AddModelError("Photos", "Şəkil seçin!");
                        return View();
                    }
                    if (!Photos.IsImage())
                    {
                        ModelState.AddModelError("Photos", "Şəkil seçin!");
                        return View();
                    }
                    if (!Photos.Max5Mb())
                    {
                        ModelState.AddModelError("Photos", "Şəkilin həcmi 5MB-dan çoxdur!");
                        return View();
                    }
                    string folder = Path.Combine(_env.WebRootPath, "img");
                    productImage.Image = await Photos.SaveFileAsync(folder);
                    productImages.Add(productImage);
                }
            }

            #endregion
           dbProduct.ProductImages.AddRange(productImages);
           dbProduct.Name =product.Name;   
           dbProduct.ProductDetail.Description=product.ProductDetail.Description;   
           dbProduct.ProductDetail.Price=product.ProductDetail.Price;
           dbProduct.CategoryId = product.CategoryId = catId;

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteProduct
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region DeleteProductImage
        public async Task<IActionResult> DeleteProductImage(int imgId, int count)
        {
            if (count == 1)
            {
                return Content("error");
            }
            ProductImage productImage = await _db.ProductImages.FirstOrDefaultAsync(x => x.Id == imgId);
            string path = Path.Combine(_env.WebRootPath, "img");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _db.ProductImages.Remove(productImage);
            await _db.SaveChangesAsync();
            return Ok();
        }
        #endregion


    }
}
