using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourcesProvider : EditorWindow
{
    private const string PackageResourcesDirectory = "Packages/com.simpleomnia.simplexr/Runtime/Resources/";
    private const string ProjectResourcesDirectory = "Assets/sXR/Resources/";
    private const string MainPrefabName = "sxr_prefab";
    private bool initialResourcesLoaded = false; 

    /// <summary>
    /// Right click option to add sxr_prefab
    /// </summary>
    [MenuItem("GameObject/simpleXR/sXR prefab", false, 10)]
    static void CreateMainPrefab() 
    { CreatePrefab(ProjectResourcesDirectory + "Prefabs"+ Path.DirectorySeparatorChar + MainPrefabName + ".prefab"); }

    /// <summary>
    /// Right click option to open window with other prefabs
    /// </summary>
    [MenuItem("GameObject/simpleXR/sXR Other", false, 11)]
    static void ShowWindow() 
    { GetWindow<ResourcesProvider>().ShowPopup(); }

    /// <summary>
    /// Right click option to copy resources to Assets
    /// </summary>
    [MenuItem("GameObject/simpleXR/Copy Resources", false, 12)]
    static void CopyResourcesFromPackageToProject() {
        // Copy from package directory to project directory
        if (!Directory.Exists(ProjectResourcesDirectory))
            Directory.CreateDirectory(ProjectResourcesDirectory);

        foreach (var directory in Directory.GetDirectories(PackageResourcesDirectory, "*", SearchOption.AllDirectories)) {
            if (directory != "Shaders")
            {
                string targetDirectory = directory.Replace(PackageResourcesDirectory, ProjectResourcesDirectory);
            
                Directory.CreateDirectory(targetDirectory);

                foreach (var filePath in Directory.GetFiles(directory)) {
                    string targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(filePath));

                    if (File.Exists(targetFilePath)) { 
                        Debug.Log("Resource already available at "+targetFilePath);
                        continue; }

                    File.Copy(filePath, targetFilePath, false); } } }

        AssetDatabase.Refresh(); 
    }

    private static void CreatePrefab(string prefabPath) {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null) {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeObject = instance; }
        else {
            Debug.Log("Could not find prefab, attempting to copy resources to Assets folder");
            CopyResourcesFromPackageToProject(); } }
}