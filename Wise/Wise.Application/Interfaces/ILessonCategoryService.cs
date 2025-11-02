using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.LessonCategory;
using Wise.Domain.Entities;
using Wise.Domain.Enums;

namespace Wise.Application.Interfaces
{
    public interface ILessonCategoryService
    {
        Task<IEnumerable<LessonCategory>> GetAllAsync();
        Task<LessonCategory?> GetByIdAsync(int id);
        Task<LessonCategory> CreateCategoryAsync(LessonCategoryDto dto);
        Task<LessonCategory?> UpdateCategoryAsync(int id, LessonCategory model);
        Task DeleteCategoryAsync(int id);
    }
}
