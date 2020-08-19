namespace Asteroid
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class PlayerShipComponent : SerializedMonoBehaviour
    {
        [SerializeField] private IShipMovement _shipMovement;
        [SerializeField] private IWeapon _playerWeapon;
        [SerializeField] private IShipHyperSpace _hyperSpace;
        [SerializeField] private EntityHealthComponent _entityHealthComponent;

        public IShipMovement ShipMovement => _shipMovement;
        public IWeapon PlayerWeapon => _playerWeapon;
        public IShipHyperSpace HyperSpace => _hyperSpace;
        public EntityHealthComponent EntityHealthComponent => _entityHealthComponent;

        private void Awake()
        {
            if (_shipMovement == null) _shipMovement = GetComponent<IShipMovement>();
            if (_playerWeapon == null) _playerWeapon = GetComponentInChildren<IWeapon>();
            if (_hyperSpace == null) _hyperSpace = GetComponent<IShipHyperSpace>();
            if (_entityHealthComponent == null) _entityHealthComponent = GetComponent<EntityHealthComponent>();
        }

        public void Init()
        {
            _entityHealthComponent.RefillLive(GameEntityTag.UNKNOWN);
        }
    }

}
