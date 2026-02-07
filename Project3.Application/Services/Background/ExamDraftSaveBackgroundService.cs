using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project3.Application.Interfaces.Websocket;
using Project3.Application.Queues;
using Project3.Domain.Entities;
using Project3.Domain.Enums;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services.Background
{
    public class ExamDraftSaveBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ExamSubmitQueue _queue;
        private readonly IExamAnswerCache _cache;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public ExamDraftSaveBackgroundService(
            IServiceScopeFactory scopeFactory,
            ExamSubmitQueue queue,
            IExamAnswerCache cache)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var examStudentRepo = scope.ServiceProvider
                        .GetRequiredService<IRepository<StudentExam>>();

                    var studentQuestionRepo = scope.ServiceProvider
                        .GetRequiredService<IRepository<StudentQuestion>>();

                    var jobs = _queue.Snapshot();

                    var examId = -1;
                    var studentId = -1;

                    foreach (var job in jobs)
                    {
                        examId = job.ExamId;
                        studentId = job.StudentId;

                        var examState = await examStudentRepo.Query()
                            .FirstOrDefaultAsync(x =>
                                x.ExamId == examId &&
                                x.StudentId == studentId,
                                stoppingToken);

                        if (examState == null ||
                            examState.Status != ExamStatus.IN_PROGRESS)
                            continue;

                        var answers = _cache.GetAnswers(examId, studentId);
                        if (answers == null || !answers.Any())
                            continue;

                        foreach (var ans in answers)
                        {
                            var existing = await studentQuestionRepo.Query()
                                .FirstOrDefaultAsync(x =>
                                    x.ExamId == examId &&
                                    x.StudentId == studentId &&
                                    x.QuestionId == ans.QuestionId,
                                    stoppingToken);

                            if (existing == null)
                            {
                                await studentQuestionRepo.AddAsync(new StudentQuestion
                                {
                                    ExamId = examId,
                                    StudentId = studentId,
                                    QuestionId = ans.QuestionId,
                                    Answer = ans.Answer,
                                    Result = null,            
                                    CreatedAt = DateTime.Now,
                                    QuestionPoint = 0
                                });
                            }
                            else
                            {
                                existing.Answer = ans.Answer;
                                studentQuestionRepo.UpdateAsync(existing);
                            }
                        }
                    }

                    await studentQuestionRepo.SaveChangesAsync();
                    Console.WriteLine("Draft-save " + examId + " " + studentId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Draft-save background error: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
