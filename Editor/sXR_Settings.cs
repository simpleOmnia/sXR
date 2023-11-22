using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace sxr_internal{
    public class sXR_Settings : EditorWindow 
    {
       private RectOffset rctOffButton, rctOffTextField, rctOffToggle, rctOffSlider;

        private GUIStyle myStyle;
        
        private LoadableSettings loadableSettings = new LoadableSettings();

        private bool initialLoad = true;

        [MenuItem("sXR/sXR Settings")]
        static void Init() {
            sXR_Settings window = (sXR_Settings)EditorWindow.GetWindow(typeof(sXR_Settings));
            window.Show(); }

    void SaveToPrefs() {
        PlayerPrefs.SetString("sXR_DataPath", loadableSettings.dataPath);
        PlayerPrefs.SetString("sXR_BackupPath", loadableSettings.backupPath);
        PlayerPrefs.SetInt("sXR_UseAutosaver", loadableSettings.use_autosaver ? 1:0);
        PlayerPrefs.SetInt("sXR_UseAutoVR", loadableSettings.use_autoVR ? 1:0);
        PlayerPrefs.SetInt("sXR_UseStartScreen", loadableSettings.use_startscreen ? 1:0);
        PlayerPrefs.SetInt("sXR_UseSafetyWalls", loadableSettings.use_safetyWalls ? 1:0);
        PlayerPrefs.SetFloat("sXR_SafetyWallBoundsNS", loadableSettings.safetyWallBoundsNS);
        PlayerPrefs.SetFloat("sXR_SafetyWallBoundsEW", loadableSettings.safetyWallBoundsEW);
        PlayerPrefs.SetInt("sXR_DebugSetting", loadableSettings.debugSetting);
        PlayerPrefs.SetFloat("sXR_RecordFrequency", loadableSettings.recordFrequency);
        PlayerPrefs.SetInt("sXR_UseSRanipal", loadableSettings.use_SRanipal ? 1:0);
        PlayerPrefs.SetInt("sXR_InterpolateGaze", loadableSettings.interpolateGaze ? 1:0);
        PlayerPrefs.SetFloat("sXR_InterpolateAmount", loadableSettings.interpolateAmount);
        PlayerPrefs.SetInt("sXR_UseSteamVR", loadableSettings.use_steamVR ? 1:0);
        PlayerPrefs.SetInt("sXR_UseURP", loadableSettings.use_URP ? 1:0);
        PlayerPrefs.SetInt("sXR_UseSinglePass", loadableSettings.use_singlePass ? 1:0);
        PlayerPrefs.SetInt("sXR_OverrideDims", loadableSettings.overrideDimensions ? 1:0);
        PlayerPrefs.SetInt("sXR_OverrideX", loadableSettings.overrideX );
        PlayerPrefs.SetInt("sXR_OverrideY", loadableSettings.overrideY );
    }
        
        void OnGUI()
        {
            if (initialLoad)
            {
                initialLoad = false;
                ResourcesProvider.CopyResourcesFromPackageToProject(); 
                loadableSettings.LoadFromPrefs();
                AddDefines();
                Debug.Log("Loaded sXR settings"); 
            }
            rctOffButton = GUI.skin.button.margin;
            rctOffButton.left = 25;
            rctOffButton.right = 25;

            rctOffTextField = GUI.skin.textField.margin;
            rctOffTextField.left = 25;
            rctOffTextField.right = 25;
            
            rctOffToggle = GUI.skin.toggle.margin;
            rctOffToggle.left = 10;

            rctOffSlider = GUI.skin.horizontalSlider.margin;
            rctOffSlider.left = 25;
            rctOffSlider.right = 25;

            rctOffToggle = GUI.skin.toggle.margin;

            myStyle = new GUIStyle(GUI.skin.label) {fontSize = 25};
            
            GUILayout.Label("Settings for simpleXR\n", myStyle);

            EditorGUI.BeginChangeCheck();
            
            GUILayout.Label(new GUIContent("Data path: ", "Sets automatically if left empty, can be used to manually specify output path"));
            loadableSettings.dataPath = GUILayout.TextField(loadableSettings.dataPath); 
            
            GUILayout.Space(5);
            GUILayout.Label(new GUIContent("Backup data path: ", "Sets automatically if left empty, can be used to manually specify output path"));
            loadableSettings.backupPath = GUILayout.TextField(loadableSettings.backupPath);
            
            GUILayout.Space(10);
            loadableSettings.use_autosaver = GUILayout.Toggle(loadableSettings.use_autosaver,
                new GUIContent("   Use Autosaver to save scenes on play",
                    "Automatically saves the scene and asset library when Editor Play button is pressed"));
            
            GUILayout.Space(10);
            loadableSettings.use_autoVR = GUILayout.Toggle(loadableSettings.use_autoVR,
                new GUIContent("   Automatically switch to headset when connected",
                    "Automatically switches to a device with head-tracking if one is available"));
            
            GUILayout.Space(10);
            loadableSettings.use_startscreen = GUILayout.Toggle(loadableSettings.use_startscreen,
                new GUIContent("   Use Start Screen",
                    "When unselected, will automatically start with subject/phase/block/trial as 0"));
            
            GUILayout.Space(10);
            loadableSettings.use_safetyWalls = GUILayout.Toggle(loadableSettings.use_safetyWalls,
                new GUIContent("   Display \"Stop\" message when close to border",
                    "Plays a sound and displays a full-screen red page telling participant to stop"));
            GUILayout.Space(15);
            GUILayout.Label(new GUIContent("North/South boundary (Z axis) ["+ $"{loadableSettings.safetyWallBoundsNS:0.00}" +"]: ", 
                "Sets the distance between the north and south safety boundaries"));
            loadableSettings.safetyWallBoundsNS = GUILayout.HorizontalSlider(loadableSettings.safetyWallBoundsNS, 1f, 20); 
            GUILayout.Space(15);
            GUILayout.Label(new GUIContent("East/West boundary  ["+ $"{loadableSettings.safetyWallBoundsEW:0.00}" +"]: ", 
                "Sets the distance between the east and west (X axis) safety boundaries"));
            loadableSettings.safetyWallBoundsEW = GUILayout.HorizontalSlider(loadableSettings.safetyWallBoundsEW, 1f, 20);

            GUILayout.Space(10);
            GUILayout.Label(new GUIContent("Debug Mode: ", "Sets the frequency of debug messages from sXR. Recommended: Frequent"));
            loadableSettings.debugSetting = GUILayout.Toolbar(loadableSettings.debugSetting, new string[]{"Off", "Framewise", "Frequent"});

            GUILayout.Label(new GUIContent("Camera/Eye-tracking recording frequency ["+ $"{loadableSettings.recordFrequency:0.00}" +"]: ", 
                "Sets how often to record the camera position/eye-tracking information"));
            
            loadableSettings.recordFrequency = GUILayout.HorizontalSlider(loadableSettings.recordFrequency, 0, 2); 
            
            GUILayout.Space(15);
            loadableSettings.use_SRanipal = GUILayout.Toggle(loadableSettings.use_SRanipal,
                new GUIContent("   Use SRanipal",
                    "Enable if using Vive Pro Eye or other HTC equipment that utilizes SRanipal"));
            GUILayout.Space(10);
            loadableSettings.interpolateGaze = GUILayout.Toggle(loadableSettings.interpolateGaze,
                new GUIContent("   Interpolate Gaze",
                    "Enable to decrease eye-tracking jitter by interpolating over frames"));
            GUILayout.Label(new GUIContent("Interpolate Amount ["+ $"{loadableSettings.interpolateAmount:0.00}" +"]: ", 
                "Sets how often to record the camera position/eye-tracking information"));
            loadableSettings.interpolateAmount = GUILayout.HorizontalSlider(loadableSettings.interpolateAmount, 1, 30); 
           
            GUILayout.Space(20);
            loadableSettings.use_steamVR = GUILayout.Toggle(loadableSettings.use_steamVR,
                new GUIContent("   Use SteamVR Controller Bindings",
                    "DEPRECATED. Enable to use SteamVR controller bindings, can make some devices unreachable"));
            
            GUILayout.Space(20);
            loadableSettings.use_URP = GUILayout.Toggle(loadableSettings.use_URP,
                new GUIContent("   Use Universal Rendering Pipeline (URP)",
                    "Enable to use URP"));
            
            GUILayout.Space(20);
            loadableSettings.use_singlePass = GUILayout.Toggle(loadableSettings.use_singlePass,
                new GUIContent("   Use Single Pass Rendering",
                    "Enable to use single pass, will lose stereoscopic effect for higher performance in some scenarios."));

            if (loadableSettings.use_singlePass)
            {
                GUILayout.Space(15);
                loadableSettings.overrideDimensions = GUILayout.Toggle(loadableSettings.overrideDimensions,
                    new GUIContent("   Override render dimensions",
                        "Enable to use override single pass render dimensions."));

                if (loadableSettings.overrideDimensions)
                {
                    GUILayout.Space(15);
                    GUILayout.Label(new GUIContent("X-dimension[" + $"{loadableSettings.overrideX:0}" + "]: ",
                        "Sets the x dimension to be rendered to"));
                    int overrideXInt = Mathf.RoundToInt(loadableSettings.overrideX);
                    overrideXInt = Mathf.RoundToInt(GUILayout.HorizontalSlider(overrideXInt, 500, 2000));
                    loadableSettings.overrideX = overrideXInt;

                    GUILayout.Space(15);
                    GUILayout.Label(new GUIContent("Y-dimension[" + $"{loadableSettings.overrideY:0}" + "]: ",
                        "Sets the y dimension to be rendered to"));
                    int overrideYInt = Mathf.RoundToInt(loadableSettings.overrideY);
                    overrideYInt = Mathf.RoundToInt(GUILayout.HorizontalSlider(overrideYInt, 500, 2000));
                    loadableSettings.overrideY = overrideYInt;

                }
            }

            if (EditorGUI.EndChangeCheck()){
                AddDefines();
                SaveToPrefs(); 
            } 
        }

        private void AddDefines()
        { 
            if (loadableSettings.use_autosaver) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_AUTOSAVER", NamedBuildTarget.Standalone);
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_AUTOSAVER", NamedBuildTarget.Standalone);  
            
            if (loadableSettings.use_autoVR) 
                EditorUtils.AddDefineIfNecessary("SXR_USE_AUTOVR", NamedBuildTarget.Standalone);
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_AUTOVR", NamedBuildTarget.Standalone);  
            
            if(loadableSettings.use_startscreen)
                EditorUtils.AddDefineIfNecessary("SXR_USE_STARTSCREEN", NamedBuildTarget.Standalone);
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_STARTSCREEN", NamedBuildTarget.Standalone);  
            
            if (loadableSettings.use_SRanipal)
            {
                EditorUtils.AddDefineIfNecessary("SXR_USE_SRANIPAL", NamedBuildTarget.Standalone);
                Debug.Log("SXR_USE_SRANIPAL");
                string viveSR_path = Application.dataPath+"/ViveSR/ViveSR.asmdef";
                string viveSRAssemblyDefinitionContent = @"{
""name"": ""ViveSR"",
""references"": [],
""optionalUnityReferences"": [],
""includePlatforms"": [],
""excludePlatforms"": [],
""allowUnsafeCode"": false,
""overrideReferences"": false,
""precompiledReferences"": [],
""autoReferenced"": true,
""defineConstraints"": [],
""versionDefines"": [],
""noEngineReferences"": false
}";
                try {
                    // Check if the file exists
                    if (!System.IO.File.Exists(viveSR_path))
                    {
                        // If the file does not exist, write the content to a new file at the path
                        System.IO.File.WriteAllText(viveSR_path, viveSRAssemblyDefinitionContent);
                    }
                } catch (System.Exception e) {
                    UnityEngine.Debug.LogError("sXR attempted to add ViveSR assembly file but failed, make sure you have the SRanipal SDK installed.  See the wiki for more info: https://github.com/unity-sXR/sXR/wiki/Vive-Pro-Eye-Setup\n" + e.Message);
                }
            }
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_SRANIPAL",NamedBuildTarget.Standalone); 
            
            if (loadableSettings.use_steamVR)
            {
                EditorUtils.AddDefineIfNecessary("SXR_USE_STEAMVR", NamedBuildTarget.Standalone);
                Debug.LogWarning("SteamControllerVR is deprecated, should only use with projects previously designed with SteamVR controls.");
            }
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_STEAMVR",NamedBuildTarget.Standalone); 

            if (loadableSettings.use_URP) 
                EditorUtils.AddDefineIfNecessary("SXR_USE_URP", NamedBuildTarget.Standalone);
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_URP",NamedBuildTarget.Standalone);
            
            if (loadableSettings.use_singlePass) 
                EditorUtils.AddDefineIfNecessary("SXR_USE_SINGLE_PASS", NamedBuildTarget.Standalone);
            else 
                EditorUtils.RemoveDefineIfNecessary("SXR_USE_SINGLE_PASS",NamedBuildTarget.Standalone);

        }

        void OnEnable() { initialLoad = true; }
        void OnDisable() {new ObjectPreview().Cleanup(); }

    }
}