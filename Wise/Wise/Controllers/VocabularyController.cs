using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class VocabularyController : ControllerBase
    {
        private readonly IVocabularyService _vocabService;

        public VocabularyController(IVocabularyService vocabService)
        {
            _vocabService = vocabService;
        }

        [HttpPost("import-vocabulary")]
        public async Task<IActionResult> ImportFromJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File JSON trống hoặc không tồn tại.");

            using var stream = new StreamReader(file.OpenReadStream());
            var jsonContent = await stream.ReadToEndAsync();

            List<Vocabulary>? vocabList;
            try
            {
                vocabList = JsonSerializer.Deserialize<List<Vocabulary>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Không đọc được file JSON: {ex.Message}");
            }

            if (vocabList == null || vocabList.Count == 0)
                return BadRequest("Không có dữ liệu hợp lệ trong file JSON.");

            foreach (var vocab in vocabList)
            {
                await _vocabService.CreateAsync(vocab);
            }

            return Ok(new { message = $"Đã import {vocabList.Count} từ vựng thành công." });
        }

    }
}
