using Project3.Application.Dtos.Question;
using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
    public class QuestionService : CrudService<Question>, IQuestionService
    {
        public QuestionService(IRepository<Question> repo)
        : base(repo)
        {
        }

        public async Task<bool> AddListQuestion(CreateQuestionDto[] questionDtos)
        {
            var entities = questionDtos.Select(dto => new Question
            {
                Content = dto.Content,
                Answer = dto.Answer,
                Point = dto.Point,
                Difficulty = dto.Difficulty,
                Type = dto.Type,
                SubjectId = dto.SubjectId,
                Chapter = dto.Chapter
            }).ToList();

            await _repository.AddRangeAsync(entities);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
