using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class PetController : Controller
    {
        [HttpGet("/Thucung")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
