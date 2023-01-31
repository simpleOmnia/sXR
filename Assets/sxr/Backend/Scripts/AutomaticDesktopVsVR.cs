using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace sxr_internal {
    /// <summary>
    /// Attach this to your vrCamera to automatically switch to
    /// headset tracking if a headset is connected. Without a headset,
    /// arrow keys/WASD will move the camera and mouse will rotate
    /// (See SimpleFirstPersonMovement script)
    /// </summary>
    public class AutomaticDesktopVsVR : MonoBehaviour {
#if SXR_USE_AUTOVR
    [SerializeField] float checkFrequency=3; 
	    float lastCheck = 0;
        private SimpleFirstPersonMovement firstPerson;

        void Start() {
            Debug.Log("Using automatic Desktop vs VR");
            firstPerson = gameObject.GetComponent<SimpleFirstPersonMovement>(); }

        void Update(){
            if (Time.time - lastCheck > checkFrequency) 
                CheckHeadset(); }

        /// <summary>
        /// Checks all devices for head-tracking whenever a device is
        /// disconnected/connected
        /// </summary>
        /// <param name="device"></param>
        void CheckHeadset() {
            List<InputDevice> inputDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, inputDevices);
            if (inputDevices.Count > 0 ) 
                StartXR();
            else
                StopXR();

            lastCheck = Time.time; }

        void StartXR() {
            if (gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>() == null) 
                gameObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            firstPerson.enabled = false;
            gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().enabled = true;
                                                                                                      
            if(!XRGeneralSettings.Instance.Manager.activeLoader){
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems(); } }
     
        void StopXR() {
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete) 
                XRGeneralSettings.Instance.Manager.StopSubsystems(); 
            firstPerson.enabled = true;
                
            if (gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>() != null)
                gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().enabled = false; } 
#endif
    }
}
