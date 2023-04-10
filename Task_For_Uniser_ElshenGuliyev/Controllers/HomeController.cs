using Task_For_Uniser_ElshenGuliyev.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task_For_Uniser_ElshenGuliyev.DAL;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.Controllers
{
    public class HomeController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
       
        public HomeController(AppDbContext db)
        {
            _db = db;
           
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page=1)
        {
            ViewBag.Categories = _db.Categories.OrderByDescending(c=>c.Id).ToList();//To send all categories to index
            ViewBag.CurrentPage = page;//Default page number ==1
            ViewBag.Pagecount = Math.Ceiling((decimal)_db.Products.Count() / 8);//There are 8 products on 1 page.Therefore, the number of all products is divided by 8 and the number of pages is found.Count type is integer.Accordingly, when Count is the number of products, a fraction can be obtained. Accordingly, we use Math.Ceiling (decimal) so that the whole number is obtained and the new remainder is added to a new page.
            List<Product> products = await _db.Products.Include(p=>p.ProductDetail).Include(p=>p.ProductImages).OrderByDescending(p=>p.Id).Skip((page-1)*8).Take(8).ToListAsync();//to get all product from database and to send to view
            return View(products);
        }
        #endregion

        #region ProductSearch
        public async Task<IActionResult> Search(string keyword)
        {
            List<Product> products = await _db.Products.Include(p=>p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).Where(p=>p.Name.Contains(keyword)).ToListAsync();
            if (keyword == null)
            {
                List< Product > product = await _db.Products.Include(p => p.ProductDetail).Include(p=>p.ProductImages).Include(p=>p.Category).OrderByDescending(p=>p.Id).Take(8).ToListAsync();
                return PartialView("_SearchPartial", product);
            }
            return PartialView("_SearchPartial", products);  
        }
        #endregion

        #region CategoryFilter
        public async Task<IActionResult> CategoryFilter(int? mycatId)
        {
            List<Product> products = await _db.Products.Include(p=>p.Category).Include(p=>p.ProductDetail).Include(p=>p.ProductImages).Where(p=>p.CategoryId== mycatId).ToListAsync();
            if (mycatId == null)
            {
                List<Product> product = await _db.Products.OrderByDescending(p => p.Id).Include(p => p.ProductDetail).Include(p => p.ProductImages).Include(p=>p.Category).Take(8).ToListAsync();
                return PartialView("_CategoryFilterPartial", product);
            }
            return PartialView("_CategoryFilterPartial", products);
            
        }
        #endregion

        #region FindProductForPrice
        public async Task<IActionResult> FindProductForPrice(int? minimumpr,int? maximumpr)
        {
            List<Product> products = await _db.Products.Include(p=>p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).Where(p=>p.ProductDetail.Price>=minimumpr||p.ProductDetail.Price<=maximumpr).ToListAsync();
            if (minimumpr  == null && maximumpr == null)
            {
                List<Product> myProducts = await _db.Products.Include(p => p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).Take(8).ToListAsync();
                return PartialView("_FindProductForPricePartial", myProducts);
            }
            if (minimumpr ==null && maximumpr !=null)
            {
                List<Product> myProducts2 = await _db.Products.Include(p => p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).Where(p =>p.ProductDetail.Price <= maximumpr).ToListAsync();
                return PartialView("_FindProductForPricePartial", myProducts2);
            }

            if (minimumpr != null && maximumpr == null)
            {
                List<Product> myProducts3 = await _db.Products.Include(p => p.ProductDetail).Include(p => p.ProductImages).Include(p => p.Category).Where(p => p.ProductDetail.Price >= minimumpr).ToListAsync();
                return PartialView("_FindProductForPricePartial", myProducts3);
            }
            return PartialView("_FindProductForPricePartial", products);  
        }
        #endregion

    }
}
