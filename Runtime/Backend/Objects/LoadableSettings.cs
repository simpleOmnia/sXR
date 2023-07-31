using System.IO;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Contains sxr settings that are saved and loaded
    /// through the sXR toolbar.  
    /// </summary>
    public class LoadableSettings {
        public bool use_autosaver = true;
        public bool use_autoVR = true; 
        public bool use_SRanipal = false;
        public bool use_steamVR = false;
        public bool use_URP = false;
        public bool use_singlePass = false; 
        
        public bool overrideDimensions = false;
        public int overrideX = 1200;
        public int overrideY = 1200; 
        
        public bool interpolateGaze = true;
        public float interpolateAmount = 18.0f;
            
        public bool use_safetyWalls = true;
        public float safetyWallBoundsNS = 1.5f; // north/south
        public float safetyWallBoundsEW = 1.5f; // east/west
        
        public float recordFrequency = 0.1f;
        
        public int debugSetting = 2;
       
        public string dataPath;
        
        public string backupPath; 
        
        public void LoadFromPrefs(){
            dataPath = PlayerPrefs.GetString("sXR_DataPath", "");
            backupPath = PlayerPrefs.GetString("sXR_BackupPath", "");
            use_autosaver = PlayerPrefs.GetInt("sXR_UseAutosaver", 0) ==1;
            use_autoVR = PlayerPrefs.GetInt("sXR_UseAutoVR", 0) ==1;
            use_safetyWalls = PlayerPrefs.GetInt("sXR_UseSafetyWalls", 0) ==1;
            safetyWallBoundsNS = PlayerPrefs.GetFloat("sXR_SafetyWallBoundsNS", 5f);
            safetyWallBoundsEW = PlayerPrefs.GetFloat("sXR_SafetyWallBoundsEW", 5f);
            debugSetting = PlayerPrefs.GetInt("sXR_DebugSetting", 2);
            recordFrequency = PlayerPrefs.GetFloat("sXR_RecordFrequency", 1f);
            use_SRanipal =PlayerPrefs.GetInt("sXR_UseSRanipal", 0) ==1;
            interpolateGaze = PlayerPrefs.GetInt("sXR_InterpolateGaze", 0) ==1;
            interpolateAmount = PlayerPrefs.GetFloat("sXR_InterpolateAmount", 5f);
            use_steamVR = PlayerPrefs.GetInt("sXR_UseSteamVR", 0) ==1;
            use_URP = PlayerPrefs.GetInt("sXR_UseURP", 0) ==1;
            use_singlePass = PlayerPrefs.GetInt("sXR_UseSinglePass", 0) ==1; 
            overrideDimensions = PlayerPrefs.GetInt("sXR_OverrideDims") ==1;
            overrideX = PlayerPrefs.GetInt("sXR_OverrideX");
            overrideY = PlayerPrefs.GetInt("sXR_OverrideY");
        
        }

    }
}