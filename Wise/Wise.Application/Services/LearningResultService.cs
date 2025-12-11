using Microsoft.EntityFrameworkCore;
using Wise.Application.DTOs.Learning;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class LearningResultService : ILearningResultService
    {
        private readonly IRepository<LearningResult> _resultRepo;
        private readonly IRepository<LearningDetail> _detailRepo;

        public LearningResultService(
            IRepository<LearningResult> resultRepo,
            IRepository<LearningDetail> detailRepo)
        {
            _resultRepo = resultRepo;
            _detailRepo = detailRepo;
        }

        public async Task<LearningResult> SubmitAsync(LearningSubmitDto dto)
        {
            int total = dto.Details.Count;
            int correct = dto.Details.Count(d => d.IsCorrect);

            double accuracy = total == 0 ? 0 : (double)correct / total * 100;
            int timeSpent = (int)dto.Details.Sum(d => d.ResponseTime);

            var result = new LearningResult
            {
                UserId = dto.UserId,
                LessonId = dto.LessonId,
                Score = correct,
                Accuracy = accuracy,
                TimeSpent = timeSpent,
                CompletedAt = DateTime.Now
            };

            await _resultRepo.AddAsync(result);
            await _resultRepo.SaveChangesAsync();

            // Lưu từng chi tiết câu trả lời
            foreach (var d in dto.Details)
            {
                var detail = new LearningDetail
                {
                    LearningResultId = result.Id,
                    QuestionId = d.QuestionId,
                    AnswerId = d.AnswerId,
                    IsCorrect = d.IsCorrect,
                    ResponseTime = d.ResponseTime,
                    Skill = d.Skill,
                    Topic = d.Topic
                };

                await _detailRepo.AddAsync(detail);
            }

            await _detailRepo.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<LearningResult>> GetByUserAsync(int userId)
        {
            return await _resultRepo.Query()
                .Where(r => r.UserId == userId)
                .Include(r => r.Details)
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();
        }
    }
}
