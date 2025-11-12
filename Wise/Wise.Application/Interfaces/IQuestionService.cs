using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Question;
using Wise.Domain.Entities;

namespace Wise.Application.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(int id);
        Task<IEnumerable<Question>> GetByLessonIdAsync(int lessonId);
        Task<Question> CreateAsync(CreateQuestionDto dto);
        Task<Question?> UpdateAsync(int id, CreateQuestionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId);
    }
}
