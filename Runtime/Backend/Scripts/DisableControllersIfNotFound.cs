using UnityEngine;
using UnityEngine.XR;

namespace sxr_internal {
    public class DisableControllersIfNotFound : MonoBehaviour {
        private GameObject leftController, rightController; 
        void Start() {
            leftController = sxr.GetObject("LeftController");
            rightController = sxr.GetObject("RightController"); }
        
        void Update() {
#if SXR_USE_STEAMVR
            //TODO update for steamvr controllers
            
#else
            if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).isValid)
                leftController.SetActive(true);
            else
                leftController.SetActive(false);

            if (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).isValid)
                rightController.SetActive(true);
            else
                rightController.SetActive(false); 
#endif
        } 
    }
}
