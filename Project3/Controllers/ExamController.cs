using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Application.Dtos.Exam;
using Project3.Application.Dtos.StudentExam;
using Project3.Application.Interfaces;
using Project3.Application.Queues;
using Project3.Domain.Entities;
using Project3.Domain.Enums;
using Project3.Domain.Interfaces;

namespace Project3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IRepository<StudentExam> _examStudentRepo;
        private readonly ExamSubmitQueue _examSubmitQueue;

        public ExamController(
            IExamService examService,
            IRepository<StudentExam> examStudentRepo,
            ExamSubmitQueue examSubmitQueue)
        {
            _examService = examService;
            _examStudentRepo = examStudentRepo;
            _examSubmitQueue = examSubmitQueue;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound("Exam not found");

            return Ok(exam);
        }

        [HttpPost("create-exam")]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamForTeacherOrAdmin dto)
        {
            try
            {
                var exam = new Exam
                {
                    Name = dto.Name,
                    BlueprintId = dto.BlueprintId,
                    ClassId = dto.ClassId,
                    DurationMinutes = dto.DurationMinutes,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime
                };

                await _examService.CreateAsync(exam);
                return Ok(exam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] UpdateExamDto dto)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound("Exam not found");

            exam.Name = dto.Name;
            exam.BlueprintId = dto.BlueprintId;
            exam.ClassId = dto.ClassId;
            exam.DurationMinutes = dto.DurationMinutes;
            exam.StartTime = dto.StartTime;
            exam.EndTime = dto.EndTime;

            await _examService.UpdateAsync(exam);
            return Ok(new
            {
                message = "Update exam successfully",
                exam
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound("Exam not found");

            await _examService.DeleteAsync(id);
            return Ok(new
            {
                message = "Delete exam successfully"
            });
        }

        [HttpPost("start-exam")]
        public async Task<IActionResult> StartExam([FromBody] ExamStartRequest dto)
        {
            var req = HttpContext.Request;
            var wsScheme = req.Scheme == "https" ? "wss" : "ws";
            var host = req.Host.Value;
            var websocketUrl = $"{wsScheme}://{host}/ws?examId={dto.ExamId}&studentId={dto.StudentId}";

            var exam = await _examService.GetByIdAsync(dto.ExamId);
            if (exam == null) return BadRequest("Exam not found");

            var state = await _examService.GetExamStudent(dto.ExamId, dto.StudentId);

            if (state != null)
            {
                if (state.Status == ExamStatus.IN_PROGRESS)
                {
                    var deadline = state.StartTime!.AddMinutes(exam.DurationMinutes);
                    if (DateTime.Now > deadline)
                        return Ok(new { status = "expired" });

                    return Ok(new { status = "in_progress", wsUrl = websocketUrl });
                }

                if (state.Status == ExamStatus.COMPLETED)
                {
                    return Ok(new
                    {
                        status = "completed",
                        data = new ResponseResultExamDto
                        {
                            ExamId = state.ExamId,
                            StudentId = state.StudentId,
                            StartTime = state.StartTime,
                            EndTime = state.EndTime,
                            Points = state.Points,
                            Status = state.Status
                        }
                    });
                }

                return Ok(new { status = "expired" });
            }

            if (DateTime.Now < exam.StartTime)
                return BadRequest(new { status = "not_started" });

            if (DateTime.Now > exam.EndTime)
                return BadRequest(new { status = "expired" });

            var examStudent = new StudentExam
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                StartTime = DateTime.Now,
                Status = ExamStatus.IN_PROGRESS
            };

            var deadlineSubmit = examStudent.StartTime.AddMinutes(exam.DurationMinutes);
            _examSubmitQueue.Enqueue(dto.ExamId, dto.StudentId, deadlineSubmit);

            await _examStudentRepo.AddAsync(examStudent);
            await _examStudentRepo.SaveChangesAsync();

            var examForStudent = await _examService.GenerateExamAsync(new CreateExamForStudentDto
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId
            });

            return Ok(new
            {
                status = "create",
                wsUrl = websocketUrl,
                data = examForStudent
            });
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] CreateExamForStudentDto dto)
        {
            var exam = await _examService.GenerateExamAsync(dto);
            return Ok(new
            {
                message = "Generated OK",
                exam
            });
        }
    }
}
