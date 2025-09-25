using Victorina.Data;

namespace Victorina.Models
{
	public class VictorinaManager(VictorinaHolder victorinaHolder)
	{
		private VictorinaHolder _victorinaHolder = victorinaHolder;

		public IEnumerable<VictoinaModel> Models { get => _victorinaHolder.Models;}
	}
}
