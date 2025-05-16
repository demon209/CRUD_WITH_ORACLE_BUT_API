using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // Đây sẽ map đến: /Order/Index hoặc /hoadon 
        public IActionResult Index()
        {
            return View();
        }

        // Đây sẽ map đến: /Order/ChiTiet hoặc /hoadon/ChiTiet
        public IActionResult OrderDetail(int id)
        {
            var order = _orderService.GetById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }
    }



}
