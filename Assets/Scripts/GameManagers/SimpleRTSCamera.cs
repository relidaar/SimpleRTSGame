using UnityEngine;

namespace GameManagers
{
    public class SimpleRTSCamera : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] protected float speedModifier;
    
        [SerializeField] protected float zoomSpeed;
        [SerializeField] protected float zoomSpeedModifier;
    
        [SerializeField] protected float minZoom;
        [SerializeField] protected float maxZoom;

        private float _speed;
        private float _zoomSpeed;

        protected virtual void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speed = speed * speedModifier;
                _zoomSpeed = zoomSpeed * zoomSpeedModifier;
            }
            else
            {
                _speed = speed;
                _zoomSpeed = zoomSpeed;
            }

            if (transform.position.y < maxZoom)
            {
                transform.position = new Vector3(transform.position.x, maxZoom, transform.position.z);
            }
            if (transform.position.y > minZoom)
            {
                transform.position = new Vector3(transform.position.x, minZoom, transform.position.z);
            }

            float horizontalSpeed = transform.position.y * _speed * Input.GetAxis("Horizontal");
            float verticalSpeed = transform.position.y * _speed * Input.GetAxis("Vertical");
            float scrollSpeed = Mathf.Log(transform.position.y) * -_zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

            if (transform.position.y >= minZoom && scrollSpeed > 0 ||
                transform.position.y <= maxZoom && scrollSpeed < 0)
            {
                scrollSpeed = 0;
            }

            if (transform.position.y + scrollSpeed > minZoom)
            {
                scrollSpeed = minZoom - transform.position.y;
            }
            else if (transform.position.y + scrollSpeed < maxZoom)
            {
                scrollSpeed = maxZoom - transform.position.y;
            }

            var verticalMove = new Vector3(0, scrollSpeed, 0);
            var lateralMove = horizontalSpeed * transform.right;
            var forwardMove = transform.forward;
            forwardMove.y = 0;
            forwardMove.Normalize();
            forwardMove *= verticalSpeed;

            var move = verticalMove + lateralMove + forwardMove;
            transform.position += move;
        }
    }
}