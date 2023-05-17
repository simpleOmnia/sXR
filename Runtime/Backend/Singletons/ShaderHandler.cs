using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

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

        private CommandBuffer commandBuffer; 
        
        [Header("Update either List, then click \"EditorUpdate\". Click \"ListShaders\" to output a list to the console")] 
        [SerializeField] private List<int> activePositions;
        [SerializeField] private List<string> activeNames; 
        [SerializeField] private bool editorUpdate;
        [SerializeField] private bool listShaders;

        [Header("List the currently active shaders (cannot edit:")]
        
        public List<int> currentActivePositions = new List<int>(); 
        public List<string> currentActiveNames = new List<string>();
        
        private int downscaleFactor;
        public void SetDownscaleFactor(int downscaleFactor)
        { this.downscaleFactor = downscaleFactor; }

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

        RenderTexture temp, processed;
        public void ActivateShaders(List<int> activePositions)
        {
            if (commandBuffer != null)
            {
                RenderTexture.ReleaseTemporary(processed);
                sxrSettings.Instance.vrCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
                commandBuffer.Release();
            }
            
            Debug.Log("Activate shaders by position: " + activePositions.ToArray().ToCommaSeparatedString()); 
            this.activePositions = activePositions;
            activeNames.Clear();
            currentActiveNames.Clear();
            currentActivePositions.Clear(); 

            // Create a new command buffer
            commandBuffer = new CommandBuffer();
            processed = RenderTexture.GetTemporary(Screen.width, Screen.height);
            commandBuffer.Blit(RenderTexture.active, processed); 
            
            // Apply each shader in turn
                foreach (int currShader in activePositions)
                {
                    temp = processed; 
                    currentActivePositions.Add(currShader);
                    activeNames.Add(shaderMaterials[currShader].name);
                    currentActiveNames.Add(shaderMaterials[currShader].name);
                    Material shaderMaterial = shaderMaterials[currShader]; 
                    // Set the shader material properties here if necessary

                    // Apply the shader
                    commandBuffer.Blit(temp, processed, shaderMaterial);
                }

                // Copy the final result back to the source render target
                commandBuffer.Blit(processed, BuiltinRenderTextureType.CameraTarget);
                
                // Add the command buffer to the camera
                //sxrSettings.Instance.vrCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
            }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            RenderTexture.ReleaseTemporary(processed);
            processed = RenderTexture.GetTemporary(src.width, src.height, 0 );
            Graphics.Blit(src, processed);
            foreach (var shaderNum in activePositions)
            {
                temp = processed;
                processed = RenderTexture.GetTemporary(src.width, src.height, 0 );
                Graphics.Blit(temp, processed, shaderMaterials[shaderNum]);
                RenderTexture.ReleaseTemporary(temp); 
            }

            Graphics.Blit(processed, dest); 
        }

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
