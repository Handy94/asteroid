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

        private void HandleGameEntityCollisionTriggered(IDamageable<GameEntityTag> damageAble, IDamager<GameEntityTag> damager)
        {
            DamageEntity(damageAble, damager);
        }

        private void DamageEntity(IDamageable<GameEntityTag> damageAble, IDamager<GameEntityTag> damager)
        {
            if (damageAble == null) return;
            int damage = damager.GetDamage();

            damageAble?.Damage(damage, damager.DamagerType);
        }
    }

}
