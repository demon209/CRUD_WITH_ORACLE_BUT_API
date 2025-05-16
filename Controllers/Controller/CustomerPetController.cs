using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CustomerPetController : Controller
    {
        [HttpGet("/Dvdanglam")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
