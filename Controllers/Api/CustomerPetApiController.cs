using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers.Api
{
    [ApiController]
    [Route("api/customer-pets")]
    public class CustomerPetApiController : ControllerBase
    {
        private readonly ICustomerPetService _customerPetService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ILogger<CustomerPetApiController> _logger;

        public CustomerPetApiController(
            ICustomerPetService customerPetService,
            ICustomerService customerService,
            IProductService productService,
            ILogger<CustomerPetApiController> logger)
        {
            _customerPetService = customerPetService;
            _customerService = customerService;
            _productService = productService;
            _logger = logger;
        }

        
        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 5)
        {
            try
            {
                var all = _customerPetService.GetAll();
                int totalItems = all.Count;
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var result = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    Data = result,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpGet("search")]
        public IActionResult Search(string keyword = "", int page = 1, int pageSize = 5, bool onlyAvailable = false)
        {
            try
            {
                var filtered = _customerPetService.SearchCustomerPet(keyword ?? "").OrderBy(p => p.CustomerPetId).ToList();

                if (onlyAvailable)
                {
                    filtered = filtered.Where(p => p.Status == "Hoàn thành").ToList();
                }

                int totalItems = filtered.Count;
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                var result = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new
                {
                    Data = result,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tìm kiếm CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var entity = _customerPetService.GetById(id);
                if (entity == null)
                    return NotFound(new { Message = "Không tìm thấy dịch vụ." });

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy theo ID");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerPet customerPet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var message = _customerPetService.Add(customerPet);
                if (message.Contains("Them thong tin dich vu thanh cong!", StringComparison.OrdinalIgnoreCase))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CustomerPet customerPet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            customerPet.CustomerPetId = id;

            try
            {
                var message = _customerPetService.Update(customerPet);
                if (message.Contains("Cap nhat thong tin thanh cong!", StringComparison.OrdinalIgnoreCase))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var message = _customerPetService.Delete(id);
                if (message.Contains("Xoa thong tin dich vu thanh cong!", StringComparison.OrdinalIgnoreCase))
                    return Ok(new { Message = message });

                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }

        [HttpPost("toggle-status")]
        public IActionResult ToggleStatus([FromBody] int id)
        {
            try
            {
                var result = _customerPetService.ToggleStatus(id);
                if (result == null)
                    return NotFound(new { Message = "Không tìm thấy dịch vụ." });

                return Ok(new { Success = true, NewStatus = result, ID = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đổi trạng thái CustomerPet");
                return StatusCode(500, new { Message = "Lỗi máy chủ nội bộ." });
            }
        }
    }
}
