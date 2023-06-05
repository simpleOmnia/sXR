using System.Collections.Generic;
using UnityEngine;

namespace sxr_internal{
    public class PlayerPrefsWrapper
    {
        public List<string> NamedPrefs { get; private set; }
        public List<string> NamedPrefsType { get; private set; }

        public PlayerPrefsWrapper() { LoadPrefs(); }

        public bool LoadPrefs() {
            string jsonList = PlayerPrefs.GetString("StoredKeys", "");
            string jsonTypesList = PlayerPrefs.GetString("StoredTypes", "");
            if (jsonList != "" && jsonTypesList != "")
            {
                NamedPrefs = JsonUtility.FromJson<Serialization<string>>(jsonList).ToList();
                NamedPrefsType = JsonUtility.FromJson<Serialization<string>>(jsonTypesList).ToList();
                if (NamedPrefs.Count == NamedPrefsType.Count)
                    return true; 
                Debug.LogWarning("sXR - StoredKeys and StoredTypes mismatch, not loading values");
            }

            Debug.Log(
                "No stored preferences found, if you know the names of your Prefs, they will automatically be added with PlayerPrefsWrapper.GetInt/GetFloat/GetString/GetBool"); 
            NamedPrefs = new List<string>();
            NamedPrefsType = new List<string>();
            return false; 
        }

        public void SavePrefs()
        {
            PlayerPrefs.SetString("StoredKeys", JsonUtility.ToJson(new Serialization<string>(NamedPrefs))); 
            PlayerPrefs.SetString("StoredTypes", JsonUtility.ToJson(new Serialization<string>(NamedPrefsType))); 
        }

        public bool HasPref(string prefName)
        { return NamedPrefs.Contains(prefName); }
        
        public object GetPref(string prefName) {
            if (NamedPrefs.Contains(prefName)) {
                int index = NamedPrefs.IndexOf(prefName);
                string type = NamedPrefsType[index];
        
                switch (type)
                {
                    case "I": // Integer
                        return GetInt(prefName);
                    case "F": // Float
                        return GetFloat(prefName);
                    case "S": // String
                        return GetString(prefName);
                    case "B": // Boolean
                        return GetBool(prefName);
                    default:
                        Debug.LogError("Unsupported preference type: " + type);
                        break; }
            }

            Debug.LogError("Preference not found: " + prefName);
            return null;
        }

        public string GetType(string prefName) {
            if (NamedPrefs.Contains(prefName))
                return NamedPrefsType[NamedPrefs.IndexOf(prefName)];
            
            Debug.Log("sXR - Could not find pref with name "+prefName+"to return GetType");
            return "0"; 
        }

        public bool RemovePref(string prefName) {
            PlayerPrefs.DeleteKey(prefName); 
            if(NamedPrefs.Contains(prefName))
            {
                NamedPrefsType.RemoveAt(NamedPrefs.IndexOf(prefName));
                NamedPrefs.Remove(prefName);
                SavePrefs(); 
                return true; 
            }
            Debug.Log("Failed to find variable: "+prefName);
            return false; 
        }

        public void RemoveAll()
        {
            PlayerPrefs.DeleteAll();
            NamedPrefs.Clear();
            NamedPrefsType.Clear();
        }

        public int GetInt(string prefName) {
            if (PlayerPrefs.HasKey(prefName)) {
                if (!NamedPrefs.Contains(prefName)) {
                    NamedPrefs.Add(prefName);
                    NamedPrefsType.Add("I"); 
                    SavePrefs(); }
                return PlayerPrefs.GetInt(prefName); }

            Debug.Log("sXR - Could not find " + prefName + " as a PlayerPrefs int value");
            return 0; 
        }

        public float GetFloat(string prefName) {
            if (PlayerPrefs.HasKey(prefName)) {
                if (!NamedPrefs.Contains(prefName)) {
                    NamedPrefs.Add(prefName);
                    NamedPrefsType.Add("F"); 
                    SavePrefs(); }
                return PlayerPrefs.GetFloat(prefName); }

            Debug.Log("sXR - Could not find " + prefName + " as a PlayerPrefs float value");
            return 0f; 
        }

        public string GetString(string prefName) {
            if (PlayerPrefs.HasKey(prefName)) {
                if (!NamedPrefs.Contains(prefName)) {
                    NamedPrefs.Add(prefName);
                    NamedPrefsType.Add("S"); 
                    SavePrefs(); }
                return PlayerPrefs.GetString(prefName); }

            Debug.Log("sXR - Could not find " + prefName + " as a PlayerPrefs string value");
            return ""; 
        }

        public bool GetBool(string prefName) {
            if (PlayerPrefs.HasKey(prefName)) {
                if (!NamedPrefs.Contains(prefName)) {
                    NamedPrefs.Add(prefName);
                    NamedPrefsType.Add("B"); 
                    SavePrefs(); }
                return PlayerPrefs.GetInt(prefName) == 1; }

            Debug.Log("sXR - Could not find " + prefName + " as a PlayerPrefs bool value");
            return false; 
        }
        
        public bool SetPlayerPref(string prefName, object value)
        {
            bool addNew = !NamedPrefs.Contains(prefName);

            if (value is int intValue) {
                if (addNew) NamedPrefsType.Add("I");
                PlayerPrefs.SetInt(prefName, intValue); }
            else if (value is float floatValue) {
                if (addNew) NamedPrefsType.Add("F");
                PlayerPrefs.SetFloat(prefName, floatValue); }
            else if (value is string stringValue) {
                if (addNew) NamedPrefsType.Add("S");
                PlayerPrefs.SetString(prefName, stringValue); }
            else if (value is bool boolValue) {
                if (addNew) NamedPrefsType.Add("B");
                PlayerPrefs.SetInt(prefName, boolValue ? 1 : 0); }
            else{
                Debug.LogError("Unsupported data type");
                return false;
            }

            if (addNew) {
                NamedPrefs.Add(prefName);
                SavePrefs(); }

            return true; 
        }
    }
}