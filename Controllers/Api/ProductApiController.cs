using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            var products = _productService.GetAll();
            var totalItems = products.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productsOnPage = products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return Ok(new
            {
                Data = productsOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }

        //onlyAvailable
        [HttpGet("search")]
        public IActionResult Search(string keyword = "", int page = 1, int pageSize = 5, bool onlyAvailable = false)
        {
            var filtered = _productService.SearchProducts(keyword ?? "");

            if (onlyAvailable)
            {
                filtered = filtered.Where(p => p.ProductType == "service").ToList();
            }
            var totalItems = filtered.Count;
            var countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productsOnPage = filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            return Ok(new
            {
                Data = productsOnPage,
                CurrentPage = page,
                TotalPages = countPages,
                TotalItems = totalItems
            });
        }

        [HttpGet("/api/products/list")]
        public IActionResult GetProducts()
        {
            var products = _productService.GetAll()
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductType,
                    p.ProductStock,
                    Name = $"{p.ProductId} - {p.ProductName}"
                }).Where(p => (p.ProductType == "service") || ((p.ProductType == "product") && (p.ProductStock != 0)))
                .ToList();

            return Ok(products);
        }

        [HttpGet("/api/products/services")]
        public IActionResult GetProductServices()
        {
            var products = _productService.GetAll()
            .Select(p => new
            {
                p.ProductId,
                p.ProductType,
                Name = $"{p.ProductId} - {p.ProductName}"

            }).Where(p => p.ProductType == "service")
            .ToList();
            return Ok(products);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var message = _productService.Add(product);
            if (message.Contains("Them san pham thanh cong!"))
                return Ok(new { Message = message });
            return BadRequest(new { Message = message });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            product.ProductId = id;
            var message = _productService.Update(product);
            if (message.Contains("Cap nhat thong tin san pham thanh cong!"))
                return Ok(new { Message = message });
            return BadRequest(new { Message = message });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var resultMessage = _productService.Delete(id);
                if (resultMessage.Contains("Xoa san pham thanh cong"))
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