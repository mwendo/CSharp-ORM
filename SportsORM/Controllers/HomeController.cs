using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsORM.Models;


namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context _context;

        public HomeController(Context DBContext)
        {
            _context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Baseball"))
                .ToList();
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
            // all women's leagues
            ViewBag.WomensLeagues = _context.Leagues.Where(w => w.Name.Contains("Womens"));
            // all leagues where sport is hockey
            ViewBag.HockeyLeagues = _context.Leagues.Where(h => h.Name.Contains("Hockey"));
            // all leagues where sport is not football
            ViewBag.NotFootballLeagues = _context.Leagues.Where(x => !x.Sport.Contains("Football"));
            // all leagues that call themselves 'conferences'
            ViewBag.ConferenceLeagues = _context.Leagues.Where(c => c.Name.Contains("Conference"));
            // all leagues in the Atlantic region
            ViewBag.AtlanticRegionLeagues = _context.Leagues.Where(a => a.Name.Contains("Atlantic"));
            //all teams based in Dallas
            ViewBag.DallasTeams = _context.Teams.Where(d => d.Location.Contains("Dallas"));
            // all teams name the Raptors
            ViewBag.RaptorsTeams = _context.Teams.Where(r => r.TeamName.Contains("Raptors"));
            // all teams whose location includes "City"
            ViewBag.CityLocations = _context.Teams.Where(city => city.Location.Contains("City"));
            // all temas that begin with "T"
            ViewBag.StartWithT = _context.Teams.Where(t => t.TeamName.Contains("T"));
            // all teams order alphabetically by location
            ViewBag.AlphabeticalOrderLocation = _context.Teams.OrderBy(l => l.Location);
            // all teams ordered by team name in reverse alphabet order
            ViewBag.ReverseAlphabet = _context.Teams.OrderByDescending(y => y.TeamName);
            // every player with last name "Cooper"
            ViewBag.Cooper = _context.Players.Where(coop => coop.LastName.Contains("Cooper"));
            // every player with first name "Joshua"
            ViewBag.Joshua = _context.Players.Where(josh => josh.FirstName.Contains("Joshua"));
            // every player with last name "Cooper" EXCEPT ones with "Joshua" as first name
            ViewBag.NoJoshes = _context.Players.Where(h => h.LastName.Contains("Cooper")).Where(d => !d.FirstName.Contains("Joshua"));
            // all players with first name "Alexander" OR first name "Wyatt"
            ViewBag.EitherOr = _context.Players.Where(u => u.FirstName.Contains("Alexander") || u.FirstName.Contains("Wyatt"));
            return View();
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            return View();
        }

    }
}