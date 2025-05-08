using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services; // Service namespace
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var pets = _petService.GetAllPets();
            var products = _productService.GetAllProducts();
            var customers = _customerService.GetAllCustomers();
            var availablePets = pets.Where(p => p.Status != "Đã bán").ToList();
            ViewBag.Pets = availablePets;

            var availableProducts = products.Where(p => p.ProductStock > 0).ToList();
            ViewBag.Products = availableProducts;
            ViewBag.Customers = customers;

            ViewBag.ProductPrices = products.ToDictionary(p => p.ProductId.ToString(), p => p.ProductPrice);
            ViewBag.PetPrices = pets.ToDictionary(p => p.PetId.ToString(), p => p.Price);

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
            if (message.Contains("Them don hang thanh cong!"))
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

            var pets = _petService.GetAllPets();
            var products = _productService.GetAllProducts();
            var customers = _customerService.GetAllCustomers();

            // Gộp tên khách hàng
            var customerList = customers.Select(c => new
            {
                Text = $"{c.FirstName} {c.LastName}",
                Value = c.CustomerId
            }).ToList();

            ViewBag.Customers = new SelectList(customerList, "Value", "Text", order.CustomerId);

            // ==== XỬ LÝ THÚ CƯNG ====
            var availablePets = pets.Where(p => p.Status != "Đã bán").ToList();

            // Nếu thú cưng trong đơn hàng đã bán, thêm lại để hiển thị
            var currentPet = order.PetId.HasValue ? pets.FirstOrDefault(p => p.PetId == order.PetId.Value) : null;
            if (currentPet != null && !availablePets.Any(p => p.PetId == currentPet.PetId))
            {
                availablePets.Add(currentPet);
            }

            ViewBag.Pets = new SelectList(availablePets, "PetId", "PetName", order.PetId);

            // ==== XỬ LÝ SẢN PHẨM ====
            var availableProducts = products.Where(p => p.ProductStock > 0).ToList();

            var currentProduct = order.ProductId.HasValue ? products.FirstOrDefault(p => p.ProductId == order.ProductId.Value) : null;
            if (currentProduct != null && !availableProducts.Any(p => p.ProductId == currentProduct.ProductId))
            {
                availableProducts.Add(currentProduct);
            }

            ViewBag.Products = new SelectList(availableProducts, "ProductId", "ProductName", order.ProductId);

            // Dữ liệu giá
            ViewBag.ProductPrices = products.ToDictionary(p => p.ProductId.ToString(), p => p.ProductPrice);
            ViewBag.PetPrices = pets.ToDictionary(p => p.PetId.ToString(), p => p.Price);

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
        [HttpGet]
        public IActionResult OrderDetail(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var customer = _customerService.GetCustomerById(order.CustomerId);
            var pet = order.PetId.HasValue ? _petService.GetPetById(order.PetId.Value) : null;
            var product = order.ProductId.HasValue ? _productService.GetProductById(order.ProductId.Value) : null;

            var viewModel = new OrderDetailViewModel
            {
                Order = order,
                Customer = customer,
                Pet = pet,
                Product = product
            };

            return View(viewModel);
        }

    }
}
