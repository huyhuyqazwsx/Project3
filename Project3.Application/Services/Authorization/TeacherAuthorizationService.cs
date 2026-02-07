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
    public class TeacherAuthorizationService : ITeacherAuthorizationService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IRepository<Class> _classRepo;
        private readonly IRepository<Exam> _examRepo;

        public TeacherAuthorizationService(
            ICurrentUserService currentUser,
            IRepository<Class> classRepo,
            IRepository<Exam> examRepo)
        {
            _currentUser = currentUser;
            _classRepo = classRepo;
            _examRepo = examRepo;
        }

        public async Task<bool> CanAccessClassAsync(int classId)
        {
            // Admin toàn quyền
            if (_currentUser.Role == "Admin")
                return true;

            // Chỉ teacher
            if (_currentUser.Role != "Teacher")
                return false;

            var userId = _currentUser.UserId;

            return await _classRepo
                .Query()
                .AnyAsync(c =>
                    c.Id == classId &&
                    c.TeacherId == userId
                );
        }

        public async Task<bool> CanAccessExamAsync(int examId)
        {
            // Admin toàn quyền
            if (_currentUser.Role == "Admin")
                return true;

            // Chỉ teacher
            if (_currentUser.Role != "Teacher")
                return false;

            var userId = _currentUser.UserId;

            var classId = await _examRepo
                .Query()
                .Where(e => e.Id == examId)
                .Select(e => e.ClassId)
                .FirstOrDefaultAsync();

            if (classId == 0)
                return false;

            return await _classRepo
                .Query()
                .AnyAsync(c =>
                    c.Id == classId &&
                    c.TeacherId == userId
                );
        }
    }
}
