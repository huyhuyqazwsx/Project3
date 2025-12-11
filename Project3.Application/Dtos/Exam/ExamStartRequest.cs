using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Exam
{
    public class ExamStartRequest
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
    }
}
