namespace Asteroid
{
    using HandyPackage;
    using UniRx.Async;
    using UnityEngine;

    public class StandalonePlayerInputListener : IInitializable, ITickable
    {
        private InputSignal _inputSignal;

        private const string INPUT_AXIS_VERTICAL = "Vertical";
        private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
        private const string INPUT_SHOOT_NAME = "Fire1";

        public UniTask Initialize()
        {
            _inputSignal = DIResolver.GetObject<InputSignal>();

            return UniTask.CompletedTask;
        }

        public void Tick()
        {
            CheckMovementInput();
            CheckShootInput();
            CheckHyperSpaceInput();
        }

        private void CheckMovementInput()
        {
            float vAxis = Mathf.Clamp(Input.GetAxisRaw(INPUT_AXIS_VERTICAL), 0, 1);
            float hAxis = Input.GetAxisRaw(INPUT_AXIS_HORIZONTAL);

            if (vAxis > 0) _inputSignal.Fire(PlayerShipInputKey.MOVE_FORWARD);
            else _inputSignal.Fire(PlayerShipInputKey.STOP_MOVE_FORWARD);

            if (hAxis < 0) _inputSignal.Fire(PlayerShipInputKey.ROTATE_COUNTERCLOCKWISE);
            else if (hAxis > 0) _inputSignal.Fire(PlayerShipInputKey.ROTATE_CLOCKWISE);
            else _inputSignal.Fire(PlayerShipInputKey.STOP_ROTATE);
        }

        private void CheckShootInput()
        {
            if (Input.GetButton(INPUT_SHOOT_NAME))
            {
                _inputSignal.Fire(PlayerShipInputKey.SHOOT);
            }
            else if (Input.GetButtonUp(INPUT_SHOOT_NAME))
            {
                _inputSignal.Fire(PlayerShipInputKey.STOP_SHOOT);
            }
        }

        private void CheckHyperSpaceInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _inputSignal.Fire(PlayerShipInputKey.HYPER_SPACE);
            }
        }
    }

}
