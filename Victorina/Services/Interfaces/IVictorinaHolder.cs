using Victorina.Models;

namespace Victorina.Services.Interfaces
{
    public interface IVictorinaHolder
    {
        public IEnumerable<VictorinaModel> Models { get; set; }

        public void Init(List<VictorinaModel> models);
    }
}
