using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Wise.Application.DTOs.Lesson;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILessonCategoryService _lessonCategoryService;

        public LessonController(
            ILessonService lessonService,
            ILessonCategoryService lessonCategoryService)
        {
            _lessonService = lessonService;
            _lessonCategoryService = lessonCategoryService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _lessonService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetLessonWithCateGory(int categoryId)
        {
            var exists = await _lessonCategoryService.GetByIdAsync(categoryId);
            if (exists == null)
                return NotFound("Category không tồn tại");

            var list = await _lessonService.GetListWithCategoryId(categoryId);
            return Ok(list);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLesson([FromBody] RequestLessonDto dto)
        {
            var category = await _lessonCategoryService.GetByIdAsync(dto.CategoryId);
            if (category == null)
                return BadRequest("CategoryId không hợp lệ");

            var lesson = await _lessonService.CreateLessonAsync(dto);
            return Ok(lesson);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File JSON trống hoặc không tồn tại.");

            using var stream = new StreamReader(file.OpenReadStream());
            var jsonContent = await stream.ReadToEndAsync();

            List<RequestLessonDto>? lessonList;

            try
            {
                lessonList = JsonSerializer.Deserialize<List<RequestLessonDto>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Không đọc được file JSON: {ex.Message}");
            }

            if (lessonList == null || lessonList.Count == 0)
                return BadRequest("File JSON không chứa dữ liệu hợp lệ.");

            // Validate CategoryId
            foreach (var dto in lessonList)
            {
                var category = await _lessonCategoryService.GetByIdAsync(dto.CategoryId);
                if (category == null)
                    return BadRequest($"CategoryId {dto.CategoryId} không hợp lệ trong file JSON.");
            }

            foreach (var lesson in lessonList)
            {
                await _lessonService.CreateLessonAsync(lesson);
            }

            return Ok(new { message = $"Đã import {lessonList.Count} bài học thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _lessonService.DeleteLessonAsync(id);
            return Ok(new { message = "Đã xoá bài học" });
        }
    }
}
