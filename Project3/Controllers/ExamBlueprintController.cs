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
            var result = await _examBlueprintService.CreateBlueprintAsync(dto);
            return Ok(result);
        }
    }
}
