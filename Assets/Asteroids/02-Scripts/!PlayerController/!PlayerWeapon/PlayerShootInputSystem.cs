﻿namespace Asteroid
{
    using HandyPackage;
    using UniRx;
    using UniRx.Async;
    using UnityEngine;

    public class PlayerShootInputSystem : IInitializable, System.IDisposable, IPlayerInputListener
    {
        private const string INPUT_SHOOT_NAME = "Fire1";

        private GameSignals _gameSignals;

        private IWeapon _playerWeapon;
        private CompositeDisposable disposables = new CompositeDisposable();
        private CompositeDisposable inputDisposables = new CompositeDisposable();

        public UniTask Initialize()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();

            _gameSignals.PlayerSpawnedSignal.Listen(HandlePlayerSpawned).AddTo(disposables);
            _gameSignals.PlayerDespawnedSignal.Listen(HandlePlayerDespawned, PlayerDespawnedPrioritySignal.Priority.UNLISTEN_PLAYER_SHOOT_INPUT).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnlistenForPlayerInput();
        }

        private void HandlePlayerSpawned(PlayerShipController playerShipController)
        {
            _playerWeapon = playerShipController.PlayerWeapon;
            ListenForPlayerInput();
        }

        private bool HandlePlayerDespawned(PlayerShipController playerShipController)
        {
            UnlistenForPlayerInput();
            _playerWeapon = null;
            return true;
        }

        public void ListenForPlayerInput()
        {
            Observable.EveryUpdate().Subscribe(x =>
            {
                CheckInput();
            }).AddTo(inputDisposables);
        }

        public void UnlistenForPlayerInput()
        {
            inputDisposables.Clear();
        }

        private void CheckInput()
        {
            if (Input.GetButton(INPUT_SHOOT_NAME))
            {
                _playerWeapon.Shoot();
            }
            else if (Input.GetButtonUp(INPUT_SHOOT_NAME))
            {
                _playerWeapon.StopShoot();
            }
        }
    }

}