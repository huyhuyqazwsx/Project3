using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Application.DTOs.Question
{
    public class CreateQuestionDto
    {
        public int LessonId { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; } = QuestionType.TrueFalse;
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
        public string? Paragraph { get; set; }
        public int OrderIndex { get; set; } = 0;

        public SkillType Skill { get; set; }
        public string? Topic { get; set; }
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;

        // Danh sách đáp án
        public List<CreateAnswerDto> Answers { get; set; } = new();
    }

    public class CreateAnswerDto
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
