using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet("/khachhang")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
