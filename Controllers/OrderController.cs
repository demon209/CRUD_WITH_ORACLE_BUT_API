using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services; // Service namespace
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPetService _petService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        public OrderController(IOrderService orderService, IPetService petService, IProductService productService, ICustomerService customerService)
        {
            _orderService = orderService;
            _petService = petService;
            _productService = productService;
            _customerService = customerService;
        }


        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("/Hoadon")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allOrders = _orderService.GetAllOrders();

            int totalItems = allOrders.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var ordersOnPage = allOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new OrderListViewModel
            {
                Orders = ordersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Hoadon", new { page = p })!
            };

            return View(model);
        }

        [HttpGet("/Order/LoadOrdersPartial")]
        public IActionResult LoadOrdersPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            try
            {
                var filteredOrders = _orderService.SearchOrders(keyword);

                int totalItems = filteredOrders.Count;
                int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var ordersOnPage = filteredOrders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new OrderListViewModel
                {
                    Orders = ordersOnPage,
                    CurrentPage = page,
                    CountPages = countPages,
                    GenerateUrl = p => Url.Action("LoadOrdersPartial", new { keyword = keyword, page = p })!
                };

                return PartialView("~/Views/Order/_OrderListPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }

        [HttpGet("Order/CreateOrders")]
        public IActionResult CreateOrders()
        {
            ViewBag.Pets = _petService.GetAllPets();
            ViewBag.Products = _productService.GetAllProducts();
            ViewBag.Customers = _customerService.GetAllCustomers();

            return View();
        }
        [HttpPost]
        public IActionResult CreateOrders(Order order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Pets = _petService.GetAllPets();
                ViewBag.Products = _productService.GetAllProducts();
                return View(order);
            }

            var message = _orderService.AddOrder(order);
            if (message.Contains("Thêm hóa đơn thành công!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index");
            }

            // Nếu thêm thất bại, cũng cần gán lại ViewBag
            ViewBag.Pets = _petService.GetAllPets();
            ViewBag.Products = _productService.GetAllProducts();
            ViewBag.Customers = _customerService.GetAllCustomers();
            ModelState.AddModelError("", message);
            return View(order);
        }


        [HttpPost]
        public IActionResult DeleteOrder(int id, int page = 1)
        {
            var message = _orderService.DeleteOrder(id);
            if (message.Contains("Xoa don hang thanh cong!"))
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }
            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public IActionResult EditOrder(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public IActionResult EditOrder(Order order, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(order);

            var message = _orderService.UpdateOrder(order);
            if (message.Contains("Cap nhat don hang thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index", new { page = page });
            }

            ModelState.AddModelError("", message);
            return View(order);
        }
    }
}
