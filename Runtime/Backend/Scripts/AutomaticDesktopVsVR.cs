using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace sxr_internal
{
    /// <summary>
    /// Attach this to your vrCamera to automatically switch to
    /// headset tracking if a headset is connected. Without a headset,
    /// arrow keys/WASD will move the camera and mouse will rotate
    /// (See SimpleFirstPersonMovement script)
    /// </summary>
    public class AutomaticDesktopVsVR : MonoBehaviour
    {
#if SXR_USE_AUTOVR
        [SerializeField] float checkFrequency = 3;
        private SimpleFirstPersonMovement firstPerson;

        void Start()
        {
            Debug.Log("Using automatic Desktop vs VR");
            firstPerson = gameObject.GetComponent<SimpleFirstPersonMovement>();
            StartCoroutine(CheckHeadset());
        }

        IEnumerator CheckHeadset()
        {
            while (true)
            {
                List<InputDevice> inputDevices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, inputDevices);

                if (inputDevices.Count > 0)
                {
                    StartXR();
                }
                else
                {
                    StopXR();
                }

                yield return new WaitForSeconds(checkFrequency);
            }
        }

        void StartXR()
        {
            firstPerson.enabled = false;
            if (!XRSettings.isDeviceActive)
            {
                XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
                XRGeneralSettings.Instance.Manager.StartSubsystems();
            }
        }

        void StopXR()
        {
            if (XRSettings.isDeviceActive)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
            firstPerson.enabled = true;
        }
#endif
    }
}
