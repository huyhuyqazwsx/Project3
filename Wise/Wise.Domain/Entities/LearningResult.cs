using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Domain.Entities
{
    public class LearningResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int LessonId { get; set; }

        public double Score { get; set; }
        public double Accuracy { get; set; }
        public int TimeSpent { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.Now;

        // Navigation
        public User? User { get; set; }
        public Lesson? Lesson { get; set; }
        public ICollection<LearningDetail> Details { get; set; } = new List<LearningDetail>();
    }

}
