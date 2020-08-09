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

        public void ThrustMove()
        {
            rb.AddRelativeForce(Vector2.up * _vInput * Time.deltaTime * thrustPower);
        }

        public void ThrustRotation()
        {
            rb.AddTorque(_hInput * turnThrustPower * Time.deltaTime);
        }

        public void ThrustForward()
        {
            _vInput = 1;
        }

        public void StopThrustForward()
        {
            _vInput = 0;
        }

        public void ThrustLeft()
        {
            _hInput = 1;
        }

        public void ThrustRight()
        {
            _hInput = -1;
        }

        public void StopThrustRotation()
        {
            _hInput = 0;
        }
    }

}
