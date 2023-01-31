using UnityEngine;
using UnityEngine.XR;

namespace sxr_internal {
    public class CameraTracker : MonoBehaviour {
        [SerializeField] private bool recordCamera = false, recordGaze=false;

        private Camera vrCamera;
        private bool headerPrinted;

        public void WriteCameraTrackerHeader(bool recordingGaze) {
            ExperimentHandler.Instance.WriteHeaderToTaggedFile("camera_tracker", recordingGaze ?
                "xPos,yPos,zPos,xRot,yRot,zRot,gazeScreenPosX,gazeScreenPosY" 
                : "xPos,yPos,zPos,xRot,yRot,zRot"); }
        
        public void StartRecording(bool recordGaze) {
            if (!headerPrinted) WriteCameraTrackerHeader(recordGaze);
            this.recordGaze = recordGaze && XRSettings.enabled;
            recordCamera = true; }

        public void PauseRecording() {
            recordGaze = false;
            recordCamera = false; }

        public bool RecordingGaze() { return recordGaze; }
        
        private void Start()
        {  vrCamera = sxrSettings.Instance.vrCamera; }

        private void Update() {
            if (sxrSettings.Instance.RecordThisFrame() & recordCamera) {
                if (recordGaze)
                    ExperimentHandler.Instance.WriteToTaggedFile("camera_tracker",
                        (vrCamera.gameObject.transform.position.x + "," +
                         vrCamera.gameObject.transform.position.y + "," + 
                         vrCamera.gameObject.transform.position.z + "," +
                         vrCamera.gameObject.transform.rotation.eulerAngles.x + "," +
                         vrCamera.gameObject.transform.rotation.eulerAngles.y + "," + 
                         vrCamera.gameObject.transform.rotation.eulerAngles.z + "," +
                         GazeHandler.Instance.GetScreenFixationPoint()).Replace("(","").Replace(")",""));
                
                else
                    ExperimentHandler.Instance.WriteToTaggedFile("camera_tracker",
                        vrCamera.gameObject.transform.position.x + "," +
                        vrCamera.gameObject.transform.position.y + "," + 
                        vrCamera.gameObject.transform.position.z + "," +
                        vrCamera.gameObject.transform.rotation.eulerAngles.x + "," +
                        vrCamera.gameObject.transform.rotation.eulerAngles.y + "," + 
                        vrCamera.gameObject.transform.rotation.eulerAngles.z); } }

        // Singleton initiated on Awake()
        public static CameraTracker Instance; 
        private void Awake() {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject.transform.root); }
            else  Destroy(gameObject); }
    }
}
