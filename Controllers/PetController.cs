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
            var allPets = _petService.GetAllPets();
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

        [HttpGet("/Pet/LoadPetsPartial")]
        public IActionResult LoadPetsPartial(string keyword = "", int page = 1, int pageSize = 5)
        {
            var pets = _petService.SearchPets(keyword);
            int countPages = (int)Math.Ceiling(pets.Count / (double)pageSize);
            var petsOnPage = pets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PetListViewModel
            {
                Pets = petsOnPage,
                CurrentPage = page,
                CountPages = countPages,
                GenerateUrl = p => Url.Action("LoadPetsPartial", new { keyword, page = p })!
            };

            return PartialView("~/Views/Pet/_PetListPartial.cshtml", model);
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

            // Call the service method to add the pet with the image
            string message = _petService.AddPet(pet, imageData);

            if (message.Contains("Them thu cung thanh cong!"))
            {
                TempData["Success"] = message;
                return RedirectToAction("Index");
            }

            // If there is an error, add it to ModelState
            ModelState.AddModelError("", message);
            return View(pet);
        }

        [HttpPost]
        public IActionResult DeletePet(int id, int page = 1)
        {
            string message = _petService.DeletePet(id);
            if (message.Contains("Xoa thu cung thanh cong!"))
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction("Index", new { page });
        }

        [HttpGet]
        public IActionResult EditPet(int id)
        {
            var pet = _petService.GetPetById(id);
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

            // Gọi phương thức cập nhật - truyền null nếu không có ảnh mới
            string message = _petService.UpdatePet(pet, imageData);

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
