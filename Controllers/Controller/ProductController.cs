using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet("/Sanpham")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
