using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Domain.Entities
{
    public class LearningDetail
    {
        public int Id { get; set; }
        public int LearningResultId { get; set; }

        public int QuestionId { get; set; }
        public int? AnswerId { get; set; }

        public bool IsCorrect { get; set; }
        public double ResponseTime { get; set; }

        // metadata để AI phân tích
        public SkillType Skill { get; set; }
        public string? Topic { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public LearningResult? LearningResult { get; set; }
        public Question? Question { get; set; }
        public Answer? Answer { get; set; }
    }

}
