using Victorina.Data;
using Victorina.Models;
using Victorina.Services.Interfaces;

namespace Victorina.Services.Impls
{
    public class QuizHolder : IQuizHolder
    {
        private Dictionary<string, List<QuestionModel>> questionsData = new Dictionary<string, List<QuestionModel>>();

        public void Init(QuestionPack[] questionPacks)
        {
            foreach (var api in questionPacks)
            {
                questionsData.Add(api.Id, api.Questions.ToList());
            }
        }

        public IEnumerable<QuestionModel> GetQuestions(string id)
        {
            return questionsData[id];
        }

        public QuestionModel GetQuestion(string id, int index)
        {
            return questionsData[id][index];
        }

        public int GetQuestionCount(string id) => questionsData[id].Count;
    }
}
