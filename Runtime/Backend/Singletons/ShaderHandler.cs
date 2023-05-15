using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//TODO Update Readme in Backend/shaders
//TODO AddShaderToObject() function


namespace sxr_internal {
    /// <summary>
    /// [Singleton] Handles shaders, actively listens for shader changes in editor mode (through Inspector)
    /// Should be attached to InspectorInteractables.  See ReadMe in sxr/Backend/Shaders
    ///     void ActivateShaders(List<int> activePositions)
    ///     void ActivateShaders(List<string> activeNames)
    ///     void ListShaders()
    /// On Awake:
    ///     Initializes singleton
    /// On Start:
    ///     Gathers all available shaders from "sxrSettings=>resourcesPath"/sXR/shaderMaterials"
    /// On Update:
    ///     N/A
    /// </summary>
    public class ShaderHandler : MonoBehaviour {
        
        private Material[] shaderMaterials;
        
        [Header("Update either List, then click \"EditorUpdate\". Click \"ListShaders\" to output a list to the console")] 
        [SerializeField] private List<int> activePositions;
        [SerializeField] private List<string> activeNames; 
        [SerializeField] private bool editorUpdate;
        [SerializeField] private bool listShaders;

        [Header("List the currently active shaders (cannot edit:")]
        public List<GameObject> shaderCameras = new List<GameObject>();

        public int numCameras; 
        public List<int> currentActivePositions = new List<int>(); 
        public List<string> currentActiveNames = new List<string>();

        public Material[] GetShaderMaterials()
        { return shaderMaterials; }
        
        public void Update() {
            if (editorUpdate) {
                if(!activePositions.SequenceEqual(currentActivePositions))
                    ActivateShaders(activePositions);
                else if (!activeNames.SequenceEqual(currentActiveNames))
                    ActivateShaders(activeNames); 
                editorUpdate = false; }

            if (listShaders) {
                ListShaders();
                listShaders = false; } }

        public void ActivateShaders(List<int> activePositions) {
            Debug.Log("Activate shaders by position: " + activePositions.ToArray().ToCommaSeparatedString()); 
            this.activePositions = activePositions;
            activeNames.Clear();
            currentActiveNames.Clear();
            currentActivePositions.Clear(); 

            sxrSettings.Instance.outputCamera.gameObject.SetActive(true);
            for (int i = 1; i < shaderCameras.Count; i++)
                Destroy(shaderCameras[i]);
            if (activePositions.Count < 1)
                return; 

            int currentPass=0;
            shaderCameras.Clear();
            shaderCameras.Add(sxrSettings.Instance.vrCamera.GameObject());

            foreach (int pos in activePositions) {
                if (shaderMaterials.Length < pos + 1) {
                    Debug.LogWarning("Attempted to activate shader #" + pos + ", but shader does not exist");
                    break; }
                
                currentActivePositions.Add(pos);
                activeNames.Add(shaderMaterials[pos].name);
                currentActiveNames.Add(shaderMaterials[pos].name);
                currentPass++;
                
                shaderCameras.Add(GameObject.Instantiate(sxrSettings.Instance.outputCamera.gameObject));
                shaderCameras[currentPass].GetComponentInChildren<Camera>().targetTexture
                    = RenderTexture.GetTemporary(Screen.currentResolution.width, Screen.currentResolution.height); 
                shaderCameras[currentPass].GetComponentInChildren<RawImage>().texture 
                    = shaderCameras[currentPass-1].GetComponentInChildren<Camera>().targetTexture;
                shaderCameras[currentPass].GetComponentInChildren<RawImage>().material = shaderMaterials[pos];
                shaderCameras[currentPass].name = "shadedOutput" + currentPass;

                if(currentPass>1) shaderCameras[currentPass-1].GameObject().SetActive(false); }
            
            sxrSettings.Instance.outputCamera.gameObject.SetActive(false);
            shaderCameras[currentPass].tag = "MainCamera";
            shaderCameras[currentPass].GetComponentInChildren<Camera>().targetTexture = null;
            numCameras = shaderCameras.Count; }

        public void ActivateShaders(List<string> shaderNames) {
            Debug.Log("Activate shaders by name: " + shaderNames.ToArray().ToCommaSeparatedString()); 
            activePositions.Clear();

            foreach (string name in shaderNames) {
                
                for(int i=0; i<shaderMaterials.Length; i++) {
                    if (shaderMaterials[i].name.ToLower().Equals(name.ToLower())) {
                        activePositions.Add(i);
                        break; } }
                Debug.LogWarning("Attempted to activate shader #" + name + ", but shader does not exist"); }

            ActivateShaders(activePositions); }

        public bool ModifyShader<T>(string shaderName, int settingID, T settingValue) {
            foreach (var shaderMaterial in shaderMaterials) {
                if (shaderMaterial.name == shaderName) {
                    Type type = typeof(T);
                    if(type == typeof(float))
                        shaderMaterial.SetFloat(settingID, (float)(object)settingValue);
                    else if (type == typeof(int))
                        shaderMaterial.SetInteger(settingID, (int) (object) settingValue);
                    else if (type == typeof(ComputeBuffer))
                        shaderMaterial.SetBuffer(settingID, (ComputeBuffer) (object) settingValue);
                    else
                        Debug.LogWarning("sXR: Type of "+type+" not supported in ModifyShader()");
                    
                    return true; } }
            Debug.LogWarning("sXR: Could not find shader - "+shaderName);
            return false; 
        }
        public bool ModifyShader<T>(string shaderName, string setting, T settingValue) 
        { return ModifyShader(shaderName, Shader.PropertyToID(setting), settingValue); }
        
        public void ListShaders() {
            string output = shaderMaterials.Length + " Shaders Detected: ";
            int count=0;
            foreach (var shader in shaderMaterials) {
                output += "\n" + count + ": "+shader.name;
                count++; }
            Debug.Log(output); }

        void Start() {
            Shader[] allShaders = Resources.LoadAll<Shader>("Shaders");
            List<Material> shaderMaterialsList = new List<Material>();

            for (int i = 0; i < allShaders.Length; i++)
                if (!allShaders[i].name.Contains("Hidden"))
                    shaderMaterialsList.Add(new Material(allShaders[i]));

            shaderMaterials = shaderMaterialsList.ToArray(); }


        // Singleton initiated on Awake()
        public static ShaderHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
