using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Application.DTOs.Learning
{
    public class LearningDetailSubmitDto
    {
        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public bool IsCorrect { get; set; }
        public double ResponseTime { get; set; }
        public SkillType Skill { get; set; }
        public string? Topic { get; set; }
    }

    public class LearningSubmitDto
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public List<LearningDetailSubmitDto> Details { get; set; } = new();
    }
}
