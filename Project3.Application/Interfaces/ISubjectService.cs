using Project3.Application.Dtos.Subject;
using Project3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface ISubjectService : ICrudService<Subject>
    {
        Task<bool> AddListSubject(CreateSubjectDto[] dto);
        Task<Subject?> GetByCodeAsync(String code);
    }
}
