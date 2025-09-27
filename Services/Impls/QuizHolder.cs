using Victorina.Data;
using Victorina.Models;
using Victorina.Services.Interfaces;

namespace Victorina.Services.Impls
{
    public class QuizHolder : IQuizHolder
    {
        private Dictionary<string, List<QuestionModel>> victorinsData = new Dictionary<string, List<QuestionModel>>();

        public void Init(QuestionPack[] questionApis)
        {
            foreach (var api in questionApis)
            {
                victorinsData.Add(api.Id, api.Questions.ToList());
            }
        }

        public IEnumerable<QuestionModel> GetQuestions(string id)
        {
            return victorinsData[id];
        }

        public QuestionModel GetQuestion(string id, int index)
        {
            return victorinsData[id][index];
        }

        public int GetQuestionCount(string id) => victorinsData[id].Count;
    }
}
