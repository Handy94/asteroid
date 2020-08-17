namespace Asteroid
{
    using Sirenix.OdinInspector;

    public class PlayerShipComponent : SerializedMonoBehaviour
    {
        public ShipMovement ShipMovement;
        public IWeapon PlayerWeapon;

        private void Awake()
        {
            if (PlayerWeapon == null) PlayerWeapon = GetComponentInChildren<IWeapon>();
        }
    }

}
