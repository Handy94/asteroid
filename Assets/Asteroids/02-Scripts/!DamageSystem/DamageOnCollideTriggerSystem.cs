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

        private void HandleGameEntityCollisionTriggered(GameEntityTagComponent entity1, GameEntityTagComponent entity2)
        {
            //DamageEntity(entity1, 1);
            DamageEntity(entity2, 1);
        }

        private void DamageEntity(GameEntityTagComponent en, int damage)
        {
            if (en == null) return;
            var damageAble = en.GetComponent<IDamageable>();
            damageAble?.Damage(damage);
        }
    }

}
