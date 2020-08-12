using HandyPackage;

namespace Asteroid
{
    using UnityEngine;

    public class EntityCollisionDetectorComponent : MonoBehaviour
    {
        [SerializeField] private GameEntityTagComponent gameEntityTagComponent;

        private GameSignals _gameSignals;

        private void Awake()
        {
            _gameSignals = DIResolver.GetObject<GameSignals>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var colTagComp = collision.GetComponent<GameEntityTagComponent>();
            _gameSignals.GameEntityCollisionTriggeredSignal.Fire(gameEntityTagComponent, colTagComp);
        }
    }

}
