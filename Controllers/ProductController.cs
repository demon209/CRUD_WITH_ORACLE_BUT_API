using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("/Sanpham")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allProducts = _productService.GetAll();
            int totalItems = allProducts.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productsOnPage = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new ProductListViewModel
            {
                Products = productsOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Sanpham", new { page = p })!
            };

            return View(model);
        }

        [HttpGet("/Product/LoadProductsPartial")]
        public IActionResult LoadProductsPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            var filteredProducts = _productService.SearchProducts(keyword);
            int totalItems = filteredProducts.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var productsOnPage = filteredProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new ProductListViewModel
            {
                Products = productsOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("LoadProductsPartial", new { keyword = keyword, page = p })!
            };

            return PartialView("~/Views/Product/_ProductListPartial.cshtml", model);
        }

        [HttpGet("/Product/CreateProducts")]
        public IActionResult CreateProducts()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProducts(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var resultMessage = _productService.Add(product);

            if (resultMessage.Contains("Them san pham thanh cong!"))
            {
                TempData["Success"] = resultMessage;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", resultMessage);
            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(product);

            var resultMessage = _productService.Update(product);

            if (resultMessage.Contains("Cap nhat thong tin san pham thanh cong!"))
            {
                TempData["Success"] = resultMessage;
                return RedirectToAction("Index", new { page = page });
            }

            ModelState.AddModelError("", resultMessage);
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id, int page = 1)
        {
            var resultMessage = _productService.Delete(id);

            if (resultMessage.Contains("Xoa san pham thanh cong!"))
            {
                TempData["Success"] = resultMessage;
            }
            else
            {
                TempData["Error"] = resultMessage;
            }

            return RedirectToAction("Index", new { page = page });
        }
    }
}
