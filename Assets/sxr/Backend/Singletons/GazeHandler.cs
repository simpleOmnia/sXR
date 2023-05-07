//TODO Add gaze collider to record objects in focus

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

namespace sxr_internal {
#if SXR_USE_SRANIPAL
    using ViveSR.anipal.Eye;

    public class GazeHandler : MonoBehaviour {
        public static GazeHandler Instance;

        int lastUpdate; 
        Vector3 gazeOriginCombinedLocal, gazeDirectionCombinedLocal, previousGazeDirectionCombinedLocal;
        VerboseData verboseData = new VerboseData();
        Camera vrCamera; 

        private string toWrite = ""; 

        private bool recordEyeTracker; 
        private bool headerPrinted;

        public void WriteEyeTrackerHeader() {
            ExperimentHandler.Instance.WriteToTaggedFile("eyetracker",
                "screenFixationX,screenFixationY,gazeFixationX,gazeFixationY,gazeFixationZ,leftEyePositionX,"+
                "leftEyePositionY,leftEyePositionZ,rightEyePositionX,rightEyePositionY,rightEyePositionZ," +
                "leftEyeRotationX,leftEyeRotationY,leftEyeRotationZ,rightEyeRotationX,rightEyeRotationY," +
                "rightEyeRotationZ,leftEyePupilSize,rightEyePupilSize,leftEyeOpenAmount,rightEyeOpenAmount");
            headerPrinted=true;}
        
        public void StartRecording() {
            if (!headerPrinted) WriteEyeTrackerHeader();
            recordEyeTracker = true; }

        public void PauseRecording()
        {
            recordEyeTracker = false;
            if(toWrite != "") ExperimentHandler.Instance.WriteToTaggedFile("eyetracker", toWrite, includeTimeStepInfo:false);
            toWrite = ""; 
        }

        public bool RecordingGaze() { return recordEyeTracker; }

        public bool LaunchEyeCalibration()
        {
            if (SRanipal_Eye.LaunchEyeCalibration()) return true; 
            else if (SRanipal_Eye_v2.LaunchEyeCalibration()) return true;
            Debug.Log("Failed to complete eye calibration");
            return false; 
        }

        public string GetFullGazeInfo(){
            return (GetScreenFixationPoint() +","+ GazeFixation() +","+ LeftEyePosition() +","+ RightEyePosition() +","+
                    LeftEyeRotation() +","+ RightEyeRotation() +","+ LeftEyePupilSize() +","+ RightEyePupilSize() + ","+
                    LeftEyeOpenAmount() +","+ RightEyeOpenAmount()).Replace("(","").Replace(")","");
        }

        public void Update() {
            if (sxrSettings.Instance.RecordThisFrame() & recordEyeTracker)
                toWrite += GetFullGazeInfo() + "\n"; }

        void UpdateGaze() {
            if (lastUpdate != sxrSettings.Instance.GetCurrentFrame()) {
                previousGazeDirectionCombinedLocal = gazeDirectionCombinedLocal;
                if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out gazeOriginCombinedLocal, out gazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out gazeOriginCombinedLocal, out gazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out gazeOriginCombinedLocal, out gazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out gazeOriginCombinedLocal,
                    out gazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out gazeOriginCombinedLocal,
                    out gazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out gazeOriginCombinedLocal,
                    out gazeDirectionCombinedLocal)) { }
                else { Debug.LogWarning("Failed to find SRanipal Framework (combinedGazeRayLocal), do you have the SDK installed?"); }
                
                var interpolatedGazeDirection = UnityEngine.Vector3.Lerp(previousGazeDirectionCombinedLocal, gazeDirectionCombinedLocal,
                    sxrSettings.Instance.interpolateAmount * Time.unscaledDeltaTime);
                gazeDirectionCombinedLocal = sxrSettings.Instance.interpolateGaze
                    ? interpolatedGazeDirection.normalized
                    : gazeDirectionCombinedLocal.normalized;

                if (SRanipal_Eye.GetVerboseData(out verboseData)) { }
                else if (SRanipal_Eye_v2.GetVerboseData(out verboseData)) { }
                else { /*Debug.LogWarning("Failed to find SRanipal Framework (verboseData), do you have the SDK installed?");*/ }
            

                sxr.DebugLog(gazeOriginCombinedLocal.ToString());
                sxr.DebugLog(gazeDirectionCombinedLocal.ToString());

                lastUpdate = sxrSettings.Instance.GetCurrentFrame(); 
            } }
        
        public Vector3 GetGazeCombinedGazeRayLocal() {
            UpdateGaze();
            return gazeDirectionCombinedLocal; }
        public Vector3 GetGazeCombinedPositionLocal() {
            UpdateGaze();
            return gazeDirectionCombinedLocal; }

        public Vector2 GetScreenFixationPoint(){
            UpdateGaze();
            Camera vrCamera = sxrSettings.Instance.vrCamera;
            var screenPos =
                vrCamera.WorldToScreenPoint(vrCamera.transform.position + gazeOriginCombinedLocal +
                                                vrCamera.transform.rotation * gazeDirectionCombinedLocal);

            float gazeX = screenPos.x / Screen.currentResolution.width;
            float gazeY = screenPos.y / Screen.currentResolution.height;
            sxr.DebugLog("Gaze: " + gazeX + "," + gazeY);
            return new Vector2(gazeX, gazeY); }

        public float GetCombinedEyeConvergenceDistance(){
            if (verboseData.combined.convergence_distance_validity)
                return verboseData.combined.convergence_distance_mm; 
            sxr.DebugLog("Failed to find combined convergence distance");
            return 0; }

        public Vector3 GazeFixation()
        {
            return vrCamera.transform.position + (GetCombinedEyeConvergenceDistance() * GetGazeCombinedGazeRayLocal()); 
        }
        
        public Vector3 LeftEyePosition() {
            UpdateGaze();
            return verboseData.left.gaze_origin_mm; }

        public Vector3 RightEyePosition() {
            UpdateGaze();
            return verboseData.right.gaze_origin_mm; }
        
        public Vector3 LeftEyeRotation() {
            UpdateGaze();
            return verboseData.left.gaze_direction_normalized; }

        public Vector3 RightEyeRotation() {
            UpdateGaze();
            return verboseData.right.gaze_direction_normalized; }

        public float LeftEyeOpenAmount() {
            UpdateGaze();
            return verboseData.left.eye_openness; }

        public float RightEyeOpenAmount() {
            UpdateGaze();
            return verboseData.right.eye_openness; }

        public float LeftEyePupilSize() {
            UpdateGaze();
            return verboseData.left.pupil_diameter_mm; }

        public float RightEyePupilSize() {
            UpdateGaze();
            return verboseData.right.pupil_diameter_mm; }

        private void OnApplicationQuit(){
            if(headerPrinted && toWrite != "")
                ExperimentHandler.Instance.WriteToTaggedFile("eyetracker", toWrite, includeTimeStepInfo:false);}

        void Start() { 
            vrCamera = sxrSettings.Instance.vrCamera; 
            gameObject.AddComponent<SRanipal_Eye_Framework>(); }
        void Awake() {
             if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
             else Destroy(gameObject); }
    }

#else
    public class GazeHandler : MonoBehaviour {
    
        private InputDevice eyeTracker;
        private Eyes eyesData;
        private Camera vrCamera;

        private string toWrite = ""; 
        private int lastUpdate;
        private bool recordEyeTracker; 
        private bool headerPrinted;

        public void WriteEyeTrackerHeader() {
            ExperimentHandler.Instance.WriteToTaggedFile("eyetracker",
                "screenFixationX,screenFixationY,gazeFixationX,gazeFixationY,gazeFixationZ,leftEyePositionX,"+
                "leftEyePositionY,leftEyePositionZ,rightEyePositionX,rightEyePositionY,rightEyePositionZ," +
                "leftEyeRotationX,leftEyeRotationY,leftEyeRotationZ,rightEyeRotationX,rightEyeRotationY," +
                "rightEyeRotationZ,leftEyePupilSize,rightEyePupilSize,leftEyeOpenAmount,rightEyeOpenAmount");
            headerPrinted=true; }
        
        public void StartRecording() {
            if (!headerPrinted) WriteEyeTrackerHeader();
            recordEyeTracker = true; }

        public void PauseRecording() {
            recordEyeTracker = false;
            if(toWrite != "") ExperimentHandler.Instance.WriteToTaggedFile("eyetracker", toWrite, includeTimeStepInfo:false);
            toWrite = ""; }

        public bool RecordingGaze() { return recordEyeTracker; }

        public string GetFullGazeInfo(){
            return (GetScreenFixationPoint() +","+ GazeFixation() +","+ LeftEyePosition() +","+ RightEyePosition() +","+
                    LeftEyeRotation() +","+ RightEyeRotation() +","+ LeftEyePupilSize() +","+ RightEyePupilSize() + ","+
                    LeftEyeOpenAmount() +","+ RightEyeOpenAmount()).Replace("(","").Replace(")","");
        }

        public void Update() {
            if (sxrSettings.Instance.RecordThisFrame() & recordEyeTracker)
                toWrite += GetFullGazeInfo() + "\n"; }
       
        /// <summary>
        /// Updates eyeData once per frame for use in all methods that can use eyeData.
        /// If eyeData cannot be used, can return primitive gaze pos/rot 
        /// </summary>
        /// <returns></returns>
        bool FindEyeData(){
            if(lastUpdate != sxrSettings.Instance.GetCurrentFrame()){
                lastUpdate = sxrSettings.Instance.GetCurrentFrame(); 
                if (!eyeTracker.isValid) {
                    List<InputDevice> inputDevices = new List<InputDevice>();
                    InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, inputDevices);
                    if (inputDevices.Count > 0) {
                        eyeTracker = inputDevices[0];
                        Debug.Log(inputDevices.Count == 1
                            ? "Detected eyetracker: " + eyeTracker.manufacturer + ", " + eyeTracker.name
                            : "Detected more than one eyetracker, setting to first found: " + eyeTracker.manufacturer + ", " +
                              eyeTracker.name);
                        if (eyeTracker.TryGetFeatureValue(CommonUsages.eyesData, out eyesData))
                            return true;
                        Debug.LogWarning("Detected eyetracker but no eyesData available... Only primitive gaze information available"); } }
                else return true; 

                Debug.LogWarning("Failed to find eye tracker with eyesData Struct, should you be using SRanipal (Vive Pro Eye)?");
                return false; }

            return true; }
        
        
        public Vector3 GazeFixation() {
            if (FindEyeData()) {
                if (eyesData.TryGetFixationPoint(out Vector3 fixationPoint))
                    return fixationPoint;
                sxr.DebugLog("Found eyesData but unable to get fixation point"); }
            else
                sxr.DebugLog("No eyesData available, try GetGazeCombinedGazeRayLocal() and GetGazeCombinedRotationLocal() ");

            return new Vector3(); }
        
        public Vector3 LeftEyePosition() {
            if (FindEyeData()) {
                if (eyesData.TryGetLeftEyePosition(out Vector3 leftPos))
                    return leftPos;
                sxr.DebugLog("Found eyesData but unable to get left eye position"); }
            else
                sxr.DebugLog("No eyesData available");

            return new Vector3(); }

        public Vector3 RightEyePosition() {
            if (FindEyeData()) {
                if (eyesData.TryGetRightEyePosition(out Vector3 rightPos))
                    return rightPos;
                sxr.DebugLog("Found eyesData but unable to get left eye position"); }
            else
                sxr.DebugLog("No eyesData available");

            return new Vector3(); }
        
        public Vector3 LeftEyeRotation() {
            if (FindEyeData()) {
                if (eyesData.TryGetLeftEyeRotation(out Quaternion leftRot))
                    return leftRot.eulerAngles;
                sxr.DebugLog("Found eyesData but unable to get left eye rotation"); }
            else
                sxr.DebugLog("No eyesData available");

            return new Vector3(); }

        public Vector3 RightEyeRotation() {
            if (FindEyeData()) {
                if (eyesData.TryGetRightEyeRotation(out Quaternion rightRot))
                    return rightRot.eulerAngles;
                sxr.DebugLog("Found eyesData but unable to get left eye position"); }
            else
                sxr.DebugLog("No eyesData available");

            return new Vector3(); }

        public float LeftEyeOpenAmount() {
            if (FindEyeData()) {
                 if (eyesData.TryGetLeftEyeOpenAmount(out float leftOpenAmount))
                    return leftOpenAmount;
                 sxr.DebugLog("Found eyesData but unable to get left eye open amount"); }
            else
                sxr.DebugLog("No eyesData available");

            return 0; }

        public float RightEyeOpenAmount() {
            if (FindEyeData()) {
                if (eyesData.TryGetRightEyeOpenAmount(out float rightOpenAmount))
                    return rightOpenAmount;
                sxr.DebugLog("Found eyesData but unable to get left eye position"); }
            else
                sxr.DebugLog("No eyesData available");

            return 0; }

        public Vector3 GetGazeCombinedGazeRayLocal() {
            if (FindEyeData())
                sxr.DebugLog("EyeData found, can likely use GazeFixation()");
            
            List<InputFeatureUsage> inputFeatureUsages = new List<InputFeatureUsage>();
            eyeTracker.TryGetFeatureUsages(inputFeatureUsages);
            foreach(var feature in inputFeatureUsages)
                if(feature.name == "gazeRotation")
                    if (eyeTracker.TryGetFeatureValue(feature.As<Quaternion>(), out Quaternion gazeRot))
                        return gazeRot.eulerAngles; 
            
            sxr.DebugLog("Unable to find gazePosition feature"); 
            return new Vector3(); }

        public Vector3 GetGazeCombinedPositionLocal(){
            if (FindEyeData())
                sxr.DebugLog("EyeData found, can likely use GazeFixation()");
            
            List<InputFeatureUsage> inputFeatureUsages = new List<InputFeatureUsage>();
            eyeTracker.TryGetFeatureUsages(inputFeatureUsages);
            foreach(var feature in inputFeatureUsages)
                if(feature.name == "gazePosition")
                    if (eyeTracker.TryGetFeatureValue(feature.As<Vector3>(), out Vector3 gazePos))
                        return gazePos; 
            
            sxr.DebugLog("Unable to find gazePosition feature"); 
            return new Vector3(); }
        
        public Vector2 GetScreenFixationPoint() {
            Camera vrCamera = sxrSettings.Instance.vrCamera;
            Vector3 screenPos;
            float gazeX, gazeY; 
            
            if (FindEyeData()) {
                screenPos =
                    vrCamera.WorldToScreenPoint(GazeFixation());
        
                gazeX = screenPos.x / Screen.currentResolution.width; 
                gazeY = screenPos.y / Screen.currentResolution.height;
                sxr.DebugLog("Gaze: " + gazeX + "," + gazeY);
                return new Vector2(gazeX, gazeY); }
            
            sxr.DebugLog("Failed to find eyeData, attempting to use primitive gaze position/rotation for screen point");
            
            screenPos =
                vrCamera.WorldToScreenPoint(vrCamera.transform.position + GetGazeCombinedPositionLocal() +
                                            vrCamera.transform.rotation * GetGazeCombinedGazeRayLocal());
            gazeX = screenPos.x / Screen.currentResolution.width;
            gazeY = screenPos.y / Screen.currentResolution.height;
            sxr.DebugLog("Gaze: " + gazeX + "," + gazeY);
            return new Vector2(gazeX, gazeY); }
        
        public float LeftEyePupilSize() {
            sxr.DebugLog("Pupil size not yet available through OpenXR implementation, if your headset " +
                         "supports pupil size, post a feature request on the Github or email sxr.unity@gmail.com");
            return 0; }

        public float RightEyePupilSize() {
            sxr.DebugLog("Pupil size not yet available through OpenXR implementation, if your headset " +
                         "supports pupil size, post a feature request on the Github or email sxr.unity@gmail.com");
            return 0; }

        private void OnApplicationQuit(){
            if(headerPrinted && toWrite != "")
                ExperimentHandler.Instance.WriteToTaggedFile("eyetracker", toWrite, includeTimeStepInfo:false);}

        private void Start() { vrCamera = sxrSettings.Instance.vrCamera; }

        // Singleton initiated on Awake()
        public static GazeHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
#endif
