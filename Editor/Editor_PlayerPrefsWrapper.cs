using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using sxr_internal; 

public class Editor_PlayerPrefsWrapper : EditorWindow
{
    private PlayerPrefsWrapper wrapper;
    private GUIStyle myStyle;
    private RectOffset rctOffButton, rctOffTextField, rctOffToggle, rctOffSlider;
    private bool newAdd, finalizeAdd, confirmAdd;
    private int dtype;
    private string addName, addValue="";
    
    private bool newRemove, finalizeRemove;
    private string removeName; 
    
    private bool deleteAllWarning; 

    [MenuItem("sXR/Stored Variables")]
    static void Init()
    {
        Editor_PlayerPrefsWrapper window = (Editor_PlayerPrefsWrapper)EditorWindow.GetWindow(typeof(Editor_PlayerPrefsWrapper));
        window.Show();
    }

    void OnGUI()
    {
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

        myStyle = new GUIStyle(GUI.skin.label) { fontSize = 15 };

        int columns = wrapper.NamedPrefs.Count > 30 ? wrapper.NamedPrefs.Count / 10 : 3; // Number of columns
        int count = 0; // Counter for preferences

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginVertical();

        foreach (var prefName in wrapper.NamedPrefs)
        {
            if (count % columns == 0)
            {
                if (count > 0) // Add an additional check to close the previous horizontal layout group
                    EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
            }

            GUILayout.Label("[" + prefName + ": " + wrapper.GetPref(prefName) + " (" + wrapper.GetType(prefName) +") ]");

            count++;
            count += wrapper.GetPref(prefName).ToString().Length / 6;

            if (count >= columns)
            {
                count = 0;
                EditorGUILayout.EndHorizontal();
            }
        }

        if (count % columns != 0)
            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        
        EditorGUI.BeginChangeCheck();
        
        GUILayout.Space(20); 
        if(GUILayout.Button(new GUIContent("Reload Variables", "Click to reload all variables"))) { wrapper.LoadPrefs(); }
        
        GUILayout.Space(20);
        if (newAdd || GUILayout.Button(new GUIContent("Add/Change Variable", "Click to add or change a stored variable")))
        {
            newAdd = true;
            addName = EditorGUILayout.TextField("Preference Name: ", addName);
            dtype = EditorGUILayout.Popup("Data Type: ", dtype, new string[] { "Integer", "Float", "String", "Boolean" });
            addValue = EditorGUILayout.TextField("Preference Value: ", addValue);
            switch (dtype)
            {
                case 0: // Integer
                    addValue = Regex.Replace(addValue, @"\D", m => m.Value.Substring(0, m.Value.Length - 1));
                    break;
                case 1: // Float
                    addValue = Regex.Replace(addValue, @"\.\d*\.", m => m.Value.Substring(0, m.Value.Length - 1));
                    break;
                case 2: // String (no modification needed)
                    break;
                case 3: // Boolean
                    addValue = Regex.Replace(addValue, "[^TtFf]", ""); // Only accepts "T", "t", "F", or "f"
                    break;
            }
            if (finalizeAdd || GUILayout.Button((wrapper.HasPref(addName) ? "Change: " : "ADD: ") + addName))
            {
                finalizeAdd = true; 
                confirmAdd = GUILayout.Button("CONFIRM VALUE: " + addValue);
            }
        }
        GUILayout.Space(20);
        if (newRemove|| GUILayout.Button(new GUIContent("Remove Variable? ", "Click to remove a stored variable")))
        {
            newRemove = true; 
            removeName =  EditorGUILayout.TextField("Preference Name: ", removeName);
            if(GUILayout.Button("Remove " + removeName+"?")){
                wrapper.RemovePref(removeName);
                removeName = "";
                newRemove = false; } }

        GUILayout.Space(20); 
        if(deleteAllWarning || GUILayout.Button(new GUIContent("Delete All? ", "Click to delete all saved variables")))
        {
            deleteAllWarning = true; 
            GUILayout.Label("WARNING - THIS WILL DELETE ALL PlayerPrefs.\n          (This includes sXR settings)");
            if (GUILayout.Button("DELETE ALL"))
            {
                wrapper.RemoveAll();
                deleteAllWarning = false; 
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            if (confirmAdd)
            {
                switch (dtype)
                {
                    case 0: // Integer
                        int intValue;
                        if (Int32.TryParse(addValue, out intValue))
                            wrapper.SetPlayerPref(addName, intValue);
                        break;
                    case 1: // Float
                        float floatValue;
                        if (float.TryParse(addValue, out floatValue))
                            wrapper.SetPlayerPref(addName, floatValue);
                        break;
                    case 2: // String
                        wrapper.SetPlayerPref(addName, addValue);
                        break;
                    case 3: // Boolean
                        wrapper.SetPlayerPref(addName, addValue=="T" || addValue=="t");
                        break;
                    default:
                        Debug.LogError("Unsupported data type");
                        break;
                }
                Debug.Log("Adding " + addName + ": " + addValue); 
                newAdd = false;
                finalizeAdd = false;
                confirmAdd = false; 
                addName = "";
                addValue = "";
            }
        }
    }

    void OnEnable() { wrapper = new PlayerPrefsWrapper(); }
    void OnDisable() { new ObjectPreview().Cleanup(); }
}
