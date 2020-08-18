using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Asteroid
{
    using HandyPackage;
    using UniRx;

    [Category("@@-Handy Package/Enemy")]
    public class CheckForPlayerExistCondition : ConditionTask
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
                var hyperSpaceComp = _bookKeepingInGameData.PlayerShipComponent.GetComponent<IShipHyperSpace>();
                if (!hyperSpaceComp.IsOnHyperSpace)
                {
                    YieldReturn(true);
                }
            }

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerHyperSpaceFinishedSignal.Listen(HandlePlayerHyperSpaceFinished).AddTo(disposables);
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

        private void HandlePlayerHyperSpaceFinished()
        {
            YieldReturn(true);
        }
    }

}
