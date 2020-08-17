namespace Asteroid
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class PlayerShipComponent : SerializedMonoBehaviour
    {
        [SerializeField] private IShipMovement _shipMovement;
        [SerializeField] private IWeapon _playerWeapon;

        public IShipMovement ShipMovement => _shipMovement;
        public IWeapon PlayerWeapon => _playerWeapon;

        private void Awake()
        {
            if (_shipMovement == null) _shipMovement = GetComponent<IShipMovement>();
            if (_playerWeapon == null) _playerWeapon = GetComponentInChildren<IWeapon>();
        }
    }

}
