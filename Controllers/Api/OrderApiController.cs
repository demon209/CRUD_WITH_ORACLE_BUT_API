using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPetService _petService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public OrderApiController(IOrderService orderService, IPetService petService, IProductService productService, ICustomerService customerService)
        {
            _orderService = orderService;
            _petService = petService;
            _productService = productService;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            var orders = _orderService.GetAll();
            var totalItems = orders.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var ordersOnPage = orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Data = ordersOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }

        [HttpGet("search")]
        public IActionResult Search(string keyword = "", int page = 1, int pageSize = 5)
        {
            var filteredOrders = _orderService.SearchOrders(keyword ?? "");
            var totalItems = filteredOrders.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var ordersOnPage = filteredOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Data = ordersOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("{id}/detail")]
        public IActionResult GetDetail(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            var customer = _customerService.GetById(order.CustomerId);
            var pet = order.PetId.HasValue ? _petService.GetById(order.PetId.Value) : null;
            var product = order.ProductId.HasValue ? _productService.GetById(order.ProductId.Value) : null;

            return Ok(new
            {
                Order = order,
                Customer = customer,
                Pet = pet,
                Product = product
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = _orderService.Add(order);
            if (message.Contains("Them don hang thanh cong!"))
                return Ok(new { Message = message });

            return BadRequest(new { Message = message });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            order.OrderId = id;
            var message = _orderService.Update(order);
            if (message.Contains("Cap nhat don hang thanh cong!"))
                return Ok(new { Message = message });

            return BadRequest(new { Message = message });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var message = _orderService.Delete(id);
                if (message.Contains("Xoa don hang thanh cong!"))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi xóa: " + ex.Message });
            }
        }
    }
}
