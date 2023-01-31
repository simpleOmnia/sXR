using sxr_internal;
using UnityEngine;
using UnityEngine.UI;

public class OverwriteDisplay : MonoBehaviour
{
   [SerializeField] RenderTexture vrCameraTarget; 
    // Update is called once per frame
    void Update()
    {        
        sxrSettings.Instance.vrCamera.targetTexture = vrCameraTarget;
        UI_Handler.Instance.GetRawImageAtPosition(sxr.UI_Position.VRcamera).texture = vrCameraTarget;
        UI_Handler.Instance.GetRawImageAtPosition(sxr.UI_Position.VRcamera).SetNativeSize(); }
}
