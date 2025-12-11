using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wise.Application.DTOs.LessonCategory;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonCategoryController : ControllerBase
    {
        private readonly ILessonCategoryService _lessonCategoryService;
        public LessonCategoryController(ILessonCategoryService lessonCategoryService)
        {
            _lessonCategoryService = lessonCategoryService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCateGory([FromBody] LessonCategoryDto dto)
        {
            try
            {
                await _lessonCategoryService.CreateCategoryAsync(dto);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategory(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File JSON trống hoặc không tồn tại.");

            using var stream = new StreamReader(file.OpenReadStream());
            var jsonContent = await stream.ReadToEndAsync();

            List<LessonCategoryDto>? catList;
            try
            {
                catList = JsonSerializer.Deserialize<List<LessonCategoryDto>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Không đọc được file JSON: {ex.Message}");
            }

            if (catList == null || catList.Count == 0)
                return BadRequest("Không có dữ liệu hợp lệ trong file JSON.");

            foreach (var cat in catList)
            {

                await _lessonCategoryService.CreateCategoryAsync(cat);
            }

            return Ok(new { message = $"Đã import {catList.Count} mục thành công." });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cat = await _lessonCategoryService.GetAllAsync();
                return Ok(cat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id , [FromBody] LessonCategoryDto dto)
        {
            try
            {
                var check = await _lessonCategoryService.GetByIdAsync(id);
                if (check == null) return BadRequest("Không tồn tại");

                check.Description = dto.Description;
                check.Name = dto.Name;
                check.ImageUrl = dto.ImageUrl;

                await _lessonCategoryService.UpdateCategoryAsync(id, check);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _lessonCategoryService.DeleteCategoryAsync(id);
            return Ok();
        }

    }
}
