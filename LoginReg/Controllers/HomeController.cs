using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginReg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public User LoggedID() //Return User model if valid - else null
        {
            int? LoggedID = HttpContext.Session.GetInt32("LoggedIn");
            User Logged = dbContext.Users.FirstOrDefault(u => u.UserId == LoggedID);
            return Logged;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
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
                return RedirectToAction("Success");
            } else {
                return View("Index");
            }
        }

        [HttpGet("login")]
        public IActionResult ViewLogin()
        {
            return View("ViewLogin");
        }

        [HttpPost("login/user")]
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
                    return View("ViewLogin");
                }
                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();
                // verify provided password against has stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Email or Password is incorrect!");
                    return View("ViewLogin");
                }
                HttpContext.Session.SetInt32("LoggedIn", userInDb.UserId);
                return RedirectToAction("Success");
            } else {
                return View("ViewLogin");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            if(LoggedID() != null)
            {
                return View("Success");
            } else {
                return RedirectToAction("ViewLogin");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
