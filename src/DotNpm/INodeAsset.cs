using System.Threading.Tasks;

namespace DotNpm {

    public interface INodeAsset {
        Task EnsureReadyAsync();
    }
}