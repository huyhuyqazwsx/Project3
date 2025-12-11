using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;
using static Wise.Application.DTOs.Vocabulary.VocabularyDto;

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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vocabService.GetAllAsync();
            return Ok(result);

        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] VocabularyRequestDto dto)
        {
            if (dto == null) return BadRequest("Lỗi dto");
            var voca = new Vocabulary
            {
                LessonId = dto.LessonId,
                Word = dto.Word,
                Synonym = dto.Synonym,
                PartOfSpeech = dto.PartOfSpeech,
                Transcription = dto.Transcription,
                AudioUrl = dto.AudioUrl,
                ImageUrl = dto.ImageUrl,
                Meaning = dto.Meaning,
                Example = dto.Example
            };

            await _vocabService.CreateAsync(voca);
            return Ok(voca);
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
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
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
