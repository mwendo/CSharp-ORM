using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BeltExam.Controllers
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

        [HttpPost("loginUser")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
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
                List<Entertainment> Activities = dbContext.Entertainments.Include(a => a.Participants).ThenInclude(r => r.User).Include(a => a.Coordinator).OrderBy(a => a.EntertainmentDate).Where(a => a.EntertainmentDate > DateTime.Now).ToList();
                return View("Dashboard", Activities);
            } else {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("delete/{EntertainmentId}")]
        public IActionResult DeleteEntertainment(int EntertainmentId)
        {
            Entertainment DeleteEntertainment = dbContext.Entertainments.FirstOrDefault(a => a.EntertainmentId == EntertainmentId);
            if (DeleteEntertainment.UserId == UserID())
            {
                dbContext.Remove(DeleteEntertainment);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                return RedirectToAction("Logout");
            }
        }

        [HttpGet("join/{EntertainmentId}")]
        public IActionResult JoinEntertainment(int EntertainmentId)
        {
            Participant newParticipant = new Participant();
            newParticipant.UserId = UserID();
            newParticipant.EntertainmentId = EntertainmentId;
            dbContext.Participants.Add(newParticipant);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("leave/{EntertainmentId}")]
        public IActionResult LeaveEntertainment(int EntertainmentId)
        {
            Participant participantLeave = dbContext.Participants.FirstOrDefault(u => u.UserId == UserID() && u.EntertainmentId == EntertainmentId);
            dbContext.Participants.Remove(participantLeave);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("planEntertainment")]
        public IActionResult PlanEntertainment()
        {
            if(LoggedInUser() != null)
            {
                @ViewBag.Id = UserID();
                return View("PlanEntertainment");
            } else {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("addEntertainment")]
        public IActionResult AddEntertainment(Entertainment newEntertainment)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newEntertainment);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                return View("PlanEntertainment");
            }
        }

        [HttpGet("activities/details/{EntertainmentId}")]
        public IActionResult EntertainmentDetails(int EntertainmentId)
        {
            if(LoggedInUser() != null)
            {
                ViewBag.UserId = UserID();
                ViewBag.ThisEntertainment = dbContext.Entertainments.Include(a => a.Participants).ThenInclude(u => u.User).Include(a => a.Coordinator).FirstOrDefault(a => a.EntertainmentId == EntertainmentId);
                ViewBag.Participating = dbContext.Participants.FirstOrDefault(u => u.UserId == UserID() && u.EntertainmentId == EntertainmentId);
                return View("EntertainmentDetails");
            } else {
                return RedirectToAction("Index");
            }
        }
    }
}