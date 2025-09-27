using Victorina.Models;

namespace Victorina.Services.Interfaces
{
    public interface IVictorinaHolder
    {
        public IEnumerable<VictoinaModel> Models { get; set; }

        public void Init(List<VictoinaModel> models);
    }
}
