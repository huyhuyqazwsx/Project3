using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Entities;

namespace Wise.Application.Interfaces
{
    public interface IVocabularyService
    {
        Task<IEnumerable<Vocabulary>> GetAllAsync();
        Task<Vocabulary?> GetByIdAsync(int id);
        Task<IEnumerable<Vocabulary>> GetByLessonIdAsync(int lessonId);
        Task<Vocabulary> CreateAsync(Vocabulary model);
        Task<Vocabulary?> UpdateAsync(int id, Vocabulary model);
        Task<bool> DeleteAsync(int id);
    }
}
