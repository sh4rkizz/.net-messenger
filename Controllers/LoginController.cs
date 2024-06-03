using dotnet_messenger.Models;
using Microsoft.AspNetCore.Mvc;
using dotnet_messenger.Database;


namespace dotnet_messenger.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel form)
        {
            if (string.IsNullOrWhiteSpace(form.Username))
            {
                ViewData["Username"] = "This field is required";
                return View();
            }

            if (string.IsNullOrWhiteSpace(form.Password))
            {
                ViewData["Password"] = "This field is required";
                return View();
            }

            using ApplicationContext db = new();
            var user = (
                from u in db.User
                where u.Username == form.Username && u.Password == form.Password
                select u
            ).FirstOrDefault();

            if (user == null)
            {
                ViewData["Login"] = $"Incorrect data. Try again";
                return View();
            }

            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("Login", user.Login, cookieOptions);
            Response.Cookies.Append("Password", user.Password, cookieOptions);

            return RedirectToAction("Messenger", "Messenger");
        }
    }
}
