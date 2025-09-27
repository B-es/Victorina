using Victorina.Models;

namespace Victorina.Data
{
	public class QuestionPack
	{
		public string Id { get; set; } = "";
		public QuestionModel[] Questions { get; set; } = [];
	}
}
