using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers.Api
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerApiController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            var customers = _customerService.GetAll();
            var totalItems = customers.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customersOnPage = customers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Data = customersOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }

        [HttpGet("search")]
        public IActionResult Search(string keyword = "", int page = 1, int pageSize = 5)
        {
            var filtered = _customerService.SearchCustomers(keyword ?? "");
            var totalItems = filtered.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var customersOnPage = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Data = customersOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }
        [HttpGet("/api/customers/list")]
        public IActionResult GetCustomers()
        {
            var customers = _customerService.GetAll()
                .Select(c => new
                {
                    c.CustomerId,
                    FullName = $"{c.CustomerId} - {c.FirstName} {c.LastName}"
                }).ToList();

            return Ok(customers);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null) return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = _customerService.Add(customer);
            if (message.Contains("Them khach hang thanh cong!"))
                return Ok(new { Message = message });

            return BadRequest(new { Message = message });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            customer.CustomerId = id;
            var message = _customerService.Update(customer);
            if (message.Contains("Cap nhat thong tin khach hang thanh cong!"))
                return Ok(new { Message = message });

            return BadRequest(new { Message = message });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var resultMessage = _customerService.Delete(id);
                if (resultMessage.Contains("Xoa khach hang thanh cong!"))
                    return Ok(new { Message = resultMessage });

                return BadRequest(new { Message = resultMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi xóa: " + ex.Message });
            }
        }
    }
}
