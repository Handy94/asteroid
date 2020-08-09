namespace HandyPackage
{
    using System;
    using UnityEngine;

    public class MonoRunner : MonoBehaviour
    {
        private InitializableManager _initializableManager = new InitializableManager();
        private TickableManager _tickableManager = new TickableManager();
        private DisposableManager _disposableManager = new DisposableManager();
        public EventSignal InitializeCompleteEvent = new EventSignal();

        private bool _isInitializationComplete = false;
        #region Mono
        private async void Start()
        {
            await _initializableManager.Initialize();
            await _initializableManager.LateInitialize();
            InitializeCompleteEvent.Fire();
            _isInitializationComplete = true;
        }

        private void FixedUpdate()
        {
            if (_isInitializationComplete)
                _tickableManager.FixedTick();
        }

        private void Update()
        {
            if (_isInitializationComplete)
                _tickableManager.Tick();
        }

        private void LateUpdate()
        {
            if (_isInitializationComplete)
                _tickableManager.LateTick();
        }

        private void OnDestroy()
        {
            _disposableManager.Dispose();
            _disposableManager.LateDispose();
        }
        #endregion

        #region Container
        public void InstallContainer(DIContainer container)
        {
            if (container.Container.Count == 0) return;
            foreach (var item in container.Container)
            {
                RegisterObject(item.Value);
            }
        }

        public void RegisterObject(object obj)
        {
            if (obj is IInitializable) _initializableManager.RegisterInitializable((IInitializable)obj);
            if (obj is ILateInitializable) _initializableManager.RegisterLateInitializable((ILateInitializable)obj);

            if (obj is IFixedTickable) _tickableManager.RegisterFixedTickable((IFixedTickable)obj);
            if (obj is ITickable) _tickableManager.RegisterTickable((ITickable)obj);
            if (obj is ILateTickable) _tickableManager.RegisterLateTickable((ILateTickable)obj);

            if (obj is IDisposable) _disposableManager.RegisterDisposable((IDisposable)obj);
            if (obj is ILateDisposable) _disposableManager.RegisterLateDisposable((ILateDisposable)obj);
        }
        #endregion
    }
}
