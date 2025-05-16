using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVC.Controllers.Api
{
    [ApiController]
    [Route("api/pets")]
    public class PetApiController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetApiController> _logger;

        public PetApiController(IPetService petService, ILogger<PetApiController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            try
            {
                var pets = _petService.GetAll();
                var totalItems = pets.Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var petsOnPage = pets
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    Data = petsOnPage,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thú cưng.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }
        [HttpGet("/api/pets/available")]
        public IActionResult GetPetAvailable()
        {
            var products = _petService.GetAll()
            .Select(p => new
            {
                p.PetId,
                p.Status,
                p.PetName,
                Name = $"{p.PetId} - {p.PetName} - {p.Status}"

            })
            .ToList();
            return Ok(products);

        }

        [HttpGet("search")]
        public IActionResult Search(
            string keyword = "",
            int page = 1,
            int pageSize = 5,
            bool onlyAvailable = false,
            int? minPrice = null,
            int? maxPrice = null
        )
        {
            try
            {
                var filtered = _petService.SearchPets(keyword ?? "");

                // Lọc theo tình trạng: chỉ lấy những con chưa bán nếu checkbox được bật
                if (onlyAvailable)
                {
                    filtered = filtered
                        .Where(p => !string.Equals(p.Status, "Đã bán", StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Lọc theo giá tối thiểu
                if (minPrice.HasValue)
                {
                    filtered = filtered
                        .Where(p => p.Price >= minPrice.Value)
                        .ToList();
                }

                // Lọc theo giá tối đa
                if (maxPrice.HasValue)
                {
                    filtered = filtered
                        .Where(p => p.Price <= maxPrice.Value)
                        .ToList();
                }

                // Sắp xếp theo PetId
                filtered = filtered.OrderBy(p => p.PetId).ToList();

                // Phân trang
                var totalItems = filtered.Count;
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var petsOnPage = filtered
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    Data = petsOnPage,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm thú cưng.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var pet = _petService.GetById(id);
                if (pet == null) return NotFound(new { Message = "Không tìm thấy thú cưng." });

                return Ok(pet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin thú cưng theo ID.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpPost]
        public IActionResult Create([FromForm] Pet pet, [FromForm] IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            byte[]? imageData = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                if (imageFile.Length > 5 * 1024 * 1024)
                    return BadRequest(new { Message = "Kích thước ảnh quá lớn. Tối đa 5MB." });

                using var ms = new MemoryStream();
                imageFile.CopyTo(ms);
                imageData = ms.ToArray();
            }

            try
            {
                var message = _petService.Add(pet, imageData);
                if (message.Contains("Them thu cung thanh cong!"))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thú cưng.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] Pet pet, [FromForm] IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            pet.PetId = id;

            byte[]? imageData = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                if (imageFile.Length > 5 * 1024 * 1024)
                    return BadRequest(new { Message = "Kích thước ảnh quá lớn. Tối đa 5MB." });

                using var ms = new MemoryStream();
                imageFile.CopyTo(ms);
                imageData = ms.ToArray();
            }

            try
            {
                var message = _petService.Update(pet, imageData);
                if (message.Contains("Cap nhat thong tin thu cung thanh cong!"))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thú cưng.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var message = _petService.Delete(id);
                if (message.Contains("Xoa thu cung thanh cong!"))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thú cưng.");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }
    }
}
