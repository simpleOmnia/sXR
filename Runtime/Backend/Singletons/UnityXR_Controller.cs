using UnityEngine;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


namespace sxr_internal {
    public class UnityXR_Controller : ControllerVR {
        private InputDevice leftController, rightController;

        public void SendHaptic(uint chan, float amp, float dur, bool rightHand)
        {
            HapticCapabilities capabilities;
            if ((rightHand & rightController.TryGetHapticCapabilities(out capabilities)) |
                (!rightHand & leftController.TryGetHapticCapabilities(out capabilities)))
            {
                if (capabilities.supportsImpulse)
                    (rightHand ? rightController : leftController).SendHapticImpulse(chan, amp, dur);
                else
                    Debug.Log("Impulse not supported by controller");
            }
            else
            {
                Debug.Log("Unable to detect controller capabilities");
                Debug.Log(rightController); 
            }
        }
        
        private void Update() {

            if(useController){
                leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                if (!rightController.isValid && !leftController.isValid) {
                    sxr.DebugLog("Failed to find Left/Right hand, searching for other GameController", 5000);
                    rightController = InputDevices.GetDeviceAtXRNode(XRNode.GameController); }

                if (!rightController.isValid && !leftController.isValid )
                    sxr.DebugLog("Failed to find VR controller", 5000);

                InputDevice[] controllers = {rightController, leftController}; 
                foreach (var controller in controllers) 
                    if(controller.isValid) {
                        bool rightSide = controller == rightController;
                        if (!controller.TryGetFeatureValue(CommonUsages.triggerButton, out buttonPressed[
                            (int) (rightSide ? sxr_internal.ControllerButton.RH_Trigger : sxr_internal.ControllerButton.LH_Trigger)]))
                            sxr.DebugLog("No trigger found for device: " + controller.name, 10000);

                        if (!controller.TryGetFeatureValue(CommonUsages.gripButton, out buttonPressed[
                            (int) (rightSide ? sxr_internal.ControllerButton.RH_SideButton: sxr_internal.ControllerButton.LH_SideButton)]))
                            sxr.DebugLog("No side button found for device: " + controller.name, 10000);
                        
                        if (!controller.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed[
                            (int) (rightSide ? sxr_internal.ControllerButton.RH_ButtonA : sxr_internal.ControllerButton.LH_ButtonA)]))
                            sxr.DebugLog("No primary button found for device: " + controller.name, 10000);
                        
                        if (!controller.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressed[
                            (int) (rightSide ? sxr_internal.ControllerButton.RH_ButtonB : sxr_internal.ControllerButton.LH_ButtonB)]))
                            sxr.DebugLog("No secondary button found for device: " + controller.name, 10000);

                        if (controller.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool trackPadClicked)) {
                            if (trackPadClicked) {
                                Vector2 trackPad; 
                                if (!controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out trackPad))
                                    sxr.DebugLog("Primary 2D axis clicked but unable to set Vector2 value");
                                else {
                                    if (trackPad.x > .2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr_internal.ControllerButton.RH_TrackPadRight : sxr_internal.ControllerButton.LH_TrackPadRight) ]= true;
                                    if (trackPad.x < -.2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr_internal.ControllerButton.RH_TrackPadLeft : sxr_internal.ControllerButton.LH_TrackPadLeft) ]= true;
                                    if (trackPad.y > .2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr_internal.ControllerButton.RH_TrackPadUp : sxr_internal.ControllerButton.LH_TrackPadUp) ]= true;
                                    if (trackPad.y < -.2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr_internal.ControllerButton.RH_TrackPadDown : sxr_internal.ControllerButton.LH_TrackPadDown) ]= true; } }
                        }

                        buttonPressed[(int) sxr_internal.ControllerButton.Trigger] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_Trigger] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_Trigger];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.SideButton] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_SideButton] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_SideButton];

                        buttonPressed[(int) sxr_internal.ControllerButton.ButtonA] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_ButtonA] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_ButtonA];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.ButtonB] =
                            buttonPressed[(int) sxr_internal.ControllerButton.ButtonB] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_ButtonB];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.TrackPadDown] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_TrackPadDown] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_TrackPadDown];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.TrackPadLeft] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_TrackPadLeft] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_TrackPadLeft];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.TrackPadRight] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_TrackPadRight] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_TrackPadRight];
                        
                        buttonPressed[(int) sxr_internal.ControllerButton.TrackPadUp] =
                            buttonPressed[(int) sxr_internal.ControllerButton.LH_TrackPadUp] ||
                            buttonPressed[(int) sxr_internal.ControllerButton.RH_TrackPadUp];
                    }
            }
        }

        // Singleton initiated on Awake()
        public static UnityXR_Controller Instance;
        private void Awake() {  if ( Instance == null) {Instance = this; DontDestroyOnLoad(gameObject.transform.root);} 
            else Destroy(gameObject);  }
    }
}
