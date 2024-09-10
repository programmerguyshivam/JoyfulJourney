using Journer.Repository;
using Journey.DTOs;
using JoyfulJourney.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JoyfulJourney.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Packages()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Destinations()
        {
            var data = repository.getAdminDestination();
            return View(data);
        }
        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Journey()
        {
            return View();
        }

        

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CutomerBooking(AddBookUserDTO addBookUserDTO)
        {
            repository.AddBook(addBookUserDTO);
            return RedirectToAction("Index","Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
