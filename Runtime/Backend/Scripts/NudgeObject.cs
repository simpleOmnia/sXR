using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Attach to objects to be able to nudge them around during runtime with a keyboard key.
    /// Hold the specified key (set with Inpspector) and use arrow keys to move the object
    /// </summary>
    public class NudgeObject : MonoBehaviour {
        [SerializeField] private KeyCode keycode = KeyCode.Space;
        [SerializeField] private float nudgeSpeed = 1.0f;  

        void Update() {
            var t = transform; 
            if (Input.GetKeyDown(keycode)){
                if(Input.GetKey(KeyCode.UpArrow))
                    t.position +=  Time.deltaTime * nudgeSpeed * t.forward;
                if(Input.GetKey(KeyCode.DownArrow))
                    t.position += Time.deltaTime * nudgeSpeed * -1.0f * t.forward;
                    
                if(Input.GetKey(KeyCode.LeftArrow))
                    t.position += Time.deltaTime * nudgeSpeed * -1.0f * t.forward;
                    
                if(Input.GetKey(KeyCode.RightArrow))
                    t.position += Time.deltaTime * nudgeSpeed * t.forward; } } 
    }
}