using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Lesson;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;
using Wise.Domain.Enums;

namespace Wise.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IRepository<Lesson> _repository;
        private readonly IRepository<Vocabulary> _repository1;
        public LessonService(IRepository<Lesson> repository , IRepository<Vocabulary> repository1)
        {
            _repository = repository;
            _repository1 = repository1;
        }
        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<ResponseLessonDto>> GetListWithCategoryId(int catId)
        {
            return await _repository.Query()
                .Where(l => l.CategoryId == catId)
                .AsNoTracking()
                .OrderBy(l => l.Title)
                .Select(l => new ResponseLessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    ImageUrl = l.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _repository.Query()
                .Include(l => l.Vocabularies)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lesson> CreateLessonAsync(Lesson model)
        {
            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();
            return model;
        }

        public async Task<Lesson?> UpdateLessonAsync(int id, Lesson model)
        {
            var exis = await _repository.GetByIdAsync(id);
            if (exis == null) return null;

            exis.Title = model.Title;
            exis.Description = model.Description;
            exis.ImageUrl = model.ImageUrl;
            exis.Type = model.Type;
            exis.Skill = model.Skill;
            exis.Difficulty = model.Difficulty;
            exis.Level = model.Level;
            exis.CategoryId = model.CategoryId;

            _repository.Update(exis);
            await _repository.SaveChangesAsync();
            return exis;
        }

        public async Task DeleteLessonAsync(int id)
        {
            var exis = await _repository.GetByIdAsync(id);
            if(exis != null)
            {
                _repository.Delete(exis);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vocabulary>> GetVocabularyWithIdLessonAsync(int lessonId)
        {
            return await _repository1.Query()
                .Where(v => v.LessonId == lessonId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
