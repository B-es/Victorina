using System.Security.Cryptography;
using Victorina.Data;

namespace Victorina.Models
{
	public class QuizManager()
    {

        // Убираем хранение состояния из менеджера
        public QuestionModel GetCurrentQuestion(QuizState state, QuizHolder quizHolder)
        {
            return quizHolder.GetQuestion(state.Id, state.CurrentIndex);
        }

        public bool SubmitResult(QuizState state, QuizHolder quizHolder, int userChoiceIndex)
        {
            var currentQuestion = GetCurrentQuestion(state, quizHolder);

            if (currentQuestion.RightAnswer == userChoiceIndex)
            {
                state.RightCount++;
            }
            state.CurrentIndex++;

            return state.CurrentIndex >= state.QuestionsCount;
        }

        public QuizResult GetQuizResult(QuizState state)
        {
            double percent = state.RightCount / Convert.ToDouble(state.QuestionsCount) * 100;
            int starsCount = percent < 20 ? 0 : percent < 80 ? 1 : percent < 100 ? 2 : 3;

            return new QuizResult
            {
                RightCount = state.RightCount,
                AllCount = state.QuestionsCount,
                StarsCount = starsCount
            };
        }
    }
}
