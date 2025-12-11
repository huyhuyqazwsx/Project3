using Project3.Application.Dtos.ExamBlueprint;
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
    public class ExamBlueprintService : CrudService<ExamBlueprint>, IExamBlueprintService
    {
        public readonly IRepository<ExamBlueprintChapter> _chapterRepo;
        public ExamBlueprintService(
            IRepository<ExamBlueprint> blueprintRepo,
            IRepository<ExamBlueprintChapter> chapterRepo
        ) : base(blueprintRepo)
        {
            _chapterRepo = chapterRepo;
        }

        public async Task<ExamBlueprintDto> CreateBlueprintAsync(CreateExamBlueprintDto dto)
        {
            var blueprint = new ExamBlueprint
            {
                SubjectId = dto.SubjectId,
                CreatedAt = DateTime.UtcNow
            };

            await base.CreateAsync(blueprint);

            var chapters = dto.Chapters.Select(c => new ExamBlueprintChapter
            {
                BlueprintId = blueprint.Id,
                Chapter = c.Chapter,
                EasyCount = c.EasyCount,
                MediumCount = c.MediumCount,
                HardCount = c.HardCount,
                VeryHardCount = c.VeryHardCount
            }).ToList();

            await _chapterRepo.AddRangeAsync(chapters);
            await _chapterRepo.SaveChangesAsync();

            var result = new ExamBlueprintDto
            {
                Id = blueprint.Id,
                SubjectId = blueprint.SubjectId,
                CreatedAt = blueprint.CreatedAt,
                TotalQuestions = chapters.Sum(c => c.TotalQuestions),
                Chapters = chapters.Select(c => new ExamBlueprintChapterDto
                {
                    Chapter = c.Chapter,
                    EasyCount = c.EasyCount,
                    MediumCount = c.MediumCount,
                    HardCount = c.HardCount,
                    VeryHardCount = c.VeryHardCount
                }).ToList()
            };

            return result;
        }
    }
}
