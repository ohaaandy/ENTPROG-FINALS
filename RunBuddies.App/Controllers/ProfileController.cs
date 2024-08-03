using Microsoft.AspNetCore.Mvc;

namespace RunBuddies.App.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
