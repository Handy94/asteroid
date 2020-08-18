namespace Asteroid
{
    using UnityEngine;

    public class WeaponAimer : MonoBehaviour
    {
        public void AimToTarget(Vector2 target)
        {
            Vector2 targetDirection = target - (Vector2)transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

}
