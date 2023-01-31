using UnityEngine;
using UnityEngine.XR;

namespace sxr_internal {
    /// <summary>
    /// Handles only outputting to one eye. Creates a second camera that renders black
    /// and sends it to override previous frame in opposite eye. 
    /// </summary>
    public class EyeballSelection : MonoBehaviour {
        private GameObject child; 
        private Camera oppositeEye;

        void Update() {
            sxrSettings settings = sxrSettings.Instance; 
            Camera mainCam = gameObject.GetComponentInChildren<Camera>();
            mainCam.stereoTargetEye = (settings.eyeSelection==sxrSettings.EyeSelect.Both) ? StereoTargetEyeMask.Both : 
                (settings.eyeSelection==sxrSettings.EyeSelect.Left) ? StereoTargetEyeMask.Left : StereoTargetEyeMask.Right; 
            oppositeEye.enabled = XRSettings.enabled && (settings.eyeSelection!=sxrSettings.EyeSelect.Both);
            oppositeEye.stereoTargetEye = (settings.eyeSelection==sxrSettings.EyeSelect.Both) ? StereoTargetEyeMask.None :
                (settings.eyeSelection==sxrSettings.EyeSelect.Left) ? StereoTargetEyeMask.Right : StereoTargetEyeMask.Left; }

        void Start() {
            child = new GameObject();
            child.transform.parent = gameObject.transform; 
            child.AddComponent<Camera>();
            oppositeEye = child.GetComponent<Camera>(); 
            oppositeEye.cullingMask = 0;
            oppositeEye.clearFlags = CameraClearFlags.SolidColor;
            oppositeEye.backgroundColor = Color.black; } 
    }
}