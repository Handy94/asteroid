namespace Asteroid
{
    using HandyPackage;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using System.Collections.Generic;
    using UniRx.Async;
    using UnityEngine;

    public class PlayerRocketMovementInputSystem : SerializedMonoBehaviour,
        IInitializable, IFixedTickable, System.IDisposable
    {
        [SerializeField] public RocketMovement RocketMovement;
        [System.NonSerialized, OdinSerialize] public PlatformBasedPlayerPlayerRocketInputMovementData PlayerRocketInputMovementData;

        private Dictionary<RocketMovement, PlayerRocketInputMovementData> _rocketInput = new Dictionary<RocketMovement, PlayerRocketInputMovementData>();
        private InputManager _inputManager;

        public UniTask Initialize()
        {
            _inputManager = DIResolver.GetObject<InputManager>();

            RegisterRocketForInput(RocketMovement, PlayerRocketInputMovementData.Value);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {

        }

        public void FixedTick()
        {
            foreach (var item in _rocketInput)
            {
                item.Key.ThrustMove();
                item.Key.ThrustRotation();
            }
        }

        private void RegisterToInputSystem(PlayerRocketInputMovementData playerRocketInputMovementData)
        {
            _inputManager.RegisterInputListener(playerRocketInputMovementData.ThrustForwardInputListener);
            _inputManager.RegisterInputListener(playerRocketInputMovementData.StopThrustForwardInputListener);
            _inputManager.RegisterInputListener(playerRocketInputMovementData.ThrustLeftInputListener);
            _inputManager.RegisterInputListener(playerRocketInputMovementData.StopThrustLeftInputListener);
            _inputManager.RegisterInputListener(playerRocketInputMovementData.ThrustRightInputListener);
            _inputManager.RegisterInputListener(playerRocketInputMovementData.StopThrustRightInputListener);
        }

        private void RemoveFromInputSystem(PlayerRocketInputMovementData playerRocketInputMovementData)
        {
            _inputManager.RemoveInputListener(playerRocketInputMovementData.ThrustForwardInputListener);
            _inputManager.RemoveInputListener(playerRocketInputMovementData.StopThrustForwardInputListener);
            _inputManager.RemoveInputListener(playerRocketInputMovementData.ThrustLeftInputListener);
            _inputManager.RemoveInputListener(playerRocketInputMovementData.StopThrustLeftInputListener);
            _inputManager.RemoveInputListener(playerRocketInputMovementData.ThrustRightInputListener);
            _inputManager.RemoveInputListener(playerRocketInputMovementData.StopThrustRightInputListener);
        }

        public void RegisterRocketForInput(RocketMovement rocketMovement, PlayerRocketInputMovementData playerRocketInputMovementData)
        {
            playerRocketInputMovementData.ThrustForwardInputListener.SetAction(rocketMovement.ThrustForward);
            playerRocketInputMovementData.StopThrustForwardInputListener.SetAction(rocketMovement.StopThrustForward);
            playerRocketInputMovementData.ThrustLeftInputListener.SetAction(rocketMovement.ThrustLeft);
            playerRocketInputMovementData.StopThrustLeftInputListener.SetAction(rocketMovement.StopThrustRotation);
            playerRocketInputMovementData.ThrustRightInputListener.SetAction(rocketMovement.ThrustRight);
            playerRocketInputMovementData.StopThrustRightInputListener.SetAction(rocketMovement.StopThrustRotation);

            RegisterToInputSystem(playerRocketInputMovementData);

            _rocketInput.Add(rocketMovement, playerRocketInputMovementData);
        }

        public void RemoveRocketFromInput(RocketMovement rocketMovement)
        {
            if (!_rocketInput.ContainsKey(rocketMovement)) return;

            RemoveFromInputSystem(_rocketInput[rocketMovement]);

            _rocketInput.Remove(rocketMovement);
        }
    }

}
