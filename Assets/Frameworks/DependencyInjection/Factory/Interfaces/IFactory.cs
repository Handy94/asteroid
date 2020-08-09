using System.Threading.Tasks;
using UniRx.Async;

namespace HandyPackage
{

    public interface IFactory
    {
    }

    public interface IFactory<TValue> : IFactory
    {
        UniTask<TValue> Create();
    }

    public interface IFactory<in TParam1, TValue> : IFactory
    {
        UniTask<TValue> Create(TParam1 param);
    }

    public interface IFactory<in TParam1, in TParam2, TValue> : IFactory
    {
        UniTask<TValue> Create(TParam1 param1, TParam2 param2);
    }

    public interface IFactory<in TParam1, in TParam2, in TParam3, TValue> : IFactory
    {
        UniTask<TValue> Create(TParam1 param1, TParam2 param2, TParam3 param3);
    }

}