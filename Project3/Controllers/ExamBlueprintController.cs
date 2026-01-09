using Microsoft.AspNetCore.Mvc;
using Project3.Application.Dtos.ExamBlueprint;
using Project3.Application.Interfaces;

namespace Project3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamBlueprintController : ControllerBase
    {
        private readonly IExamBlueprintService _examBlueprintService;

        public ExamBlueprintController(IExamBlueprintService examBlueprintService)
        {
            _examBlueprintService = examBlueprintService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateExamBlueprintDto dto)
        {
            try
            {
                var result = await _examBlueprintService.CreateBlueprintAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _examBlueprintService.GetAllAsync();
            return Ok(list.Select(b => new
            {
                b.Id,
                b.SubjectId,
                b.CreatedAt
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
                var result = await _examBlueprintService.GetDetailAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateExamBlueprintDto dto)
        {
            try
            {
                var result = await _examBlueprintService.UpdateBlueprintAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _examBlueprintService.DeleteAsync(id);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("by-class/{classId}")]
        public async Task<IActionResult> GetByClass(int classId)
        {
            try
            {
                var result = await _examBlueprintService.GetExamsWithBlueprintByClassAsync(classId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
