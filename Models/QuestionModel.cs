namespace Victorina.Models
{
    public class QuestionModel
    {
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string[] Answers { get; set; }
        public int RightAnswer { get; set; }
    }
}
