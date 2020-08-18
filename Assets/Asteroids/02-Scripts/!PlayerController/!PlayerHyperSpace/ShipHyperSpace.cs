namespace Asteroid
{
    using System.Collections;
    using UnityEngine;

    public class ShipHyperSpace : MonoBehaviour, IShipHyperSpace
    {
        [SerializeField] private Vector2 minRandomViewportPosition;
        [SerializeField] private Vector2 maxRandomViewportPosition;
        [SerializeField] private float delaySpawnInSeconds = 2;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Collider2D shipCollider;
        [SerializeField] private SpriteRenderer shipSprite;

        private Camera _mainCamera;

        public bool IsOnHyperSpace { get; private set; } = false;

        private void Awake()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
        }

        public async void DoHyperSpace(System.Action onDone)
        {
            if (IsOnHyperSpace) return;
            IsOnHyperSpace = true;

            StartCoroutine(Coroutine_HyperSpace());

            onDone?.Invoke();
        }

        private void SetShipAppear(bool isAppear)
        {
            shipCollider.enabled = isAppear;
            shipSprite.enabled = isAppear;
        }

        private void RandomizePosition()
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            Vector2 randomViewportPos = Vector2.zero;
            randomViewportPos.x = Random.Range(minRandomViewportPosition.x, maxRandomViewportPosition.x);
            randomViewportPos.y = Random.Range(minRandomViewportPosition.y, maxRandomViewportPosition.y);

            Vector2 worldPos = _mainCamera.ViewportToWorldPoint(randomViewportPos);
            rb.MovePosition(worldPos);
        }

        private IEnumerator Coroutine_HyperSpace()
        {
            SetShipAppear(false);

            yield return new WaitForSeconds(delaySpawnInSeconds);

            RandomizePosition();

            yield return new WaitForFixedUpdate();

            SetShipAppear(true);

            IsOnHyperSpace = false;
        }
    }

}
