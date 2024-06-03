using dotnet_messenger.Database;
using dotnet_messenger.Models;
using dotnet_messenger.Database.Models;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_messenger.Controllers
{
    public class MessengerController : Controller
    {

        [HttpGet]
        public IActionResult Messenger(FilterModel model)
        {
            using ApplicationContext db = new();

            var user = (
                from u in db.User
                where (
                    u.Login == Request.Cookies["Login"]
                    && u.Password == Request.Cookies["Password"]
                )
                select u
            ).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var messagesQuery = (
                from msg in db.Message
                where msg.ToId == user.Id || msg.FromId == user.Id
                select new MessageModel
                {
                    Id = msg.Id,
                    From = db.User.First(u => u.Id == msg.FromId).Username,
                    To = db.User.First(u => u.Id == msg.ToId).Username,
                    Title = msg.Title,
                    Text = msg.Text,
                    Date = msg.Date,
                    Status = msg.Status
                }
            ).OrderByDescending(m => m.Date);

            var filteredByStatusMessages = !string.IsNullOrWhiteSpace(model.Status)
                ? messagesQuery.Where((msg) => msg.Status == (model.Status == "read"))
                : messagesQuery;

            var filteredByUsernameMessages = !string.IsNullOrWhiteSpace(model.Username)
                ? filteredByStatusMessages.Where((msg) => msg.From == model.Username || msg.To == model.Username)
                : filteredByStatusMessages;

            ViewData["Username"] = user.Username;
            ViewData["Messages"] = filteredByUsernameMessages.ToArray();

            return View();
        }

        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            using ApplicationContext db = new();

            var message = db.Message.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            message.Status = true;
            db.Message.Update(message);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult Messenger(SendMessageModel model, FilterModel preserveFilters)
        {
            using ApplicationContext db = new();

            if (string.IsNullOrWhiteSpace(model.To))
            {
                ViewData["To-Message"] = "This field is required";
                return View();
            }

            if (string.IsNullOrWhiteSpace(model.Text))
            {
                ViewData["Text-Message"] = "This field is required";
                return View();
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                ViewData["Title-Message"] = "This field is required";
                return View();
            }

            var user = (
                from usr in db.User
                where usr.Login == Request.Cookies["Login"]
                where usr.Password == Request.Cookies["Password"]
                select usr
            ).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var receiverUser = (
                from usr in db.User
                where usr.Username == model.To
                select usr
            ).FirstOrDefault();

            if (receiverUser == null)
            {
                ViewData["To-Message"] = $"User '{model.To}' was not found";
                return View();
            }

            DateTime currentTime = DateTime.UtcNow;
            db.Message.Add(new Message
            {
                FromId = user.Id,
                ToId = receiverUser.Id,
                Title = model.Title,
                Text = model.Text,
                Date = DateTime.UtcNow,
                Status = false
            });
            db.SaveChanges();

            return RedirectToAction("Messenger", "Messenger");
        }
    }
}
