using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Entities;

namespace Wise.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // --- DbSet cho tất cả entity ---
        public DbSet<User> Users => Set<User>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Vocabulary> Vocabularies => Set<Vocabulary>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<LearningResult> LearningResults => Set<LearningResult>();
        public DbSet<LessonCategory> LessonCategories => Set<LessonCategory>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- USER ---
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // ---REFRESH TOKEN
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();

            // --- LESSON ---
            modelBuilder.Entity<Lesson>()
                .Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Enum mapping -> lưu dạng chuỗi (dễ đọc hơn trong DB)
            modelBuilder.Entity<Lesson>()
                .Property(l => l.Type)
                .HasConversion<string>();
            modelBuilder.Entity<Lesson>()
                .Property(l => l.Skill)
                .HasConversion<string>();
            modelBuilder.Entity<Lesson>()
                .Property(l => l.Difficulty)
                .HasConversion<string>();

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Category)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- QUESTION ---
            modelBuilder.Entity<Question>()
                .Property(q => q.Type)
                .HasConversion<string>();

            // Quan hệ: Lesson (1) -> (n) Question
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Lesson)
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ: Question (1) -> (n) Answer
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- VOCABULARY ---
            modelBuilder.Entity<Vocabulary>()
                .HasOne(v => v.Lesson)
                .WithMany(l => l.Vocabularies)
                .HasForeignKey(v => v.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- LEARNING RESULT ---
            modelBuilder.Entity<LearningResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.LearningResults)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LearningResult>()
                .HasOne(r => r.Lesson)
                .WithMany(l => l.LearningResults)
                .HasForeignKey(r => r.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Tên bảng trong DB (tùy chọn, để đồng nhất chuẩn SQL) ---
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Lesson>().ToTable("Lessons");
            modelBuilder.Entity<Vocabulary>().ToTable("Vocabularies");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Answer>().ToTable("Answers");
            modelBuilder.Entity<LearningResult>().ToTable("LearningResults");
            modelBuilder.Entity<LessonCategory>().ToTable("LessonCategories");
    }
}
}
