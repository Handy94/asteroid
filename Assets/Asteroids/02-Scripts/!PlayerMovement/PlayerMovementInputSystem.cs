using HandyPackage;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Asteroid
{
    public class PlayerMovementInputSystem : IInitializable, System.IDisposable, IPlayerInputListener
    {
        private const string INPUT_AXIS_VERTICAL = "Vertical";
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";

        private RocketMovement _rocketMovement;
        private bool _canInput;
        private CompositeDisposable disposables = new CompositeDisposable();

        public PlayerMovementInputSystem(RocketMovement rocketMovement)
        {
            _rocketMovement = rocketMovement;
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
            float vAxis = Mathf.Clamp(Input.GetAxis(INPUT_AXIS_VERTICAL), 0, 1);
            float hAxis = Input.GetAxis(INPUT_AXIS_HORIZONTAL);

            _rocketMovement.MoveRocket(vAxis);
            _rocketMovement.RotateRocket(-hAxis);
        }
    }
}
