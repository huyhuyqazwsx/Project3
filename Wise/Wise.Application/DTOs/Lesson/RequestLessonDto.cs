using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Application.DTOs.Lesson
{
    public class RequestLessonDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public LessonType Type { get; set; }
        public SkillType Skill { get; set; }
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;


        public int Level { get; set; } = 1;
        public int CategoryId { get; set; }
    }
}
