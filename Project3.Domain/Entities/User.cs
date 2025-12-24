using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Project3.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string MSSV { get; set; }
        public required string FullName { get; set; } = String.Empty;
        public required DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRole Role { get; set; }

        // Navigation properties
        public ICollection<Class> TaughtClasses { get; set; } = new List<Class>();
        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
        public ICollection<QuestionExam> QuestionExams { get; set; } = new List<QuestionExam>();
        public ICollection<StudentQuestion> StudentQuestions { get; set; } = new List<StudentQuestion>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
