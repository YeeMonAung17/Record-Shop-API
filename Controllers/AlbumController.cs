using Microsoft.AspNetCore.Mvc;

namespace Record_Shop.Controllers
{
    public class AlbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
