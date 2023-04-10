using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_For_Uniser_ElshenGuliyev.DAL;
using Task_For_Uniser_ElshenGuliyev.Models;

namespace Task_For_Uniser_ElshenGuliyev.Controllers
{

    
    public class CategoriesController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            ViewBag.CategoriesCount = _db.Categories.Count();
            List<Category> categories = await _db.Categories.OrderByDescending(c=>c.Id).Include(c=>c.Products).Take(8).ToListAsync();
            return View(categories);
        }
        #endregion

        #region CategoriesCreate
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _db.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
           
        }
        #endregion

        #region CategoriesDelete
        public async Task<IActionResult> Delete(int?id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Category category = await _db.Categories.FirstOrDefaultAsync(x=>x.Id==id);
            _db .Categories.Remove(category);   
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");   
        }
        #endregion

        #region LoadMoreCategories
        public async Task<IActionResult> LoadMoreCategories(int skip)
        {
            List<Category> categories =await _db.Categories.Include(c=>c.Products).OrderByDescending(c => c.Id).Skip(skip).Take(8).ToListAsync();
            return PartialView("_CategoriesPartial", categories);
        }
        #endregion

    }
}
