using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.IO;
using System.Linq;

namespace MVC.Controllers
{
    public class PetController : Controller
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet("/Thucung")]
        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allPets = _petService.GetAll(); 
            int totalItems = allPets.Count;
            int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var petsOnPage = allPets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PetListViewModel
            {
                Pets = petsOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("Thucung", new { page = p })!
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult LoadPetsPartial(
                        string keyword = "",
                        int page = 1,
                        int pageSize = 5,
                        bool onlyAvailable = false,
                        int? minPrice = null,
                        int? maxPrice = null)
        {
            try
            {
                var filteredPets = _petService.SearchPets(keyword);

                // Lọc theo trạng thái "chưa bán"
                if (onlyAvailable)
                {
                    filteredPets = filteredPets.Where(p => p.Status != "Đã bán").ToList();
                }

                // Lọc theo khoảng giá
                if (minPrice.HasValue)
                {
                    filteredPets = filteredPets.Where(p => p.Price >= minPrice.Value).ToList();
                }
                if (maxPrice.HasValue)
                {
                    filteredPets = filteredPets.Where(p => p.Price <= maxPrice.Value).ToList();
                }

                // Sắp xếp theo ID
                filteredPets = filteredPets.OrderBy(p => p.PetId).ToList();


                int totalItems = filteredPets.Count;
                int countPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var petsOnPage = filteredPets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var model = new PetListViewModel
                {
                    Pets = petsOnPage,
                    CurrentPage = page,
                    CountPages = countPages,
                    GenerateUrl = p => Url.Action("LoadPetsPartial", new { keyword = keyword, page = p, onlyAvailable = onlyAvailable, minPrice, maxPrice })!
                };

                return PartialView("~/Views/Pet/_PetListPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }


        [HttpGet]
        public IActionResult CreatePets() => View();

        [HttpPost]
        public IActionResult CreatePets(Pet pet, IFormFile imageFile)
        {
            if (!ModelState.IsValid) return View(pet);

            byte[] imageData = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                imageFile.CopyTo(ms);
                imageData = ms.ToArray();
            }

            string message = _petService.Add(pet, imageData); // Gọi đúng hàm Add(pet, image)

            if (message.Contains("Them thu cung thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", message);
            return View(pet);
        }

        [HttpPost]
        public IActionResult DeletePet(int id, int page = 1)
        {
            string message = _petService.Delete(id); // Gọi đúng hàm Delete(id)

            if (message.Contains("Xoa thu cung thanh cong!"))
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Index", new { page });
        }

        [HttpGet]
        public IActionResult EditPet(int id)
        {
            var pet = _petService.GetById(id); // Gọi đúng hàm GetById
            if (pet == null)
                return NotFound();

            return View(pet);
        }

        [HttpPost]
        public IActionResult EditPet(Pet pet, IFormFile? imageFile, int page = 1)
        {
            if (!ModelState.IsValid)
                return View(pet);

            byte[]? imageData = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                imageFile.CopyTo(ms);
                imageData = ms.ToArray();
            }

            string message = _petService.Update(pet, imageData); // Gọi đúng hàm Update(pet, image)

            if (message.Contains("Cap nhat thong tin thu cung thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index", new { page });
            }

            ModelState.AddModelError("", message);
            return View(pet);
        }
    }
}
