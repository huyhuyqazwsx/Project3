using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Class
{
    namespace Project3.Application.Dtos.Class
    {
        public class RequestUpdateClass
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int SubjectId { get; set; }
            public int TeacherId { get; set; }
        }
    }
}
