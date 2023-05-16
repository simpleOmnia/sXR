using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace sxr_internal {
    public class sxrSettings : MonoBehaviour {

        public bool usePreviousSettings = true; 
        
        public enum EyeSelect{Both, Left, Right}

        public enum DebugMode {Off, Framewise, Frequent }
        
        public DebugMode debugMode;

        [Header("\n For manually assigning data output directory, \nsubjectData is automatically stored " +
                "at \nAssets/Experiments/<Experiment Name>/...")]
        public string subjectDataDirectory;
        public string backupDataDirectory;

        [Header("How often to record HMD position and eyetracker \ninformation (when enabled)")]
        public float recordFrequency = .1f;

        [Tooltip("Automatically updates based on frequency,\ncan be used to manually record a single frame")]
        public int recordFrame; 
        
        [Header("\n Allows for display to only render in one eye.")]
        public EyeSelect eyeSelection = EyeSelect.Both;
       
        [Header("If enabled will display a warning and play a sound \nif participant reaches specified bounds")]
        public bool safetyMessage = true;
        [Tooltip("Distance between walls in meters")]
        public float distanceBetweenNorthSouth = 10;
        [Tooltip("Distance between walls in meters")]
        public float distanceBetweenEastWest = 10;
        
        [Header("Custom sound files for sxr.PlaySound()")]
        public List<AudioClip> audioClips;

        [Header("Gaze Tracking")] public bool interpolateGaze = true;
        public float interpolateAmount = 18.0f;

        public Camera vrCamera;

        private int currentFrame = 0;
        public int GetCurrentFrame() { return currentFrame; }

        private float lastRecord;

        private void Update() {
            currentFrame++;
            if (Time.time - lastRecord > recordFrequency) {
                recordFrame = currentFrame + 1;
                lastRecord = Time.time; } }

        public bool RecordThisFrame() { return currentFrame == recordFrame; }

        void LoadFromJson() {
            string savedSettingsPath = Application.dataPath + Path.DirectorySeparatorChar +
                                       "sxr" + Path.DirectorySeparatorChar + "Editor" +
                                       Path.DirectorySeparatorChar + "sxrSettings.json";
            LoadableSettings loadableSettings  = new LoadableSettings();
            if (File.Exists(savedSettingsPath)) {
                Debug.Log("Loaded settings from sxr/Editor json file");
                string loadedSettings = File.ReadAllText(savedSettingsPath);
                loadableSettings = JsonUtility.FromJson<LoadableSettings>(loadedSettings);


                safetyMessage = loadableSettings.use_safetyWalls;
                distanceBetweenNorthSouth = loadableSettings.safetyWallBoundsNS;
                distanceBetweenEastWest = loadableSettings.safetyWallBoundsEW;

                interpolateGaze = loadableSettings.interpolateGaze;
                interpolateAmount = loadableSettings.interpolateAmount;

                recordFrequency = loadableSettings.recordFrequency;

                debugMode = (DebugMode) loadableSettings.debugSetting;

                subjectDataDirectory = loadableSettings.dataPath;
                backupDataDirectory = loadableSettings.backupPath; }
            else
                Debug.Log("No saved editor settings detected.  To change features like autosaving the " +
                          "scene or using manual data paths, see the 'sxr' tab on the toolbar"); }

        // Singleton initiated on Awake()
        public static sxrSettings Instance; 
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject);}
            else Destroy(gameObject); 
            
            if (!vrCamera)
                vrCamera = gameObject.transform.Find("vrCameraAssembly").GameObject().GetComponentInChildren<Camera>(); 
            if(usePreviousSettings)
                LoadFromJson(); }
    }
}   
