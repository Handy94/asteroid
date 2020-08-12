using HandyPackage;
using UniRx;
using UniRx.Async;

namespace Asteroid
{
    public class DamageOnCollideTriggerSystem : IInitializable, System.IDisposable
    {
        private CompositeDisposable disposables = new CompositeDisposable();

        private GameSignals _gameSignals;

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();

            _gameSignals.GameEntityCollisionTriggeredSignal.Listen(HandleGameEntityCollisionTriggered).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void HandleGameEntityCollisionTriggered(GameEntityTagComponent damager, GameEntityTagComponent damaged)
        {
            DamageEntity(damaged, damager, 1);
        }

        private void DamageEntity(GameEntityTagComponent en, GameEntityTagComponent damager, int damage)
        {
            if (en == null) return;
            var damageAble = en.GetComponent<IDamageable<GameEntityTag>>();
            damageAble?.Damage(damage, damager.GameEntityTag);
        }
    }

}
