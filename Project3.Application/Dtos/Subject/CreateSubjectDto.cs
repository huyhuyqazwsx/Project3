using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Subject
{
    public class CreateSubjectDto
    {
        public required string Name { get; set; }
        public required string SubjectCode { get; set; }
        public int TotalChapters { get; set; }  // Số chương trong môn học
    }

    public class ResponseSubjectDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string SubjectCode { get; set; }
        public int TotalChapters { get; set; }
    }

    public class UpdateSubjectDto
    {
        public required string Name { get; set; }
        public required string SubjectCode { get; set; }
        public int TotalChapters { get; set; }
    }
}
