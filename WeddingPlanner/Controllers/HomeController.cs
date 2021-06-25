using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public User LoggedInUser()
        {
            int? LoggedID = HttpContext.Session.GetInt32("LoggedIn");
            User logged = dbContext.Users.FirstOrDefault(u => u.UserId == LoggedID);
            return logged;
        }
        public int UserID()
        {
            int UserID = LoggedInUser().UserId;
            return UserID;
        }

        // Register New User
        [HttpPost("newUser")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("LoggedIn", user.UserId);
                return RedirectToAction("Dashboard");
            } else {
                return View("Index");
            }
        }

        // Login Current User
        [HttpPost("loginUser")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If initial ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // verify provided password against has stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Email or Password is incorrect!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("LoggedIn", userInDb.UserId);
                return RedirectToAction("Dashboard");
            } else {
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if(LoggedInUser() != null)
            {
                ViewBag.UserId = UserID();
                ViewBag.Weddings = dbContext.Weddings.Include(w => w.Guests).ThenInclude(r => r.User).Include(u => u.Planner).ToList();
                return View("Dashboard");
            } else {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("delete/{WeddingId}")]
        public IActionResult DeleteWedding(int WeddingId)
        {
            Wedding DeleteWedding = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == WeddingId);
            if (DeleteWedding.UserId == UserID())
            {
                dbContext.Remove(DeleteWedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("/rsvp/{WeddingId}")]
        public IActionResult RSVPWedding(int WeddingId)
        {
            RSVP newRSVP = new RSVP();
            newRSVP.UserId = UserID();
            newRSVP.WeddingId = WeddingId;
            dbContext.RSVPs.Add(newRSVP);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("planWedding")]
        public IActionResult PlanWedding()
        {
            if(LoggedInUser() != null)
            {
                @ViewBag.Id = UserID();
                return View("PlanWedding");
            } else {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("addWedding")]
        public IActionResult AddWedding(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newWedding);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingDetails");
            } else {
                return View("PlanWedding");
            }
        }

        [HttpGet("wedding/details/{WeddingId}")]
        public IActionResult WeddingDetails(int WeddingId)
        {
            ViewBag.ThisWedding = dbContext.Weddings.Include(w => w.Guests).ThenInclude(u => u.User).FirstOrDefault(u => u.WeddingId  == WeddingId);
            return View("WeddingDetails");
        }
    }
}