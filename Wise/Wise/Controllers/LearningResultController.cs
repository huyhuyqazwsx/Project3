using Microsoft.AspNetCore.Mvc;
using Wise.Application.DTOs.Learning;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LearningResultController : ControllerBase
    {
        private readonly ILearningResultService _service;

        public LearningResultController(ILearningResultService service)
        {
            _service = service;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] LearningSubmitDto dto)
        {
            var result = await _service.SubmitAsync(dto);
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var results = await _service.GetByUserAsync(userId);
            return Ok(results);
        }
    }
}
