using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Asteroid
{
    using HandyPackage;
    using UniRx;

    [Category("@@-Handy Package/Enemy")]
    public class CheckForPlayerSpawnedCondition : ConditionTask
    {
        private GameSignals _gameSignals;
        private BookKeepingInGameData _bookKeepingInGameData;

        private CompositeDisposable disposables = new CompositeDisposable();

        protected override string OnInit()
        {
            string res = base.OnInit();

            _gameSignals = DIResolver.GetObject<GameSignals>();
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();

            return res;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_bookKeepingInGameData.PlayerShipComponent != null)
            {
                YieldReturn(true);
            }

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
        }

        protected override bool OnCheck()
        {
            return false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            disposables.Clear();
        }

        private void HandlePlayerSpawned(PlayerShipComponent playerShipComponent)
        {
            YieldReturn(true);
        }
    }

}
