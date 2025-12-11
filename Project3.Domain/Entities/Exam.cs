using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int DurationMinutes { get; set; } = 30;

        public int? BlueprintId { get; set; }
        public ExamBlueprint? Blueprint { get; set; }

        public int ClassId { get; set; }
        public Class? Class { get; set; }

        public ICollection<QuestionExam> QuestionExams { get; set; } = new List<QuestionExam>();
        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
    }
}
