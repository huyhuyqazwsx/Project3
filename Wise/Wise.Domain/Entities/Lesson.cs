using Wise.Domain.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";

    public int CategoryId { get; set; }
    public LessonCategory? Category { get; set; }

    public int OrderIndex { get; set; } = 0;

    // Navigation
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Vocabulary> Vocabularies { get; set; } = new List<Vocabulary>();
    public ICollection<LearningResult> LearningResults { get; set; } = new List<LearningResult>();
}
