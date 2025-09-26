namespace Victorina.Models
{
    public class QuizState
    {
        public int CurrentIndex { get; set; } = 0;

        public int RightCount { get; set; } = 0;

        public string Id { get; set; } = "";

        public int QuestionsCount { get; set; } = 0;
    }
}
