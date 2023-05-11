using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabContextMenu : EditorWindow
{
    private const string PackagePrefabDirectory = "Packages/com.simpleomnia.simplexr/Runtime/Resources/Prefabs/";
    private const string ProjectPrefabDirectory = "Assets/Resources/Prefabs/";
    private const string MainPrefabName = "sxr_prefab";

    [MenuItem("GameObject/simpleXR/sXR prefab", false, 10)]
    static void CreateMainPrefab()
    {
        CopyPrefabFromPackageToProject(MainPrefabName);
        string assetPath = ProjectPrefabDirectory + MainPrefabName + ".prefab";
        CreatePrefab(assetPath);
    }

    [MenuItem("GameObject/simpleXR/sXR Other", false, 11)]
    static void ShowWindow()
    {
        var window = GetWindow<PrefabContextMenu>();
        window.ShowPopup();
    }

    private void OnGUI()
    {
        // Copy prefabs from package directory to project directory
        Directory.CreateDirectory(ProjectPrefabDirectory);
        foreach (string filePath in Directory.GetFiles(PackagePrefabDirectory, "*.prefab"))
        {
            string fileName = Path.GetFileName(filePath);
            File.Copy(filePath, ProjectPrefabDirectory + fileName, true);
        }

        // Add menu items for prefabs
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { ProjectPrefabDirectory });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (GUILayout.Button(Path.GetFileNameWithoutExtension(assetPath)))
            {
                CreatePrefab(assetPath);
                this.Close();
            }
        }
    }

    private static void CopyPrefabFromPackageToProject(string prefabName)
    {
        Directory.CreateDirectory(ProjectPrefabDirectory);
        string sourceFilePath = PackagePrefabDirectory + prefabName + ".prefab";
        string destinationFilePath = ProjectPrefabDirectory + prefabName + ".prefab";
        File.Copy(sourceFilePath, destinationFilePath, true);
    }

    private static void CreatePrefab(string prefabPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
            Selection.activeObject = instance;
        }
    }
}