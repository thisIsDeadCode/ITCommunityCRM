using ITCommunityCRM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace ITCommunityCRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(
            string id = null,
            string first_name = null,
            string username = null,
            string auth_date = null,
            string hash = null)
        {
            if (
                !string.IsNullOrEmpty(id) &&
                !string.IsNullOrEmpty(first_name) &&
                !string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(auth_date) &&
                !string.IsNullOrEmpty(hash)
            )
            {
                return RedirectToAction("TelegramLogin", "ExternalAuthorization", new { id, first_name, username, auth_date, hash });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
