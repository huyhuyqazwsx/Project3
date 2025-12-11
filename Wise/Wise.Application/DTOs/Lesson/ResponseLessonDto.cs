using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wise.Application.DTOs.Lesson
{
    public class ResponseLessonDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int OrderIndex { get; set; }
        public int CategoryId { get; set; }
    }

}
