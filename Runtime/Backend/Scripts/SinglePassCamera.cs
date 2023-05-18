using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Placed on output camera to set screen resolution,
    /// scales the texture containing the vrCamera view
    /// </summary>
    public class SinglePassCamera : MonoBehaviour {

        public RenderTexture vrCameraTarget;
        void SetRes() {
            //sxrSettings.Instance.vrCamera.fieldOfView = sxrSettings.Instance.outputCamera.fieldOfView;
            Vector2 dims = (PlayerPrefs.GetInt("sXR_OverrideDims") == 1
                ? new Vector2(PlayerPrefs.GetInt("sXR_OverrideX"), PlayerPrefs.GetInt("sXR_OverrideY"))
                : new Vector2(Screen.width, Screen.height)); 
            Debug.Log("Using resolution: " + dims.x + ", " + dims.y); 
            vrCameraTarget = new RenderTexture((int)dims.x, (int)dims.y, 32);
            sxrSettings.Instance.vrCamera.targetTexture = vrCameraTarget;
            UI_Handler.Instance.GetRawImageAtPosition(UI_Position.VRcamera).texture = vrCameraTarget;
            UI_Handler.Instance.GetRawImageAtPosition(UI_Position.VRcamera).SetNativeSize(); }

        void Update() {
#if SXR_USE_SINGLE_PASS
            if (sxrSettings.Instance.GetCurrentFrame() == 5)
                SetRes(); 
#else
            UI_Handler.Instance.GetRawImageAtPosition(UI_Position.VRcamera).enabled = false; 
#endif
        } 

                

    }
}
