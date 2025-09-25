using Victorina.Models;

namespace Victorina.Data
{
    public class QuizHolder
    {
        private Dictionary<string, List<QuestionModel>> victorinsData = new Dictionary<string, List<QuestionModel>>();

        public void Init(QuestionPack[] questionApis)
        {
            foreach (var api in questionApis)
            {
                victorinsData.Add(api.Key, api.Questions.ToList());
            }
        }

        public IEnumerable<QuestionModel> GetQuestions(string id)
        {
            return victorinsData[id];
        }

    }
}
