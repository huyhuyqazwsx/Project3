using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
    public class ClassService : CrudService<Class>, IClassService
    {
        public ClassService(IRepository<Class> repository
            ) : base(repository)
        {
        }
    }
}
