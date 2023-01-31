using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Placed on output camera to set screen resolution,
    /// scales the texture containing the vrCamera view
    /// </summary>
    public class GetScreenDimensions : MonoBehaviour {
        public RenderTexture vrCameraTarget;
        void SetRes() {
            //sxrSettings.Instance.vrCamera.fieldOfView = sxrSettings.Instance.outputCamera.fieldOfView;
            Debug.Log("Using resolution: " + Screen.currentResolution.width + ", " + Screen.currentResolution.height); 
            vrCameraTarget = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 32);
            sxrSettings.Instance.vrCamera.targetTexture = vrCameraTarget;
            UI_Handler.Instance.GetRawImageAtPosition(sxr.UI_Position.VRcamera).texture = vrCameraTarget;
            UI_Handler.Instance.GetRawImageAtPosition(sxr.UI_Position.VRcamera).SetNativeSize(); }

        void Update() {
            if (sxrSettings.Instance.GetCurrentFrame() == 5)
                SetRes(); } 
    }
}
