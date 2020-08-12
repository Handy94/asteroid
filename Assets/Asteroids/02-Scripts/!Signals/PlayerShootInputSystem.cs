namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class PlayerShootInputSystem : IInitializable, System.IDisposable, IPlayerInputListener
    {
        private const string INPUT_SHOOT_NAME = "Fire1";

        private IWeapon weapon;
        private CompositeDisposable disposables = new CompositeDisposable();

        public PlayerShootInputSystem(IWeapon weapon)
        {
            this.weapon = weapon;
        }

        public UniTask Initialize()
        {
            ListenForPlayerInput();
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnlistenForPlayerInput();
        }

        public void ListenForPlayerInput()
        {
            Observable.EveryUpdate().Subscribe(x =>
            {
                CheckInput();
            }).AddTo(disposables);
        }

        public void UnlistenForPlayerInput()
        {
            disposables.Clear();
        }

        private void CheckInput()
        {
            if (Input.GetButton(INPUT_SHOOT_NAME))
            {
                weapon.Shoot();
            }
            else if (Input.GetButtonUp(INPUT_SHOOT_NAME))
            {
                weapon.StopShoot();
            }
        }
    }

}
