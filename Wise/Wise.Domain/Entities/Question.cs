using Wise.Domain.Entities;
using Wise.Domain.Enums;

public class Question
{
    public int Id { get; set; }
    public int LessonId { get; set; }

    public string Text { get; set; } = "";
    public QuestionType Type { get; set; }

    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? Paragraph { get; set; }

    public int OrderIndex { get; set; }

    // Metadata dùng cho AI
    public SkillType Skill { get; set; }
    public string? Topic { get; set; }
    public DifficultyLevel Difficulty { get; set; }

    // Navigation
    public Lesson? Lesson { get; set; }
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
