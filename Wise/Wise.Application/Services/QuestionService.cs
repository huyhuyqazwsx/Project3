using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Question;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _repoQuestion;
        private readonly IRepository<Answer> _repoAnswer;
        public QuestionService(IRepository<Question> repoQuestion, IRepository<Answer> repoAnswer) {
            _repoQuestion = repoQuestion;
            _repoAnswer = repoAnswer;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _repoQuestion.Query()
                .Include(t => t.Answers)
                .AsNoTracking()
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _repoQuestion.Query()
                .Include(t => t.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Question>> GetByLessonIdAsync(int lessonId)
        {
            return await _repoQuestion.Query()
                .Where(q => q.LessonId == lessonId)
                .Include(t => t.Answers)
                .AsNoTracking()
                .OrderBy(q => q.OrderIndex)
                .ToListAsync();
        }

        public async Task<Question> CreateAsync(CreateQuestionDto dto)
        {
            var question = new Question
            {
                LessonId = dto.LessonId,
                Text = dto.Text,
                Type = dto.Type,
                ImageUrl = dto.ImageUrl,
                AudioUrl = dto.AudioUrl,
                Paragraph = dto.Paragraph,
                OrderIndex = dto.OrderIndex,
                Topic = dto.Topic,
                Difficulty = dto.Difficulty,
                Skill = dto.Skill,
                Answers = dto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                }).ToList()
            };

            await _repoQuestion.AddAsync(question);
            await _repoQuestion.SaveChangesAsync();
            return question;
        }

        public async Task<Question?> UpdateAsync(int id, CreateQuestionDto dto)
        {
            var exis = await _repoQuestion.Query()
                .Include(t => t.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (exis == null) return null;

            exis.LessonId = dto.LessonId;
            exis.Type = dto.Type;
            exis.Text = dto.Text;
            exis.ImageUrl = dto.ImageUrl;
            exis.AudioUrl = dto.AudioUrl;
            exis.Paragraph = dto.Paragraph;
            exis.OrderIndex = dto.OrderIndex;

            exis.Answers.Clear();

            foreach (var answer in dto.Answers)
            {
                exis.Answers.Add(new Answer
                {
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect,
                });
            }
            _repoQuestion.Update(exis);
            await _repoQuestion.SaveChangesAsync();
            return exis;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exis = await _repoQuestion.GetByIdAsync(id);
            if (exis == null) return false;
            
            _repoQuestion.Delete(exis);
            await _repoQuestion.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _repoAnswer.Query()
                .Where(a => a.QuestionId == questionId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
