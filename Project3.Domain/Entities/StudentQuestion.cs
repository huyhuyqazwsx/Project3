using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Domain.Entities
{
    public class StudentQuestion
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // Student answer
        public string? Answer { get; set; }
        public float? Result { get; set; }
        public int? TimeSpent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User? Student { get; set; }
        public QuestionExam? QuestionExam { get; set; }
    }
}
