using Victorina.Data;
using Victorina.Models;

namespace Victorina.Services.Interfaces
{
    public interface IQuizHolder
    {
        public void Init(QuestionPack[] questionApis);

        public IEnumerable<QuestionModel> GetQuestions(string id);

        public QuestionModel GetQuestion(string id, int index);


        public int GetQuestionCount(string id);
    }
}
