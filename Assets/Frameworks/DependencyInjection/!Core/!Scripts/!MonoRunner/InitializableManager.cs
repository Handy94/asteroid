namespace HandyPackage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UniRx.Async;

    public class InitializableManager
    {
        private List<IInitializable> _initializables = new List<IInitializable>();
        private List<ILateInitializable> _lateInitializables = new List<ILateInitializable>();

        public void RegisterInitializable(IInitializable initializable)
        {
            if (_initializables.Contains(initializable)) return;
            _initializables.Add(initializable);
        }
        public void RegisterLateInitializable(ILateInitializable lateInitializable)
        {
            if (_lateInitializables.Contains(lateInitializable)) return;
            _lateInitializables.Add(lateInitializable);
        }

        public async UniTask Initialize()
        {
            int count = _initializables.Count;
            for (int i = 0; i < count; i++)
            {
                await _initializables[i].Initialize();
            }
        }
        public async UniTask LateInitialize()
        {
            int count = _lateInitializables.Count;
            for (int i = 0; i < count; i++)
            {
                await _lateInitializables[i].LateInitialize();
            }
        }
    }
}
