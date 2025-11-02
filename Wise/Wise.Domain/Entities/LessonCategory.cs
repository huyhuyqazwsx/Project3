namespace Wise.Domain.Entities
{
    public class LessonCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
