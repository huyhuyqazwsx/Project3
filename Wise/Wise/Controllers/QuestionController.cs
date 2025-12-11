using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wise.Application.DTOs.Lesson;
using Wise.Application.DTOs.Question;
using Wise.Application.Interfaces;
using Wise.Application.Services;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _questionService.GetAllAsync();
            return Ok(questions);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto)
        {
            var question = await _questionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateQuestionDto dto)
        {
            var question = await _questionService.UpdateAsync(id, dto);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _questionService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("get-answers/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await _questionService.GetAnswersByQuestionIdAsync(questionId);
            return Ok(answers);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File JSON trống hoặc không tồn tại.");

            using var stream = new StreamReader(file.OpenReadStream());
            var jsonContent = await stream.ReadToEndAsync();

            List<CreateQuestionDto>? quesList;
            try
            {
                quesList = JsonSerializer.Deserialize<List<CreateQuestionDto>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Không đọc được file JSON: {ex.Message}");
            }

            if (quesList == null || quesList.Count == 0)
                return BadRequest("Không có dữ liệu hợp lệ trong file JSON.");

            foreach (var ques in quesList)
            {
                await _questionService.CreateAsync(ques);
            }

            return Ok(new { message = $"Đã import {quesList.Count} question thành công." });
        }
    }
}
