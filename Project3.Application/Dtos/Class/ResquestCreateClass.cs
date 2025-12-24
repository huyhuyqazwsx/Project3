using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Class
{
    public class ResquestCreateClass
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
