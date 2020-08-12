namespace Asteroid
{
    using UnityEngine;

    public class RocketMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float thrustPower = 100;
        public float turnThrustPower = 20;

        private float _hInput = 0;
        private float _vInput = 0;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            ThrustMove();
            ThrustRotation();
        }

        private void ThrustMove()
        {
            rb.AddRelativeForce(Vector2.up * _vInput * Time.deltaTime * thrustPower);
        }

        private void ThrustRotation()
        {
            rb.AddTorque(_hInput * turnThrustPower * Time.deltaTime);
        }

        public void MoveRocket(float vAxis)
        {
            _vInput = vAxis;
        }

        public void RotateRocket(float hAxis)
        {
            _hInput = hAxis;
        }
    }

}
