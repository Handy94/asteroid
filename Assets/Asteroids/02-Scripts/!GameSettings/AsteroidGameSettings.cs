namespace Asteroid
{
    using UnityEngine;

    [System.Serializable]
    public class AsteroidGameSettings
    {
        public Vector2 minWrapViewportPosition = new Vector2(-0.05f, -0.05f);
        public Vector2 maxWrapViewportPosition = new Vector2(1.05f, 1.05f);

        [Header("Asteroid")]
        public Vector2 minAsteroidSpawnAdditionalViewportPosition = new Vector2(0.05f, 0.05f);
        public Vector2 maxAsteroidSpawnAdditionalViewportPosition = new Vector2(0.1f, 0.1f);
        public float asteroidSpawnTime = 1f;
        public int maxAsteroidCount = 5;

        [Header("Player")]
        public PlayerRocketController playerPrefab;
        public float respawnDelayInSeconds = 3;

        [Header("Score")]
        public int scorePerAsteroid = 10;
    }

}
