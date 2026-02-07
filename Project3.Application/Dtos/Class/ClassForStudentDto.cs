using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Class
{
    public class ClassForStudentDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public string SubjectCode { get; set; } = "";
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = "";
    }
}
