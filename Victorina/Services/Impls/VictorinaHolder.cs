using Victorina.Models;
using Victorina.Services.Interfaces;

namespace Victorina.Services.Impls
{
    public class VictorinaHolder : IVictorinaHolder
    {
        public IEnumerable<VictorinaModel> Models { get; set; }

        public void Init(List<VictorinaModel> models)
        {
            Models = models;
        }
    }
}
