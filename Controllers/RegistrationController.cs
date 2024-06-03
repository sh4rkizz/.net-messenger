using dotnet_messenger.Database;
using dotnet_messenger.Database.Models;
using Microsoft.AspNetCore.Mvc;
using dotnet_messenger.Models;


namespace dotnet_messenger.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel form)
        {
            if (string.IsNullOrWhiteSpace(form.Login))
            {
                ViewData["Login"] = "This field is required";
                return View();
            }

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

            if (db.User.Any(u => u.Login == form.Login))
            {
                ViewData["Login"] = "This login is already taken";
                return View();
            }

            if (db.User.Any(u => u.Username == form.Username))
            {
                ViewData["Username"] = "This username is already taken";
                return View();
            }

            var user = new MessengerUser
            {
                Login = form.Login,
                Username = form.Username,
                Password = form.Password
            };

            await db.User.AddAsync(user);
            await db.SaveChangesAsync();

            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("Login", user.Login, cookieOptions);
            Response.Cookies.Append("Password", user.Password, cookieOptions);

            return RedirectToAction("Messenger", "Messenger");
        }
    }
}
