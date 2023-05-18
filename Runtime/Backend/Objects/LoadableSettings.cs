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

    }
}