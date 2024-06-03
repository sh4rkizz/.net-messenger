using dotnet_messenger.Database;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_messenger.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
