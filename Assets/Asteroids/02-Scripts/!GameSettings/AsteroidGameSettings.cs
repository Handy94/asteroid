﻿namespace Asteroid
{
    using UnityEngine;

    [System.Serializable]
    public class AsteroidGameSettings
    {
        [Header("Player Initial Data")]
        public int InitialPlayerLife = 3;
        public int InitialPlayerScore = 0;
        public int InitialPlayerStage = 1;

        [Header("Screen Data")]
        public Vector2 minWrapViewportPosition = new Vector2(-0.05f, -0.05f);
        public Vector2 maxWrapViewportPosition = new Vector2(1.05f, 1.05f);

        [Header("Asteroid")]
        public Vector2 minAsteroidSpawnAdditionalViewportPosition = new Vector2(0.05f, 0.05f);
        public Vector2 maxAsteroidSpawnAdditionalViewportPosition = new Vector2(0.1f, 0.1f);

        [Header("Player")]
        public PlayerShipComponent playerPrefab;
        public float respawnDelayInSeconds = 3;

        [Header("Score")]
        public int bonusLifeScoreMultiplierThreshold = 10000;
        public int bonusLifeAdd = 1;

        [Header("Enemy")]
        public float enemySpawnInterval = 5;
        public float maxEnemyOnScreen = 2;
        public Vector2 minEnemySpawnAdditionalViewportPosition = new Vector2(0.05f, 0.05f);
        public Vector2 maxEnemySpawnAdditionalViewportPosition = new Vector2(0.1f, 0.1f);
    }

}
