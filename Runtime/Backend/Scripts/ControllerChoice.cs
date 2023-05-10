using UnityEngine;

namespace sxr_internal {
    public class ControllerChoice : MonoBehaviour {
        // Start is called before the first frame update}
        void Start() {
            #if SXR_USE_STEAMVR
            sxr.GetObject("LeftControllerSteamVR").SetActive(true); 
            sxr.GetObject("RightControllerSteamVR").SetActive(true); 
            sxr.GetObject("LeftControllerOpenXR").SetActive(false); 
            sxr.GetObject("RightControllerOpenXR").SetActive(false); 
            #else
            sxr.GetObject("LeftControllerOpenXR").SetActive(true); 
            sxr.GetObject("RightControllerOpenXR").SetActive(true); 
            sxr.GetObject("LeftControllerSteamVR").SetActive(false); 
            sxr.GetObject("RightControllerSteamVR").SetActive(false); 
            #endif
        } 
    }
}