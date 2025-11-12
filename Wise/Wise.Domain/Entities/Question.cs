using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Domain.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string Text { get; set; } = "";
        public QuestionType Type { get; set; } = QuestionType.TrueFalse; 
        public string? ImageUrl { get; set; } = string.Empty;
        public string? AudioUrl { get; set; } = string.Empty;
        public string? Paragraph { get; set; }
        public int OrderIndex { get; set; } = 0;

        // Navigation
        public Lesson? Lesson { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
