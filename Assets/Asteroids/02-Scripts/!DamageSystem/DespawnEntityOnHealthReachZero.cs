﻿using HandyPackage;
using UnityEngine;

namespace Asteroid
{
    public class DespawnEntityOnHealthReachZero : MonoBehaviour
    {
        [SerializeField] private GameEntityTagComponent gameEntityTagComponent;
        [SerializeField] private EntityHealthComponent healthComponent;

        private DisposableCollection disposables = new DisposableCollection();

        private GameSignals _gameSignals;

        private void Reset()
        {
            if (gameEntityTagComponent == null) gameEntityTagComponent = GetComponent<GameEntityTagComponent>();
            if (healthComponent == null) healthComponent = GetComponent<EntityHealthComponent>();
        }

        private void Awake()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
        }

        private void OnEnable()
        {
            healthComponent.OnHealthChanged.Listen(OnHealthChanged).AddToDisposables(disposables);
        }

        private void OnDestroy()
        {
            disposables.Clear();
        }

        private void OnHealthChanged(int health, GameEntityTag damagerEntityTag)
        {
            if (health == 0) _gameSignals.GameEntityDespawnedSignal.Fire(gameObject, gameEntityTagComponent.GameEntityTag, damagerEntityTag);
        }
    }

}
