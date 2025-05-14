using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Controllers
{
    public class CustomerPetController : Controller
    {
        private readonly ICustomerPetService _CustomerPetService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public CustomerPetController(
            ICustomerPetService CustomerPetService,
            ICustomerService customerService,
            IProductService productService)
        {
            _CustomerPetService = CustomerPetService;
            _customerService = customerService;
            _productService = productService;
        }

        private void PopulateCustomerAndProductDropdowns()
        {
            var customers = _customerService.GetAll();
            ViewBag.Customers = customers.Select(p => new SelectListItem
            {
                Value = p.CustomerId.ToString(),
                Text = $"{p.CustomerId} - {p.LastName?.Trim()} {p.FirstName?.Trim()} - {p.PhoneNumber} - {p.Email}"
            }).ToList();
            Console.WriteLine($"Customers count: {ViewBag.Customers.Count}"); // Debug log

            var products = _productService.GetAll().Where(p => p.ProductType == "service").ToList();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.ProductId.ToString(),
                Text = $"{p.ProductId} - {p.ProductName}"
            }).ToList();
            Console.WriteLine($"Products count: {ViewBag.Products.Count}"); // Debug log
        }

        [HttpGet]
        public IActionResult GetCustomerPets()
        {
            var customerPets = _CustomerPetService.GetAll();
            return Ok(customerPets);
        }

        [HttpGet("/Dvdanglam")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allCustomerPets = _CustomerPetService.GetAll();
            int totalItems = allCustomerPets.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customerPetsOnPage = allCustomerPets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new CustomerPetListViewModel
            {
                CustomerPets = customerPetsOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Khachhang", new { page = p })!
            };

            return View(model);
        }

        [HttpGet("/CustomerPet/LoadCustomerPetPartial")]
        public IActionResult LoadCustomerPetPartial(string keyword = "", int page = 1, int pageSize = 5, bool onlyAvailable = false)
        {
            try
            {
                var filteredCustomerPet = _CustomerPetService
                .SearchCustomerPet(keyword ?? "")
                .OrderBy(p => p.CustomerPetId)
                .ToList();
                 if (onlyAvailable)
                {
                    filteredCustomerPet = filteredCustomerPet.Where(p => p.Status == "Hoàn thành").OrderBy(p => p.CustomerPetId).ToList();
                }


                int totalItems = filteredCustomerPet.Count;
                int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var customerPetsOnPage = filteredCustomerPet
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var model = new CustomerPetListViewModel
                {
                    CustomerPets = customerPetsOnPage,
                    CurrentPage = page,
                    CountPages = countPages,
                    GenerateUrl = p => Url.Action("LoadCustomerPetPartial", new { keyword = keyword, page = p, onlyAvailable = onlyAvailable })!
                };

                return PartialView("~/Views/CustomerPet/_CustomerPetListPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CreateCustomerPets()
        {
            PopulateCustomerAndProductDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomerPets(CustomerPet customerPet)
        {
            if (!ModelState.IsValid)
            {
                PopulateCustomerAndProductDropdowns();
                return View(customerPet);
            }

            var message = _CustomerPetService.Add(customerPet);
            if (message.Contains("Them thong tin dich vu thanh cong!", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", message);
            PopulateCustomerAndProductDropdowns();
            return View(customerPet);
        }

        [HttpPost]
        public IActionResult DeleteCustomerPet(int id, int page = 1)
        {
            try
            {
                var resultMessage = _CustomerPetService.Delete(id);
                if (resultMessage.Contains("Xoa thong tin dich vu thanh cong!", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Success"] = resultMessage;
                }
                else
                {
                    ModelState.AddModelError("", resultMessage);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Loi khi xoa: " + ex.Message;
            }

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public IActionResult EditCustomerPet(int id)
        {
            var customerPet = _CustomerPetService.GetById(id);
            if (customerPet == null)
                return NotFound();

            PopulateCustomerAndProductDropdowns();
            return View(customerPet);
        }

        [HttpPost]
        public IActionResult EditCustomerPet(CustomerPet customerPet, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                PopulateCustomerAndProductDropdowns();
                return View(customerPet);
            }

            var message = _CustomerPetService.Update(customerPet);
            if (message.Contains("Cap nhat thong tin thanh cong!", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index", new { page = page });
            }

            ModelState.AddModelError("", message);
            PopulateCustomerAndProductDropdowns();
            return View(customerPet);
        }
        [HttpPost]
        public JsonResult ToggleStatus([FromBody] int id)
        {
            var result = _CustomerPetService.ToggleStatus(id);

            if (result == null)
                return Json(new { success = false, message = "Không tìm thấy dịch vụ." });

            return Json(new { success = true, newStatus = result });
        }


    }
}