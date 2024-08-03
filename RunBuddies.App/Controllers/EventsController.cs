using Microsoft.AspNetCore.Mvc;

namespace RunBuddies.App.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Filter()
        {
            return View();
        }
    }
}
