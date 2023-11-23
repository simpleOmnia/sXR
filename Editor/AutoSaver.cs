using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace sxr_internal
{
    /// <summary>
    /// Automatically saves the scene and asset library whenever play is pressed.  
    /// </summary>

    public class Autosaver
    {
#if SXR_USE_AUTOSAVER
    [InitializeOnEnterPlayMode]
    static void AutosaverInit() {
        Debug.LogWarning("Autosaver saved scene/assets, will trigger Null Reference if GameObject selected at start");
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets(); }
#endif
    }
}