using Microsoft.EntityFrameworkCore;
using Wise.Application.DTOs.Lesson;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly IRepository<Lesson> _lessonRepo;
        private readonly IRepository<Vocabulary> _vocaRepo;
        public LessonService(IRepository<Lesson> lessonRepo, IRepository<Vocabulary> vocaRepo)
        {
            _lessonRepo = lessonRepo;
            _vocaRepo = vocaRepo;
        }

        public async Task<IEnumerable<Lesson>> GetAllAsync()
        {
            return await _lessonRepo.GetAllAsync();
        }
        public async Task<IEnumerable<ResponseLessonDto>> GetListWithCategoryId(int catId)
        {
            return await _lessonRepo.Query()
                .Where(l => l.CategoryId == catId)
                .OrderBy(l => l.OrderIndex)
                .Select(l => new ResponseLessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    ImageUrl = l.ImageUrl,
                    CategoryId = l.CategoryId,
                    OrderIndex = l.OrderIndex
                })
                .ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _lessonRepo.Query()
                .Include(l => l.Questions)
                .Include(l => l.Vocabularies)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lesson> CreateLessonAsync(RequestLessonDto dto)
        {
            var lesson = new Lesson
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                OrderIndex = dto.OrderIndex
            };

            await _lessonRepo.AddAsync(lesson);
            await _lessonRepo.SaveChangesAsync();

            return lesson;
        }

        public async Task<Lesson?> UpdateLessonAsync(int id, RequestLessonDto dto)
        {
            var lesson = await _lessonRepo.GetByIdAsync(id);
            if (lesson == null) return null;

            lesson.Title = dto.Title;
            lesson.Description = dto.Description;
            lesson.ImageUrl = dto.ImageUrl;
            lesson.CategoryId = dto.CategoryId;
            lesson.OrderIndex = dto.OrderIndex;

            _lessonRepo.Update(lesson);
            await _lessonRepo.SaveChangesAsync();

            return lesson;
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _lessonRepo.GetByIdAsync(id);
            if (lesson != null)
            {
                _lessonRepo.Delete(lesson);
                await _lessonRepo.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vocabulary>> GetVocabularyWithIdLessonAsync(int lessonId)
        {
            return await _vocaRepo.Query()
                .Where(v => v.LessonId == lessonId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
