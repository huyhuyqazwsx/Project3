using Microsoft.AspNetCore.Mvc;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaknessAnalysisController : ControllerBase
    {
        private readonly IWeaknessAnalysisService _analysisService;

        public WeaknessAnalysisController(IWeaknessAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> Analyze(int userId)
        {
            var result = await _analysisService.AnalyzeAsync(userId);
            return Ok(result);
        }
    }
}
