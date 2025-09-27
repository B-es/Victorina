using Victorina.Models;
using Victorina.Services.Interfaces;

namespace Victorina.Services.Impls
{
    public class VictorinaHolder : IVictorinaHolder
    {
        public IEnumerable<VictoinaModel> Models { get; set; }

        public void Init(List<VictoinaModel> models)
        {
            Models = models;
        }
    }
}
