namespace Wise.Application.DTOs.Analysis
{
    public class SkillWeaknessDto
    {
        public string Skill { get; set; } = "";
        public int TotalQuestions { get; set; }
        public int Correct { get; set; }
        public int Wrong => TotalQuestions - Correct;
        public double Accuracy => TotalQuestions == 0 ? 0 : (double)Correct / TotalQuestions * 100;
    }

    public class TopicWeaknessDto
    {
        public string Topic { get; set; } = "";
        public int TotalQuestions { get; set; }
        public int Correct { get; set; }
        public int Wrong => TotalQuestions - Correct;
        public double Accuracy => TotalQuestions == 0 ? 0 : (double)Correct / TotalQuestions * 100;
    }

    public class WeaknessReportDto
    {
        public List<SkillWeaknessDto> SkillWeakness { get; set; } = new();
        public List<TopicWeaknessDto> TopicWeakness { get; set; } = new();
    }
}
