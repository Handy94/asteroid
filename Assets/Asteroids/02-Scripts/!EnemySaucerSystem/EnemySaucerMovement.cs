namespace Asteroid
{
    using UnityEngine;

    public class EnemySaucerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed = 1;

        private Vector2 _targetPosition;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }

        private void MoveToTarget()
        {
            Vector2 direction = _targetPosition - (Vector2)transform.position;
            Vector2 normalizedDirection = direction.normalized;

            rb.velocity = normalizedDirection * moveSpeed;
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        public bool IsArriveAtTarget()
        {
            return Vector2.Distance(transform.position, _targetPosition) <= 0.1f;
        }
    }

}
