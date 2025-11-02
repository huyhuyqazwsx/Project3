using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize(Roles = "Admin")]
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

        [HttpGet]
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
    }
}
