using UnityEngine;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;


namespace sxr_internal {
    public class OpenXR_Controller : ControllerVR {
        private InputDevice leftController, rightController;

        private void Update() {
            if(useController){
                leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                if (!rightController.isValid && !leftController.isValid) {
                    sxr.DebugLog("Failed to find Left/Right hand, searching for other GameController");
                    rightController = InputDevices.GetDeviceAtXRNode(XRNode.GameController); }

                if (!rightController.isValid && !leftController.isValid )
                    sxr.DebugLog("Failed to find VR controller"); 

                InputDevice[] controllers = {rightController, leftController}; 
                foreach (var controller in controllers) 
                    if(controller.isValid) {
                        bool rightSide = controller == rightController;
                        if (!controller.TryGetFeatureValue(CommonUsages.triggerButton, out buttonPressed[
                            (int) (rightSide ? sxr.ControllerButton.RH_Trigger : sxr.ControllerButton.LH_Trigger)]))
                            sxr.DebugLog("No trigger found for device: " + controller.name);

                        if (!controller.TryGetFeatureValue(CommonUsages.gripButton, out buttonPressed[
                            (int) (rightSide ? sxr.ControllerButton.RH_SideButton: sxr.ControllerButton.LH_SideButton)]))
                            sxr.DebugLog("No side button found for device: " + controller.name);
                        
                        if (!controller.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed[
                            (int) (rightSide ? sxr.ControllerButton.RH_ButtonA : sxr.ControllerButton.LH_ButtonA)]))
                            sxr.DebugLog("No primary button found for device: " + controller.name);
                        
                        if (!controller.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressed[
                            (int) (rightSide ? sxr.ControllerButton.RH_ButtonB : sxr.ControllerButton.LH_ButtonB)]))
                            sxr.DebugLog("No secondary button found for device: " + controller.name);

                        if (controller.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool trackPadClicked)) {
                            if (trackPadClicked) {
                                Vector2 trackPad; 
                                if (!controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out trackPad))
                                    sxr.DebugLog("Primary 2D axis clicked but unable to set Vector2 value");
                                else {
                                    if (trackPad.x > .2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr.ControllerButton.RH_TrackPadRight : sxr.ControllerButton.LH_TrackPadRight) ]= true;
                                    if (trackPad.x < -.2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr.ControllerButton.RH_TrackPadLeft : sxr.ControllerButton.LH_TrackPadLeft) ]= true;
                                    if (trackPad.y > .2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr.ControllerButton.RH_TrackPadUp : sxr.ControllerButton.LH_TrackPadUp) ]= true;
                                    if (trackPad.y < -.2f)
                                        buttonPressed[(int) (rightSide
                                            ? sxr.ControllerButton.RH_TrackPadDown : sxr.ControllerButton.LH_TrackPadDown) ]= true; } }
                        }

                        buttonPressed[(int) sxr.ControllerButton.Trigger] =
                            buttonPressed[(int) sxr.ControllerButton.LH_Trigger] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_Trigger];
                        
                        buttonPressed[(int) sxr.ControllerButton.SideButton] =
                            buttonPressed[(int) sxr.ControllerButton.LH_SideButton] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_SideButton];

                        buttonPressed[(int) sxr.ControllerButton.ButtonA] =
                            buttonPressed[(int) sxr.ControllerButton.LH_ButtonA] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_ButtonA];
                        
                        buttonPressed[(int) sxr.ControllerButton.ButtonB] =
                            buttonPressed[(int) sxr.ControllerButton.ButtonB] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_ButtonB];
                        
                        buttonPressed[(int) sxr.ControllerButton.TrackPadDown] =
                            buttonPressed[(int) sxr.ControllerButton.LH_TrackPadDown] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_TrackPadDown];
                        
                        buttonPressed[(int) sxr.ControllerButton.TrackPadLeft] =
                            buttonPressed[(int) sxr.ControllerButton.LH_TrackPadLeft] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_TrackPadLeft];
                        
                        buttonPressed[(int) sxr.ControllerButton.TrackPadRight] =
                            buttonPressed[(int) sxr.ControllerButton.LH_TrackPadRight] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_TrackPadRight];
                        
                        buttonPressed[(int) sxr.ControllerButton.TrackPadUp] =
                            buttonPressed[(int) sxr.ControllerButton.LH_TrackPadUp] ||
                            buttonPressed[(int) sxr.ControllerButton.RH_TrackPadUp];
                    }
            }
        }

        // Singleton initiated on Awake()
        public static OpenXR_Controller Instance;
        private void Awake() {  if ( Instance == null) {Instance = this; DontDestroyOnLoad(gameObject.transform.root);} 
            else Destroy(gameObject);  }
    }
}
