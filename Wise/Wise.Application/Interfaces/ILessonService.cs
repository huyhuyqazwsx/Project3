using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Lesson;
using Wise.Domain.Entities;
using Wise.Domain.Enums;

namespace Wise.Application.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task<IEnumerable<ResponseLessonDto>> GetListWithCategoryId(int catId);
        Task<Lesson?> GetByIdAsync(int id);
        Task<Lesson> CreateLessonAsync(Lesson model);
        Task<Lesson?> UpdateLessonAsync(int id, Lesson model);
        Task DeleteLessonAsync(int id);
        Task<IEnumerable<Vocabulary>> GetVocabularyWithIdLessonAsync(int lessonId);

    }
}
