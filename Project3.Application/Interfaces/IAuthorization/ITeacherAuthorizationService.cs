using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces.IAuthorization
{
    public interface ITeacherAuthorizationService
    {
        Task<bool> CanAccessClassAsync(int classId);
        Task<bool> CanAccessExamAsync(int examId);
    }
}
