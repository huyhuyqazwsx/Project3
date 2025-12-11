using Microsoft.EntityFrameworkCore;
using Wise.Application.DTOs.Analysis;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class WeaknessAnalysisService : IWeaknessAnalysisService
    {
        private readonly IRepository<LearningDetail> _detailRepo;

        public WeaknessAnalysisService(IRepository<LearningDetail> detailRepo)
        {
            _detailRepo = detailRepo;
        }

        public async Task<WeaknessReportDto> AnalyzeAsync(int userId)
        {
            var details = await _detailRepo.Query()
                .Where(d => d.LearningResult!.UserId == userId)
                .ToListAsync();

            var report = new WeaknessReportDto();


            var skillGroups = details
                .GroupBy(d => d.Skill)
                .Select(g => new SkillWeaknessDto
                {
                    Skill = g.Key.ToString(),
                    TotalQuestions = g.Count(),
                    Correct = g.Count(x => x.IsCorrect)
                })
                .OrderBy(g => g.Accuracy)
                .ToList();

            report.SkillWeakness = skillGroups;


            var topicGroups = details
                .Where(d => d.Topic != null)
                .GroupBy(d => d.Topic!)
                .Select(g => new TopicWeaknessDto
                {
                    Topic = g.Key,
                    TotalQuestions = g.Count(),
                    Correct = g.Count(x => x.IsCorrect)
                })
                .OrderBy(g => g.Accuracy)
                .ToList();

            report.TopicWeakness = topicGroups;

            return report;
        }
    }
}
