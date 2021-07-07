using Microsoft.AspNetCore.Mvc;

namespace PokerWithFriends.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
