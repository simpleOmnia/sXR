using sxr_internal;
using UnityEngine;

namespace ShaderSampleScene
{
    public class ShadersSample : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (sxrSettings.Instance.GetCurrentFrame() == 0) sxr.StartExperiment("ShadersSample", "0");
            sxr.StartRecordingCameraPos(true);
            sxr.StartRecordingEyeTrackerInfo();
            sxr.DisplayText(sxr.GetGazeScreenPos().x + "," + sxr.GetGazeScreenPos().y);
            sxr.SetShaderVariables("Shift", "gazeX", GazeHandler.Instance.GetScreenFixationPoint().x);
            sxr.SetShaderVariables("Shift", "gazeY", GazeHandler.Instance.GetScreenFixationPoint().y);
        }
    }
}
