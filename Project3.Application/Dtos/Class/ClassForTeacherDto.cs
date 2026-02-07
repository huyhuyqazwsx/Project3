using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Class
{
    public class ClassForTeacherDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
    }
}
