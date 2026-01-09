using Project3.Application.Dtos.Exam;
using Project3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface IExamService : ICrudService<Exam>
    {
        Task<ExamGenerateResultDto> GenerateExamAsync(CreateExamForStudentDto dto); 
        Task<StudentExam?> GetExamStudent(int examId, int studentId);
        Task<ExamGenerateResultDto> GetCurrentQuestionForExam(int examId, int studentId);
        Task<ExamResultPreviewDto> GetDetailResultExam(int examId, int studentId);
        Task<ExamResultSummaryDto> GetResultSummary(int examId, int studentId);
        Task<IEnumerable<GetListExamForStudentDto>> GetListExamForStudent(int studentId);
        Task<ExamStudentsStatusResponse> GetPreviewScoreStudentsExam(int examId);
    }
}
