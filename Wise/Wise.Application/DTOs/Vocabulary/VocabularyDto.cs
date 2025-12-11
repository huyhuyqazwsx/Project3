using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wise.Application.DTOs.Vocabulary
{
    public class VocabularyDto
    {
        public class VocabularyResponseDto
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
            public string? Topic { get; set; }
        }

        public class VocabularyRequestDto
        {
            public int LessonId { get; set; }

            public string Word { get; set; } = "";
            public string? Synonym { get; set; } = ""; //Tu dong nghia
            public string PartOfSpeech { get; set; } = ""; //Tu loai
            public string Transcription { get; set; } = ""; //Phien am
            public string AudioUrl { get; set; } = ""; //Am thanh
            public string ImageUrl { get; set; } = "";
            public string Meaning { get; set; } = ""; //Nghia
            public string Example { get; set; } = ""; //Vi du
            public string? Topic { get; set; }
        }
    }
}
