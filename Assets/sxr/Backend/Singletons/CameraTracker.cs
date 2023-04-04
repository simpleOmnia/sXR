using UnityEngine;
using UnityEngine.XR;

namespace sxr_internal {
    public class CameraTracker : MonoBehaviour {
        [SerializeField] private bool recordCamera = false, recordGaze=false;

        private Camera vrCamera;
        private bool headerPrinted;
        private string toWrite; 

        public void WriteCameraTrackerHeader(bool recordingGaze) {
            ExperimentHandler.Instance.WriteHeaderToTaggedFile("camera_tracker", recordingGaze ?
                "xPos,yPos,zPos,xRot,yRot,zRot,gazeScreenPosX,gazeScreenPosY" 
                : "xPos,yPos,zPos,xRot,yRot,zRot");
            headerPrinted = true; 
        }
        
        public void StartRecording(bool recordGaze) {
            if (!headerPrinted) WriteCameraTrackerHeader(recordGaze);
            this.recordGaze = recordGaze && XRSettings.enabled;
            recordCamera = true; }

        public void PauseRecording() {
            recordGaze = false;
            recordCamera = false;
            ExperimentHandler.Instance.WriteToTaggedFile("camera_tracker", toWrite);
            toWrite = ""; 
        }

        public bool RecordingGaze() { return recordGaze; }
        
        private void Start()
        {  vrCamera = sxrSettings.Instance.vrCamera; }

        private void Update()
        {
            var trans = vrCamera.transform;
            var pos = trans.position;
            var rot = trans.rotation; 
            if (sxrSettings.Instance.RecordThisFrame() & recordCamera) {
                if (recordGaze)
                    toWrite+= ExperimentHandler.Instance.timeStepToWriteInfo() +
                              (pos.x + "," +
                               pos.y + "," + 
                               pos.z + "," +
                               rot.eulerAngles.x + "," +
                               rot.eulerAngles.y + "," + 
                               rot.eulerAngles.z + "," +
                               GazeHandler.Instance.GetScreenFixationPoint()).Replace("(","").Replace(")","").Replace(" ","")+"\n";
                
                else
                    toWrite += ExperimentHandler.Instance.timeStepToWriteInfo() +
                               pos.x + "," +
                               pos.y + "," + 
                               pos.z + "," +
                               rot.eulerAngles.x + "," +
                               rot.eulerAngles.y + "," + 
                               rot.eulerAngles.z + "\n"; } }

        private void OnApplicationQuit(){
            if(toWrite != "")
                ExperimentHandler.Instance.WriteToTaggedFile("camera_tracker", toWrite);}
        
        // Singleton initiated on Awake()
        public static CameraTracker Instance; 
        private void Awake() {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject.transform.root); }
            else  Destroy(gameObject); }
    }
}
