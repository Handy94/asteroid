namespace Asteroid
{
    using HandyPackage;
    using UnityEngine;

    public class GameSignals
    {
        public EventSignal<GameObject, Transform[]> PlayerShootSignal = new EventSignal<GameObject, Transform[]>();
        public EventSignal<BulletComponent> BulletSpawnedSignal = new EventSignal<BulletComponent>();
        public EventSignal<BulletComponent> BulletDespawnedSignal = new EventSignal<BulletComponent>();

        public EventSignal<AsteroidComponent> AsteroidSpawnedSignal = new EventSignal<AsteroidComponent>();
        public EventSignal<AsteroidComponent> AsteroidDespawnedSignal = new EventSignal<AsteroidComponent>();

        public EventSignal<GameObject, GameEntityTag> GameEntityDespawnedSignal = new EventSignal<GameObject, GameEntityTag>();
        public EventSignal<GameEntityTagComponent, GameEntityTagComponent> GameEntityCollisionTriggeredSignal = new EventSignal<GameEntityTagComponent, GameEntityTagComponent>();
    }

}
