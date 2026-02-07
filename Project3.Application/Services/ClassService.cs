using Microsoft.EntityFrameworkCore;
using Project3.Application.Dtos.Class;
using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Interfaces;
using Project3.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
   
    public class ClassService : CrudService<Class>, IClassService
    {
        private readonly IRepository<StudentClass> _studentClassRepo;
        public ClassService(
            IRepository<Class> repository,
            IRepository<StudentClass> studentClassRepo
            ) : base(repository)
        {
            _studentClassRepo = studentClassRepo;
        }

        public async Task<AddStudentsResult> AddStudentsAsync(int classId, List<int> studentIds)
        {
            var classEntity = await _repository
                .Query()
                .Include(c => c.StudentClasses)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null)
                throw new Exception("Không tìm thấy lớp học");

            var existingIds = classEntity.StudentClasses
                .Select(sc => sc.StudentId)
                .ToHashSet();

            var newIds = studentIds
                .Distinct()
                .Where(id => !existingIds.Contains(id))
                .ToList();

            foreach (var id in newIds)
            {
                classEntity.StudentClasses.Add(new StudentClass
                {
                    ClassId = classId,
                    StudentId = id
                });
            }

            await this.UpdateAsync(classEntity);

            return new AddStudentsResult
            {
                Added = newIds.Count,
                AlreadyExists = studentIds.Count - newIds.Count
            };
        }

        public async Task<RemoveStudentsResult> RemoveStudentsAsync(int classId, List<int> studentIds)
        {
            var classEntity = await _repository
                .Query()
                .Include(c => c.StudentClasses)
                .FirstOrDefaultAsync(c => c.Id == classId);

            if (classEntity == null)
                throw new Exception("Không tìm thấy lớp học");

            var removeSet = studentIds.ToHashSet();

            var toRemove = classEntity.StudentClasses
                .Where(sc => removeSet.Contains(sc.StudentId))
                .ToList();

            foreach (var sc in toRemove)
            {
                classEntity.StudentClasses.Remove(sc);
            }

            await this.UpdateAsync(classEntity);

            return new RemoveStudentsResult
            {
                Removed = toRemove.Count
            };
        }

        public async Task<List<ResponseStudentInClassDto>> GetStudentsInClassAsync(int classId)
        {
            return await _repository.Query()
                .Where(c => c.Id == classId)
                .SelectMany(c => c.StudentClasses)
                .Where(sc => sc.Student != null)
                .Select(sc => new ResponseStudentInClassDto
                {
                    StudentId = sc.Student!.Id,
                    FullName = sc.Student.FullName,
                    MSSV = sc.Student.MSSV,
                    Email = sc.Student.Email,
                    Role = sc.Student.Role
                })
                .ToListAsync();
        }

        public async Task<List<ClassForStudentDto>> GetClassesForStudentAsync(int studentId)
        {
            return await _studentClassRepo
                .Query()
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Class)
                    .ThenInclude(c => c.Subject)
                .Include(sc => sc.Class)
                    .ThenInclude(c => c.Teacher)
                .Select(sc => new ClassForStudentDto
                {
                    ClassId = sc.Class!.Id,
                    ClassName = sc.Class.Name,

                    SubjectId = sc.Class.Subject!.Id,
                    SubjectName = sc.Class.Subject.Name,
                    SubjectCode = sc.Class.Subject.SubjectCode,

                    TeacherId = sc.Class.Teacher!.Id,
                    TeacherName = sc.Class.Teacher.FullName
                })
                .ToListAsync();
        }

        public async Task<List<ClassForTeacherDto>> GetClassesForTeacherAsync(int teacherId)
        {
            return await _repository
                .Query()
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.Subject)
                .Select(c => new ClassForTeacherDto
                {
                    ClassId = c.Id,
                    ClassName = c.Name,

                    SubjectId = c.Subject!.Id,
                    SubjectName = c.Subject.Name,
                    SubjectCode = c.Subject.SubjectCode
                })
                .ToListAsync();
        }

    }
}
