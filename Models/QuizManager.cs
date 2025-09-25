using System.Security.Cryptography;
using Victorina.Data;

namespace Victorina.Models
{
	public class QuizManager(QuizHolder quizHolder)
    {
		private QuizHolder _quizHolder = quizHolder;

		public IEnumerable<QuestionModel> _questions = [];

		private string _id;

		public void SetQuestions(string id)
		{
			_id = id;
            _questions = _quizHolder.GetQuestions(id);
		}

		private int _currentIndex = 0;

		public int RightCount { get; private set; } = 0;

		private bool _isStop => _currentIndex >= _questions.Count();

		public QuestionModel CurrentQuestion => _questions.ElementAt(_currentIndex);

		private int _starsCalculate()
		{
			double percent = RightCount / Convert.ToDouble(_questions.Count()) * 100;
			if (percent < 20) return 0;
			else if (percent < 80) return 1;
			else if (percent < 100) return 2;
			else return 3;
		}

		public bool SubmitResult(int userChoiceIndex)
		{

            if (CurrentQuestion.RightAnswer == userChoiceIndex)
            {
                RightCount++;
            }
			_currentIndex++;

            return _isStop;
        }

		public QuizResult QuizResult => new QuizResult { RightCount = RightCount, AllCount = _questions.Count(), StarsCount = _starsCalculate() };

		public QuizState QuizState => new QuizState { CurrentIndex = _currentIndex, RightCount = RightCount, Id=_id };

		public void initState(QuizState quizState)
		{
			_currentIndex = quizState.CurrentIndex;
			RightCount = quizState.RightCount;
			SetQuestions(quizState.Id);
		}
	}
}
