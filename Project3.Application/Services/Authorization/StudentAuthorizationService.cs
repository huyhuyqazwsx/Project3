using Microsoft.EntityFrameworkCore;
using Project3.Application.Interfaces.IAuthorization;
using Project3.Domain.Entities;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services.Authorization
{
    public class StudentAuthorizationService : IStudentAuthorizationService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IRepository<Exam> _examRepo;
        private readonly IRepository<StudentClass> _studentClassRepo;

        public StudentAuthorizationService(
            ICurrentUserService currentUser,
            IRepository<Exam> examRepo,
            IRepository<StudentClass> studentClassRepo)
        {
            _currentUser = currentUser;
            _examRepo = examRepo;
            _studentClassRepo = studentClassRepo;
        }

        public async Task<bool> CanAccessExamAsync(int examId)
        {
            // Admin được truy cập tất cả
            if (_currentUser.Role == "Admin")
                return true;

            // Chỉ sinh viên
            if (_currentUser.Role != "Student")
                return false;

            var userId = _currentUser.UserId;

            // Lấy classId của exam
            var classId = await _examRepo
                .Query()
                .Where(e => e.Id == examId)
                .Select(e => e.ClassId)
                .FirstOrDefaultAsync();

            if (classId == 0)
                return false;

            // Kiểm tra sinh viên có thuộc lớp đó không
            return await _studentClassRepo
                .Query()
                .AnyAsync(sc =>
                    sc.ClassId == classId &&
                    sc.StudentId == userId
                );
        }
    }
}
