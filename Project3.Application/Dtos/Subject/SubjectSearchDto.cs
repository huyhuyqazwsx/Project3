using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Subject
{
    public class SubjectSearchDto
    {
        public string? Keyword { get; set; }        // tìm theo tên / mã
        public int? MinChapters { get; set; }
        public int? MaxChapters { get; set; }

        public string SortBy { get; set; } = "name"; // name | code | chapters
        public bool Desc { get; set; } = false;
    }
}
