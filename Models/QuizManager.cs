namespace Victorina.Models
{
	public class QuizManager
	{

		private Dictionary<string, List<Question>> victorinsData = new Dictionary<string, List<Question>>();

		public void Init(QuestionApi[] questionApis)
		{
			foreach(var api in questionApis) {
				victorinsData.Add(api.Key, api.Questions.ToList());
			}
		}

		public IEnumerable<Question> Questions { get; set; }

		public void SetQuestions(string id)
		{
			Questions = victorinsData[id];
		}

		public void Clear()
		{
			currentIndex = 0;
			RightCount = 0;
		}

		public int currentIndex = 0;

		public int RightCount { get; set; } = 0;

		public bool isStop => currentIndex >= Questions.Count();

		public Question CurrentQuestion => Questions.ElementAt(currentIndex);

		private int _starsCalculate()
		{
			double percent = RightCount / Convert.ToDouble(Questions.Count()) * 100;
			if (percent < 20) return 0;
			else if (percent < 80) return 1;
			else if (percent < 100) return 2;
			else return 3;
		}

		public VictorinaResult VictorinaResult => new VictorinaResult { RightCount = RightCount, AllCount = Questions.Count(), StarsCount = _starsCalculate() };

	}
}
