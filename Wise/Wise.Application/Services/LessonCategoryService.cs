using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.LessonCategory;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class LessonCategoryService : ILessonCategoryService
    {
        private readonly IRepository<LessonCategory> _repository;
        public LessonCategoryService(IRepository<LessonCategory> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LessonCategory>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<LessonCategory?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<LessonCategory> CreateCategoryAsync(LessonCategoryDto dto)
        {
            var model = new LessonCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
            };
            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();
            return model;
        }

        public async Task<LessonCategory?> UpdateCategoryAsync(int id, LessonCategory model)
        {
            var exis = await _repository.GetByIdAsync(id);
            if (exis == null) return null;

            exis.Description = model.Description;
            exis.Name = model.Name;
            exis.ImageUrl = model.ImageUrl;

            _repository.Update(exis);
            await _repository.SaveChangesAsync();
            return exis;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var exis = await _repository.GetByIdAsync(id);
            if (exis != null)
            {
                _repository.Delete(exis);
                await _repository.SaveChangesAsync();
            }
        }
    }
}
