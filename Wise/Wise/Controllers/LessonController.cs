using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        public readonly ILessonCategoryService _lessonCategoryService;
        public LessonController(ILessonService lessonService, ILessonCategoryService lessonCategoryService)
        {
            _lessonService = lessonService;
            _lessonCategoryService = lessonCategoryService;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetLessonWithCateGory(int categoryId)
        {
            try
            {
                var exis = await _lessonCategoryService.GetByIdAsync(categoryId);
                if (exis == null) return NotFound();

                var list = await _lessonService.GetListWithCategoryId(categoryId);

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
