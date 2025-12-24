using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Class
{
    public class UpdateStudentsInClassDto   
    {
        public int ClassId { get; set; }
        public List<int> StudentIds { get; set; } = new();
    }
}
