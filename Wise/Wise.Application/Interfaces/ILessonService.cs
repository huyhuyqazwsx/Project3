using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Entities;
using Wise.Domain.Enums;

namespace Wise.Application.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task<IEnumerable<Lesson>> GetListWithLessonType(LessonType lessonType);
        Task<Lesson?> GetByIdAsync(int id);
        Task<Lesson> CreateLessonAsync(Lesson model);
        Task<Lesson?> UpdateLessonAsync(int id, Lesson model);
        Task DeleteLessonAsync(int id);
        Task<IEnumerable<Vocabulary>> GetVocabularyWithIdLessonAsync(int lessonId);

    }
}
