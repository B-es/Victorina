namespace Victorina.Models
{
	public class VictorinaManager
	{
		static private VictorinaManager instance;

		public IEnumerable<VictoinaModel> Victorins { get; set; }

		public void Init(List<VictoinaModel> models)
		{
			Victorins = models;
		}

		private VictorinaManager()
		{
		}

		static public VictorinaManager GetInstance()
		{
			if (instance == null)
			{
				instance = new VictorinaManager();
			}
			return instance;
		}
	}
}
