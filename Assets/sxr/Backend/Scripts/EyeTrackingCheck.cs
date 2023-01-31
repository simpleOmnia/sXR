using UnityEngine;

namespace sxr_internal {
/// <summary>
/// Sometimes low battery with wireless devices results in eyetracking failure without disabling the
/// HMD screen.  Informs the participant if the eyetracking cuts out during the experiment and instructs
/// them to tell the experimenter.
/// </summary>
    public class EyeTrackingCheck : MonoBehaviour {
        [SerializeField] private int maxFramesWithoutUpdate;
        private int framesWithoutGazeUpdate;
        private Vector2 lastGazeScreenPos; 

        void Update() {
            if ((CameraTracker.Instance.RecordingGaze() || GazeHandler.Instance.RecordingGaze())
             && Vector2.Angle(lastGazeScreenPos, GazeHandler.Instance.GetScreenFixationPoint())< 1) {
                framesWithoutGazeUpdate++;
                UI_Handler.Instance.eyeError.enabled = framesWithoutGazeUpdate > maxFramesWithoutUpdate; } 
            else {
                framesWithoutGazeUpdate = CameraTracker.Instance.RecordingGaze() ? 0 : framesWithoutGazeUpdate;
                UI_Handler.Instance.eyeError.enabled = false; } } 
    }
}
