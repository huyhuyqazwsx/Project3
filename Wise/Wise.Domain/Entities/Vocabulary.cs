namespace Wise.Domain.Entities
{
    public class Vocabulary
    {
        public int Id { get; set; }
        public int LessonId { get; set; }

        public string Word { get; set; } = "";
        public string? Synonym { get; set; } = ""; //Tu dong nghia
        public string PartOfSpeech { get; set; } = ""; //Tu loai
        public string Transcription { get; set; } = ""; //Phien am
        public string AudioUrl { get; set; } = ""; //Am thanh
        public string ImageUrl { get; set; } = "";
        public string Meaning { get; set; } = ""; //Nghia
        public string Example { get; set; } = ""; //Vi du

        public Lesson? Lesson { get; set; }
    }
}
