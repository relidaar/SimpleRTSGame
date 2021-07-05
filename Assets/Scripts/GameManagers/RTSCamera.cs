using UnityEngine;

namespace GameManagers
{
    public class RTSCamera : SimpleRTSCamera
    {
        [SerializeField] protected float rotateSpeed;

        private Vector2 _mousePosition;

        protected override void Update()
        {
            base.Update();
        
            if (Input.GetMouseButtonDown(2))
            {
                _mousePosition = Input.mousePosition;
            }

            if (!Input.GetMouseButton(2)) return;
        
            Vector2 tempMousePosition = Input.mousePosition;
            float dx = (tempMousePosition - _mousePosition).x * rotateSpeed;
            float dy = (tempMousePosition - _mousePosition).y * rotateSpeed;
            
            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));
            _mousePosition = tempMousePosition;
        }
    }
}