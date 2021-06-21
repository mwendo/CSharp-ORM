using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            // Get All Dishes
            ViewBag.AllDishes = dbContext.Dishes
                                .Include(d => d.Creator)
                                .ToList();
            return View();
        }

        [HttpGet("chefs")]
        public IActionResult Chefs()
        {
            ViewBag.Chefs = dbContext.Chefs.Include(u => u.CreatedDishes).ToList();
            return View("Chefs");
        }

        [HttpGet("add/chef")]
        public IActionResult NewChef()
        {
            return View("AddChef");
        }

        [HttpPost("addChef")]
        public IActionResult AddChef(Chef newChef)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newChef);
                dbContext.SaveChanges();
                return RedirectToAction("Chefs");
            } else {
                return View("AddChef");
            }
        }

        [HttpGet("new")]
        public IActionResult NewDish()
        {
            ViewBag.Chefs = dbContext.Chefs.ToList();
            return View("NewDish");
        }

        [HttpPost("addDish")]
        public IActionResult AddDish(Dish newDish)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("NewDish");
            }
        }

        [HttpGet("details/{DishesId}")]
        public IActionResult Details(int DishesId)
        {
            ViewBag.OneDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishesId == DishesId);
            return View("Details");
        }

        [HttpGet("delete/{DishesId}")]
        public IActionResult Delete(int DishesId)
        {
            Dish RetrievedDish = dbContext.Dishes.FirstOrDefault(dish => dish.DishesId == DishesId);
            dbContext.Dishes.Remove(RetrievedDish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet("edit/{DishesId}")]
        public IActionResult Edit(int DishesId)
        {
            Dish DishToEdit = dbContext.Dishes.FirstOrDefault(dish => dish.DishesId == DishesId);
            ViewBag.Chefs = dbContext.Dishes.Include(d => d.Creator).ToList();
            return View("Edit", DishToEdit);
        }

        [HttpPost("update/{DishesId}")]
        public IActionResult Update(int DishesId, Dish EditDish)
        {
            Dish DishToEdit = dbContext.Dishes.FirstOrDefault(dish => dish.DishesId == DishesId);
            DishToEdit.Name = EditDish.Name;
            DishToEdit.ChefId = EditDish.ChefId;
            DishToEdit.Calories = EditDish.Calories;
            DishToEdit.Tastiness = EditDish.Tastiness;
            DishToEdit.Description = EditDish.Description;
            DishToEdit.UpdatedAt = EditDish.UpdatedAt;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}