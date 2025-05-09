using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }

        [HttpGet("/Khachhang")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allCustomers = _customerService.GetAll();
            int totalItems = allCustomers.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customersOnPage = allCustomers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new CustomerListViewModel
            {
                Customers = customersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Khachhang", new { page = p })!
            };

            return View(model);
        }

        [HttpGet("/Customer/LoadCustomerPartial")]
        public IActionResult LoadCustomerPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            var filteredCustomer = _customerService.SearchCustomers(keyword ?? "");

            int totalItems = filteredCustomer.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var customersOnPage = filteredCustomer
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new CustomerListViewModel
            {
                Customers = customersOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("LoadCustomerPartial", new { keyword = keyword, page = p })!
            };

            return PartialView("~/Views/Customer/_CustomerListPartial.cshtml", model);
        }

        [HttpGet]
        public IActionResult CreateCustomers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomers(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            var message = _customerService.Add(customer);
            if (message.Contains("Them khach hang thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", message);
            return View(customer);
        }

        [HttpPost]
        public IActionResult DeleteCustomer(int id, int page = 1)
        {
            try
            {
                var resultMessage = _customerService.Delete(id);
                if (resultMessage.Contains("Xoa khach hang thanh cong!"))
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
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer customer, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(customer);

            var message = _customerService.Update(customer);
            if (message.Contains("Cap nhat thong tin khach hang thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index", new { page = page });
            }

            ModelState.AddModelError("", message);
            return View(customer);
        }
    }
}
