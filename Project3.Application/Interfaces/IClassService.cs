using Project3.Application.Dtos.Class;
using Project3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface IClassService : ICrudService<Class>
    {
        Task<AddStudentsResult> AddStudentsAsync(int classId, List<int> studentIds);
        Task<RemoveStudentsResult> RemoveStudentsAsync(int classId, List<int> studentIds);
        Task<List<ResponseStudentInClassDto>> GetStudentsInClassAsync(int classId);
        Task<List<ClassForStudentDto>> GetClassesForStudentAsync(int studentId);
        Task<List<ClassForTeacherDto>> GetClassesForTeacherAsync(int teacherId);

    }
}
