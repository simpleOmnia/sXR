using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace sxr_internal{
    public class sXR_Settings : EditorWindow 
    {
        
        private string savedSettingsPath = Application.dataPath + Path.DirectorySeparatorChar +
                                          "sxr" + Path.DirectorySeparatorChar + "Editor" +
                                          Path.DirectorySeparatorChar + "sxrSettings.json";
        private RectOffset rctOffButton, rctOffTextField, rctOffToggle, rctOffSlider;

        private GUIStyle myStyle;
        
        private LoadableSettings loadableSettings = new LoadableSettings();

        private bool initialLoad = true;

        [MenuItem("sXR/Editor Settings")]
        static void Init() {
            sXR_Settings window = (sXR_Settings)EditorWindow.GetWindow(typeof(sXR_Settings));
            window.Show(); }

        
    void LoadFromPrefs(){
        loadableSettings.dataPath = EditorPrefs.GetString("sXR_DataPath", "");
        loadableSettings.backupPath = EditorPrefs.GetString("sXR_BackupPath", "");
        loadableSettings.use_autosaver = EditorPrefs.GetBool("sXR_UseAutosaver", false);
        loadableSettings.use_autoVR = EditorPrefs.GetBool("sXR_UseAutoVR", false);
        loadableSettings.use_safetyWalls = EditorPrefs.GetBool("sXR_UseSafetyWalls", false);
        loadableSettings.safetyWallBoundsNS = EditorPrefs.GetFloat("sXR_SafetyWallBoundsNS", 5f);
        loadableSettings.safetyWallBoundsEW = EditorPrefs.GetFloat("sXR_SafetyWallBoundsEW", 5f);
        loadableSettings.debugSetting = EditorPrefs.GetInt("sXR_DebugSetting", 2);
        loadableSettings.recordFrequency = EditorPrefs.GetFloat("sXR_RecordFrequency", 1f);
        loadableSettings.use_SRanipal = EditorPrefs.GetBool("sXR_UseSRanipal", false);
        loadableSettings.interpolateGaze = EditorPrefs.GetBool("sXR_InterpolateGaze", false);
        loadableSettings.interpolateAmount = EditorPrefs.GetFloat("sXR_InterpolateAmount", 5f);
        loadableSettings.use_steamVR = EditorPrefs.GetBool("sXR_UseSteamVR", false);
        loadableSettings.use_URP = EditorPrefs.GetBool("sXR_UseURP", false);
    }

    void SaveToPrefs() {
        EditorPrefs.SetString("sXR_DataPath", loadableSettings.dataPath);
        EditorPrefs.SetString("sXR_BackupPath", loadableSettings.backupPath);
        EditorPrefs.SetBool("sXR_UseAutosaver", loadableSettings.use_autosaver);
        EditorPrefs.SetBool("sXR_UseAutoVR", loadableSettings.use_autoVR);
        EditorPrefs.SetBool("sXR_UseSafetyWalls", loadableSettings.use_safetyWalls);
        EditorPrefs.SetFloat("sXR_SafetyWallBoundsNS", loadableSettings.safetyWallBoundsNS);
        EditorPrefs.SetFloat("sXR_SafetyWallBoundsEW", loadableSettings.safetyWallBoundsEW);
        EditorPrefs.SetInt("sXR_DebugSetting", loadableSettings.debugSetting);
        EditorPrefs.SetFloat("sXR_RecordFrequency", loadableSettings.recordFrequency);
        EditorPrefs.SetBool("sXR_UseSRanipal", loadableSettings.use_SRanipal);
        EditorPrefs.SetBool("sXR_InterpolateGaze", loadableSettings.interpolateGaze);
        EditorPrefs.SetFloat("sXR_InterpolateAmount", loadableSettings.interpolateAmount);
        EditorPrefs.SetBool("sXR_UseSteamVR", loadableSettings.use_steamVR);
        EditorPrefs.SetBool("sXR_UseURP", loadableSettings.use_URP);
    }
        
        void OnGUI()
        {
            if (initialLoad)
            {
                initialLoad = false; 
                LoadFromPrefs();
                Debug.Log("Loaded settings from EditorPrefs"); 
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
                new GUIContent("   Use SteamVR",
                    "Enable to use SteamVR controller bindings, can make some devices unreachable"));
            
            GUILayout.Space(20);
            loadableSettings.use_URP = GUILayout.Toggle(loadableSettings.use_URP,
                new GUIContent("   Use Universal Rendering Pipeline (URP)",
                    "Enable to use URP"));

            
            if (EditorGUI.EndChangeCheck()){
                if (loadableSettings.use_autosaver) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_AUTOSAVER", NamedBuildTarget.Standalone);
                else 
                    EditorUtils.RemoveDefineIfNecessary("SXR_USE_AUTOSAVER", NamedBuildTarget.Standalone);  
                
                if (loadableSettings.use_autoVR) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_AUTOVR", NamedBuildTarget.Standalone);
                else 
                    EditorUtils.RemoveDefineIfNecessary("SXR_USE_AUTOVR", NamedBuildTarget.Standalone);  
                
                if (loadableSettings.use_SRanipal) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_SRANIPAL", NamedBuildTarget.Standalone);
                else 
                    EditorUtils.RemoveDefineIfNecessary("SXR_USE_SRANIPAL",NamedBuildTarget.Standalone); 
                
                if (loadableSettings.use_SRanipal) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_STEAMVR", NamedBuildTarget.Standalone);
                else 
                    EditorUtils.RemoveDefineIfNecessary("SXR_USE_STEAMVR",NamedBuildTarget.Standalone); 

                if (loadableSettings.use_URP) 
                    EditorUtils.AddDefineIfNecessary("SXR_USE_URP", NamedBuildTarget.Standalone);
                else 
                    EditorUtils.RemoveDefineIfNecessary("SXR_USE_URP",NamedBuildTarget.Standalone);

                SaveToPrefs(); 
            } 
        }

        void OnEnable() { initialLoad = true; }
        void OnDisable() {new ObjectPreview().Cleanup(); }

    }
}