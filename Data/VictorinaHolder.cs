using Victorina.Models;

namespace Victorina.Data
{
    public class VictorinaHolder
    {
        public IEnumerable<VictoinaModel> Models { get; set; }

        public void Init(List<VictoinaModel> models)
        {
            Models = models;
        }
    }
}
