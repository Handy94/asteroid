namespace Asteroid
{
    using HandyPackage;
    using UnityEngine;

    public class GameSignals
    {
        public EventSignal<BulletComponent> BulletSpawnedSignal = new EventSignal<BulletComponent>();
        public EventSignal<BulletComponent> BulletDespawnedSignal = new EventSignal<BulletComponent>();

        public EventSignal<AsteroidComponent> AsteroidSpawnedSignal = new EventSignal<AsteroidComponent>();
        public EventSignal<AsteroidComponent, GameEntityTag> AsteroidDespawnedSignal = new EventSignal<AsteroidComponent, GameEntityTag>();

        public EventSignal<GameObject, GameEntityTag, GameEntityTag> GameEntityDespawnedSignal = new EventSignal<GameObject, GameEntityTag, GameEntityTag>();
        public EventSignal<GameEntityTagComponent, GameEntityTagComponent> GameEntityCollisionTriggeredSignal = new EventSignal<GameEntityTagComponent, GameEntityTagComponent>();

        public EventSignal<PlayerShipComponent> PlayerSpawnedSignal = new EventSignal<PlayerShipComponent>();
        public PlayerDespawnedPrioritySignal PlayerDespawnedSignal = new PlayerDespawnedPrioritySignal();

        public GameStartPrioritySignal GameStartSignal = new GameStartPrioritySignal();
        public EventSignal GameOverSignal = new EventSignal();

        public EventSignal<EnemyComponent> EnemySpawnedSignal = new EventSignal<EnemyComponent>();
        public EventSignal<EnemyComponent, GameEntityTag> EnemyDespawnedSignal = new EventSignal<EnemyComponent, GameEntityTag>();
    }

}
