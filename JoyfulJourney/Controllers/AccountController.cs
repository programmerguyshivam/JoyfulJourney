using Journer.Repository;
using Journey.DTOs;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

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
            string subject = "New Car Added";
            string body = $"Dear{registerDTO.Email},<br>" +
                       "Your Car Was Successfully Added.<br> " +
                       "$Car: { insert.CarName}<br>" +
            "$Price : {insert.Carprice}<br>" +
            "Thank You !!";

            _repository.SendEMAIL(registerDTO.Email, subject, body);
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
            var result = _repository.LoginValidate(username, password);

            if (result.IsAdmin)
            {
                return RedirectToAction("AdminCrud", "Admin");
            }
            else if (result.IsValid)
            {
                HttpContext.Session.SetString("GetSessionUserName", username);
                return RedirectToAction("Index", "Home");
            }
            // Invalid Credentials
            ViewBag.Message = "Invalid username or password.";
            return View();

        }
    }
}
