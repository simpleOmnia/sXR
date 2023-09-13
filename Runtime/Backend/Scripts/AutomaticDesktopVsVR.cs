using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TrackedPoseDriver = UnityEngine.InputSystem.XR.TrackedPoseDriver;

namespace sxr_internal {
    public class AutomaticDesktopVsVR : MonoBehaviour {
#if SXR_USE_AUTOVR
        [SerializeField] private float checkFrequency = 3f;
        private float lastCheck = 0;
        private SimpleFirstPersonMovement firstPerson;
        private TrackedPoseDriver trackedPoseDriver;

        void Start() {
            Debug.Log("Using automatic Desktop vs VR");
            firstPerson = GetComponent<SimpleFirstPersonMovement>();
            if (GetComponent<TrackedPoseDriver>() == null)
                Debug.LogError("sXR - sxr_prefab requires TrackedPoseDriver, make sure you're using the InputSystem " +
                               "package and have set the correct input mode under 'Edit -> Project Settings -> Player' ");
            trackedPoseDriver = GetComponent<TrackedPoseDriver>();
        }

        void Update() {
            if (Time.time - lastCheck > checkFrequency) {
                CheckHeadset();
            }
        }

        void CheckHeadset() {
            List<UnityEngine.XR.InputDevice> inputDevices = new List<UnityEngine.XR.InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, inputDevices);

            if (inputDevices.Count > 0) {
                StartXR();
            } else {
                StopXR();
            }

            lastCheck = Time.time;
        }

        void StartXR() {
            firstPerson.enabled = false;
            trackedPoseDriver.enabled = true;

            if(!XRGeneralSettings.Instance.Manager.activeLoader){
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        void StopXR() {
            if (XRGeneralSettings.Instance !=null && XRGeneralSettings.Instance.Manager.isInitializationComplete) {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
            }

            firstPerson.enabled = true;
            trackedPoseDriver.enabled = false;
        }
#endif
    }
}
