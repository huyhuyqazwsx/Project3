using Microsoft.EntityFrameworkCore;
using Project3.Application.Dtos.WebSocket;
using Project3.Application.Interfaces;
using Project3.Application.Interfaces.Websocket;
using Project3.Application.Queues;
using Project3.Domain.Entities;
using Project3.Domain.Enums;
using Project3.Domain.Interfaces;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project3.Middlewares
{
    public class ExamWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ExamSubmitQueue _examSubmitQueue;
        public ExamWebSocketMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory,
            ExamSubmitQueue examSubmitQueue)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _examSubmitQueue = examSubmitQueue;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/ws"))
            {
                await _next(context);
                return;
            }

            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            if (!int.TryParse(context.Request.Query["examId"], out int examId) ||
                !int.TryParse(context.Request.Query["studentId"], out int studentId))
            {
                context.Response.StatusCode = 400;
                return;
            }

            using WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
            await ListenLoop(socket, examId, studentId);
        }

        private async Task ListenLoop(WebSocket socket, int examId, int studentId)
        {
            using var scope = _scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IExamAnswerCache>();
            var grading = scope.ServiceProvider.GetRequiredService<IExamGradingService>();
            var _examService = scope.ServiceProvider.GetService<IExamService>();

            var sendTask = Task.Run(async () =>
            {
                try
                {
                    while (socket.State == WebSocketState.Open)
                    {
                        if (_examService != null)
                        {
                            var examStudent = await _examService.GetExamStudent(examId, studentId);
                            var exam = await _examService.GetByIdAsync(examId);
                            if (examStudent == null || exam == null)
                            {
                                var msgBytes = Encoding.UTF8.GetBytes(
                                JsonSerializer.Serialize(new { status = $"error : Không có bài thi cho sinh viên này {examId} : {studentId}" })
                                );
                                await socket.SendAsync(msgBytes, WebSocketMessageType.Text, true, CancellationToken.None);
                                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                                break;
                            }
                        }
                        else
                        {
                            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                finally
                {
                    if (socket.State != WebSocketState.Closed &&
                    socket.State != WebSocketState.Aborted)
                    {
                        await socket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Closed",
                            CancellationToken.None);
                    }
                }
            });


            // nhan ycau tu fe de xu ly

            var receiveTask = Task.Run(async () => {

                var buffer = new byte[4096];
                try
                {

                    while (socket.State == WebSocketState.Open)
                    {
                        var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close) break;

                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);


                        WsMessageDto? msg;
                        try
                        {
                            msg = JsonSerializer.Deserialize<WsMessageDto>(json,
                                new JsonSerializerOptions
                                {
                                    Converters = { new JsonStringEnumConverter() }
                                });
                        }
                        catch
                        {
                            Console.WriteLine("Invalid JSON.");
                            continue;
                        }

                        if (msg == null) continue;

                        switch (msg.Action)
                        {
                            case WebsocketAction.SubmitAnswer:
                                await HandleSubmitAnswer(socket, examId, studentId, msg.Order, msg.QuestionId, msg.Answer); ;
                                break;

                            case WebsocketAction.SubmitExam:
                                await HandleSubmitExam(socket, examId, studentId);
                                return;

                            case WebsocketAction.SyncState:
                            case WebsocketAction.Reconnect:
                                await HandleSync(socket, examId, studentId);
                                break;

                            case WebsocketAction.Heartbeat:
                                var ms = Encoding.UTF8.GetBytes(
                                    JsonSerializer.Serialize(new { status = "Heartbeat" })
                                );
                                await socket.SendAsync(ms, WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (socket.State != WebSocketState.Closed &&
                        socket.State != WebSocketState.Aborted)
                    {
                        await socket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Closed",
                            CancellationToken.None);
                    }
                }

            });

            await Task.WhenAny(sendTask, receiveTask);

        }

        private async Task HandleSync(WebSocket socket, int examId, int studentId)
        {
            using var scope = _scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IExamAnswerCache>();

            var answers = cache.GetAnswers(examId, studentId);

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(answers));

            await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task HandleSubmitExam(WebSocket socket, int examId, int studentId)
        {
            using var scope = _scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IExamAnswerCache>();
            var gradingService = scope.ServiceProvider.GetRequiredService<IExamGradingService>();

            float score = await gradingService.GradeAndSaveAsync(examId, studentId);

            //Xóa khỏi hàng chờ theo dõi
            _examSubmitQueue.Remove(examId, studentId);

            var msgBytes = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(new { status = "submitted", score })
            );

            await socket.SendAsync(msgBytes, WebSocketMessageType.Text, true, CancellationToken.None);

            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Exam submitted",
                CancellationToken.None
            );
        }
        private async Task HandleSubmitAnswer(
            WebSocket socket,
            int examId, 
            int studentId , 
            int Order , 
            int QuestionId , 
            string Answer)
        {
            using var scope = _scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IExamAnswerCache>();
            var examStudentRepo = scope.ServiceProvider
                .GetRequiredService<IRepository<StudentExam>>();

            //Check hết hạn
            var state = await examStudentRepo.Query()
                .FirstOrDefaultAsync(x =>
                    x.ExamId == examId &&
                    x.StudentId == studentId);

            if (state == null || state.Status != ExamStatus.IN_PROGRESS)
            {
                await socket.SendAsync(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(new
                        {
                            status = "exam_closed"
                        })
                    ),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                await socket.CloseAsync(
                    WebSocketCloseStatus.PolicyViolation,
                    "Exam closed",
                    CancellationToken.None
                );

                return;
            }

            //Lưu đáp án
            cache.SaveAnswer(examId, studentId, Order, QuestionId, Answer);
            var msgBytes = Encoding.UTF8.GetBytes(
                                    JsonSerializer.Serialize(
                                        new { status = "submitted answer",
                                            id = QuestionId,
                                            answer = Answer
                                        })
                                );
            await socket.SendAsync(msgBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
