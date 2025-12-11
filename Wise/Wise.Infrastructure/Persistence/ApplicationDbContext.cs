using Microsoft.EntityFrameworkCore;
using Wise.Domain.Entities;

namespace Wise.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Vocabulary> Vocabularies => Set<Vocabulary>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<LearningResult> LearningResults => Set<LearningResult>();
        public DbSet<LearningDetail> LearningDetails => Set<LearningDetail>();
        public DbSet<LessonCategory> LessonCategories => Set<LessonCategory>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            // REFRESH TOKEN
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();


            // LESSON (tối giản)
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Category)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            // QUESTION
            modelBuilder.Entity<Question>()
                .Property(q => q.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Question>()
                .Property(q => q.Skill)
                .HasConversion<string>();

            modelBuilder.Entity<Question>()
                .Property(q => q.Difficulty)
                .HasConversion<string>();

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Lesson)
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);


            // ANSWER
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);


            // VOCABULARY
            modelBuilder.Entity<Vocabulary>()
                .HasOne(v => v.Lesson)
                .WithMany(l => l.Vocabularies)
                .HasForeignKey(v => v.LessonId)
                .OnDelete(DeleteBehavior.Cascade);


            // LEARNING RESULT
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


            // LEARNING DETAIL (AI DATA)

            modelBuilder.Entity<LearningDetail>()
                .Property(ld => ld.Skill)
                .HasConversion<string>();

            modelBuilder.Entity<LearningDetail>()
                .HasOne(ld => ld.LearningResult)
                .WithMany(r => r.Details)
                .HasForeignKey(ld => ld.LearningResultId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LearningDetail>()
                .HasOne(ld => ld.Question)
                .WithMany()
                .HasForeignKey(ld => ld.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LearningDetail>()
                .HasOne(ld => ld.Answer)
                .WithMany()
                .HasForeignKey(ld => ld.AnswerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Lesson>().ToTable("Lessons");
            modelBuilder.Entity<Vocabulary>().ToTable("Vocabularies");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Answer>().ToTable("Answers");
            modelBuilder.Entity<LearningResult>().ToTable("LearningResults");
            modelBuilder.Entity<LearningDetail>().ToTable("LearningDetails");
            modelBuilder.Entity<LessonCategory>().ToTable("LessonCategories");
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
        }
    }
}
