using Journer.Repository;
using Journey.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JoyfulJourney.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository _repository;

        public AccountController(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            _repository.AddCustomer(registerDTO);
            return RedirectToAction("Login","Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var result = _repository.ValidateUser(username, password);

            if (result.IsAdmin)
            {
                return RedirectToAction("AdminIndex", "Admin");
            }
            else if (result.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            // Invalid Credentials
            ViewBag.Message = "Invalid username or password.";
            return View();

        }
    }
}
