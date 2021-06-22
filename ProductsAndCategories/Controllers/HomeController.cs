using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsAndCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAndCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        // Products
        public IActionResult Index()
        {
            ViewBag.Products = dbContext.Products.ToList();
            return View();
        }
        [HttpPost("add/product")]
        public IActionResult AddProduct(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ViewBag.Products = dbContext.Products.ToList();
                return View("Index");
            }
        }
        [HttpGet("products/{ProductId}")]
        public IActionResult ShowProduct(int ProductId)
        {
            ViewBag.SpecificProduct = dbContext.Products.Include(u => u.Associations).ThenInclude(x => x.Category).FirstOrDefault(product => product.ProductId == ProductId);

            ViewBag.ListCategories = dbContext.Categories.Include(u => u.Associations).ThenInclude(x => x.Product).Where(u => u.Associations.All(u => u.ProductId != ProductId)).ToList();
            return View("ShowProduct");
        }
        [HttpPost("add/category/to/product")]
        public IActionResult AddCategoryToProduct(Association newAss)
        {
            dbContext.Add(newAss);
            dbContext.SaveChanges();
            return Redirect($"/products/{newAss.ProductId}");
        }

        // Categories
        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.Categories = dbContext.Categories.ToList();
            return View("Categories");
        }
        [HttpPost("add/category")]
        public IActionResult AddCategory(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            } else {
                ViewBag.Categories = dbContext.Categories.ToList();
                return View("Categories");
            }
        }
        [HttpGet("new/{CategoryId}")]
        public IActionResult ShowCategory(int CategoryId)
        {
            ViewBag.SpecificCategory = dbContext.Categories.Include(u => u.Associations).ThenInclude(x => x.Product).FirstOrDefault(category => category.CategoryId == CategoryId);

            ViewBag.ListProducts = dbContext.Products.Include(u => u.Associations).ThenInclude(x => x.Category).Where(u => u.Associations.All(u => u.CategoryId != CategoryId)).ToList();
            return View("ShowCategory");
        }
        [HttpPost("add/product/to/category")]
        public IActionResult AddProductToCategory(Association newAss)
        {
            // Add Logic
            dbContext.Add(newAss);
            dbContext.SaveChanges();
            return Redirect($"/new/{newAss.CategoryId}");
        }
    }
}
