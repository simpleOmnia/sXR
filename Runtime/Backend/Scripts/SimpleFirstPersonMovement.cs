using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Automatically enabled when headset not detected. Allows for easy first person controls,
    /// looks around with mouse and moves with arrows/WASD. Escape to pause controls
    /// On Update:
    ///     (Listens for controls and moves object accordingly)
    /// </summary>
    public class SimpleFirstPersonMovement : MonoBehaviour {
        [SerializeField] float mouseSensitivity = 100.0f;
        [SerializeField] float movementSpeed = 2.5f;
        [SerializeField] bool fixedVerticalHeadRotation; 

        Vector2 rotation = Vector2.zero; 
        bool active = true; 
        
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                active = !active;
                Debug.Log(!active ? "Escape button pressed, activated movement" : "Escape button pressed, de-activated movement"); }

            if (active) {
                var t = transform; 
                rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                rotation.x -= fixedVerticalHeadRotation ? 0 : Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                t.eulerAngles = (Vector2) rotation;
                
                if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    t.position += t.forward * Time.deltaTime * movementSpeed;
                
                if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                    t.position += t.forward * Time.deltaTime * movementSpeed * -1.0f;
                
                if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    t.position += t.right * Time.deltaTime * movementSpeed * -1.0f;
                
                if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    t.position += t.right * Time.deltaTime * movementSpeed; } } 
    }
}