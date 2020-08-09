using System.Threading.Tasks;
using UniRx.Async;

namespace HandyPackage
{
    public interface ILateInitializable
    {
        UniTask LateInitialize();
    }
}