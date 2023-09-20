using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// /// <summary>
    /// [Singleton]  *In progress* - displays a "STOP" message and plays a sound
    /// Implemented with sxrSettings.safetywalls
    /// </summary>
    public class SafetyHandler : MonoBehaviour {
        [SerializeField] bool displayEmergency; 
        
        public void SafetyMessage(bool enable)  {  displayEmergency = enable;  }

        void Update() {
            var pos = sxrSettings.Instance.vrCamera.gameObject.transform.position; 
            if (displayEmergency 
                & (pos.x > sxrSettings.Instance.distanceBetweenEastWest / 2
                   || pos.x < -sxrSettings.Instance.distanceBetweenEastWest / 2
                   || pos.z > sxrSettings.Instance.distanceBetweenNorthSouth / 2
                   || pos.z < -sxrSettings.Instance.distanceBetweenNorthSouth / 2)){
                SoundHandler.Instance.Stop();
                UI_Handler.Instance.emergencyStop.enabled = true; }
            else { UI_Handler.Instance.emergencyStop.enabled =  false; }

            displayEmergency = false; }

        // Singleton initiated on Awake()
        public static SafetyHandler Instance;
        private void Awake() {  if ( Instance == null) {Instance = this; DontDestroyOnLoad(gameObject.transform.root);} else Destroy(gameObject);  }
    }    
}
