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

        void LoadFromPreferences() {
            string savedSettingsPath = Application.dataPath + Path.DirectorySeparatorChar +
                                       "sxr" + Path.DirectorySeparatorChar + "Editor" +
                                       Path.DirectorySeparatorChar + "sxrSettings.json";
            LoadableSettings loadableSettings  = new LoadableSettings();
            
            loadableSettings.dataPath = PlayerPrefs.GetString("sXR_DataPath", "");
            loadableSettings.backupPath = PlayerPrefs.GetString("sXR_BackupPath", "");
            loadableSettings.use_autosaver = PlayerPrefs.GetInt("sXR_UseAutosaver", 0) ==1;
            loadableSettings.use_autoVR = PlayerPrefs.GetInt("sXR_UseAutoVR", 0) ==1;
            loadableSettings.use_safetyWalls = PlayerPrefs.GetInt("sXR_UseSafetyWalls", 0) ==1;
            loadableSettings.safetyWallBoundsNS = PlayerPrefs.GetFloat("sXR_SafetyWallBoundsNS", 5f);
            loadableSettings.safetyWallBoundsEW = PlayerPrefs.GetFloat("sXR_SafetyWallBoundsEW", 5f);
            loadableSettings.debugSetting = PlayerPrefs.GetInt("sXR_DebugSetting", 2);
            loadableSettings.recordFrequency = PlayerPrefs.GetFloat("sXR_RecordFrequency", 1f);
            loadableSettings.use_SRanipal =PlayerPrefs.GetInt("sXR_UseSRanipal", 0) ==1;
            loadableSettings.interpolateGaze = PlayerPrefs.GetInt("sXR_InterpolateGaze", 0) ==1;
            loadableSettings.interpolateAmount = PlayerPrefs.GetFloat("sXR_InterpolateAmount", 5f);
            loadableSettings.use_steamVR = PlayerPrefs.GetInt("sXR_UseSteamVR", 0) ==1;
            loadableSettings.use_URP = PlayerPrefs.GetInt("sXR_UseURP", 0) ==1;
            loadableSettings.use_singlePass = PlayerPrefs.GetInt("sXR_UseSinglePass", 0) ==1; 


            safetyMessage = loadableSettings.use_safetyWalls;
            distanceBetweenNorthSouth = loadableSettings.safetyWallBoundsNS;
            distanceBetweenEastWest = loadableSettings.safetyWallBoundsEW;

            interpolateGaze = loadableSettings.interpolateGaze;
            interpolateAmount = loadableSettings.interpolateAmount;

            recordFrequency = loadableSettings.recordFrequency;

            debugMode = (DebugMode) loadableSettings.debugSetting;

            subjectDataDirectory = loadableSettings.dataPath;
            backupDataDirectory = loadableSettings.backupPath; }
            

        // Singleton initiated on Awake()
        public static sxrSettings Instance; 
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject);}
            else Destroy(gameObject); 
            
            if (!vrCamera)
                vrCamera = gameObject.transform.Find("vrCameraAssembly").GameObject().GetComponentInChildren<Camera>(); 
            if(usePreviousSettings)
                LoadFromPreferences(); }
    }
}   
