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
    public class ExamAutoSubmitBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ExamSubmitQueue _queue;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);

        public ExamAutoSubmitBackgroundService(
            IServiceScopeFactory scopeFactory,
            ExamSubmitQueue queue)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[AUTO-SUBMIT] Background started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;

                    var snapshot = _queue.Snapshot();

                    while (_queue.TryDequeueExpired(now, out var job))
                    {
                        if (job == null)
                        {
                            continue;
                        }

                        using var scope = _scopeFactory.CreateScope();

                        var examStudentRepo = scope.ServiceProvider
                            .GetRequiredService<IRepository<StudentExam>>();

                        var gradingService = scope.ServiceProvider
                            .GetRequiredService<IExamGradingService>();

                        var state = await examStudentRepo.Query()
                            .FirstOrDefaultAsync(x =>
                                x.ExamId == job.ExamId &&
                                x.StudentId == job.StudentId,
                                stoppingToken);

                        if (state == null)
                        {
                            continue;
                        }


                        if (state.Status != ExamStatus.IN_PROGRESS)
                        {
                            continue;
                        }

                        try
                        {
                            await gradingService.GradeAndSaveAsync(job.ExamId, job.StudentId);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Background error: {ex}");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            Console.WriteLine("Background stopped");
        }

    }
}
