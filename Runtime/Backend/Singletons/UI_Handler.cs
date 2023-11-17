using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace sxr_internal {
    /// <summary>
    /// [Singleton] Keeps track of images on the UI
    ///  Attach to playerCamera -> Canvas
    /// Contains: 
    ///     void EnableOnly(Texture2D whichImage)
    ///     void SetPosition(sxr_internal.UI_Position whichPosition, Texture2D image, bool overridePosition=false)
    ///     void SetPosition(int whichPosition, Texture2D image, bool overridePosition
    ///     void FlipComponentUI(Texture2D whichImage)
    ///     void EnableComponentUI(Texture2D whichImage, bool enabled)
    ///     void EnableComponentUI(sxr_internal.UI_Position whichPosition, bool enabled)
    ///     void EnableComponentUI(sxr_internal.UI_Position whichPosition)
    ///     void DisableAllComponentsUI()
    ///     bool GetEnabled(Texture2D whichImage)
    /// On Awake:
    ///     Finds all RawImage components in children with it's name being a sxr_internal.UI_Position, and sets the corresponding Positions to be the component
    ///     Initializes singleton
    /// On Start:
    ///     N/A
    /// On Update:
    ///     N/A
    /// </summary>
    public class UI_Handler : MonoBehaviour
    {
        public RawImage[] UI_overlays = new RawImage[18];
        public RawImage pleaseWait, finished, eyeError, emergencyStop;
        public TextMeshProUGUI textboxTop, textboxTopMiddle, textboxBottomMiddle, textboxBottom, textboxTopLeft;

        [SerializeField]  GameObject interactiveUI, scrollObject, submitButton, inputWindow, inputSlider, inputDropdown;
        [SerializeField]  GameObject scrollTitle, scrollText, inputText, buttonText;
        
        public bool activeUpdate = false; 
        private bool submit;

        public void Update()
        {
            if (activeUpdate)
            {
                sxrSettings.Instance.vrCamera.cullingMask = submitButton.activeSelf
                    ? LayerMask.GetMask("InteractiveUI")
                    : ~(1 << LayerMask.NameToLayer("InteractiveUI"));
                
                inputWindow.SetActive(inputDropdown.activeSelf || inputSlider.activeSelf);
            }
            else
            {
                sxrSettings.Instance.vrCamera.cullingMask = ~(1 << LayerMask.NameToLayer("InteractiveUI"));
            }
        }

        public void UI_Submit()
        { HideInputUI(); submit = true; }

        public void HideInputUI()
        {
            scrollObject.SetActive(false);
            scrollTitle.GetComponent<Text>().text = "";
            scrollText.GetComponent<Text>().text = "";
            inputDropdown.SetActive(false);
            inputSlider.SetActive(false);
            submitButton.SetActive(false); 
        }

        public void InputSlider(int sliderMin, int sliderMax, string questionText, bool wholeNumbers) {
            var slider = inputSlider.GetComponent<Slider>();
            slider.minValue = sliderMin;
            slider.maxValue = sliderMax;
            slider.wholeNumbers = wholeNumbers; 
            SetText(inputText, questionText);
            inputSlider.SetActive(true);
            inputDropdown.SetActive(false); }
        public void InputSlider(int sliderMin, int sliderMax, string questionText)
        { InputSlider(sliderMin, sliderMax, questionText, true); }

        public void InputDropdown(string[] options, string questionText)
        {
            SetText(inputText, questionText);
            var dropdown = inputDropdown.GetComponent<TMP_Dropdown>();
            var optionsList = new List<TMP_Dropdown.OptionData>(); 
            foreach (var option in options)
                optionsList.Add(new TMP_Dropdown.OptionData(option));
            dropdown.options = optionsList; }

        /// <summary>
        /// Returns false until the UI "Submit" button is pressed. Outputs an int or string if
        /// an InputDropdown menu is used, or a float value if an InputSlider is used.
        /// </summary>
        /// <param name="output"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool ParseInputUI<T>(out T output) {
            if (inputSlider.activeSelf) {
                if (typeof(T) == typeof(float) ) 
                    output = (T) (object) inputSlider.GetComponent<Slider>().value; 
                else if (typeof(T) == typeof(int))
                    output = (T) (object) (int) inputSlider.GetComponent<Slider>().value; 
                else {
                    Debug.LogWarning("Cannot assign non float values with an InputSlider");
                    output = default(T); } }
            
            
            else if (inputDropdown.activeSelf) {
                if (typeof(T) != typeof(int) && typeof(T) != typeof(string)){
                    Debug.LogWarning("Can only return int or string values with InputDropdown");
                    output = default(T); }
                else if (typeof(T) == typeof(int))
                    output = (T) (object) inputDropdown.GetComponent<TMP_Dropdown>().value;
                else {
                    var dropdown = inputDropdown.GetComponent<TMP_Dropdown>();
                    output = (T) (object) dropdown.options[dropdown.value].text; } }

            else {
                Debug.Log("Cannot parse input without an active InputSlider or InputDropdown");
                output = default(T); }
            
            if (submit) { 
                HideInputUI(); 
                submit = false; 
                return true;  }
            
            return false; }

        /// <summary>
        /// Displays the specified image (searches by image name without the extension, e.g. "myImage" not "myImage.jpeg".
        /// </summary>
        /// <param name="imageName">Name of the image to display</param>
        /// <param name="position">Position on the UI to display image</param>
        /// <param name="overridePrevious">If there is a previous image, overwrite it with the new image</param> 
        public void DisplayImage(string imageName, sxr_internal.UI_Position position, bool overridePrevious) {
            var images = Resources.LoadAll<Texture2D>("GUI_Images");
            Texture2D image = null;
            foreach (var img in images)
                if (img.name == imageName)
                    image = img; 
            
            if (image == null) {
                Debug.Log(
                    "Unable to set image \"" + imageName + "\" not found in GUI_Images, trying main Resources folders");
                images = Resources.LoadAll<Texture2D>("");
                foreach (var img in images)
                    if (img.name == imageName)
                        image = img; 
                if (image == null)
                    Debug.LogWarning("Unable to find image in 'Resources/GUI_Images', check spelling and whitespace. Use"
                                     + "Resources.LoadAll<Texture2D>(\"\") to view all image Resources");
                else
                    UI_Handler.Instance.SetPosition(position, image, overridePrevious); }

            else
                UI_Handler.Instance.SetPosition(position, image, overridePrevious);
            UI_Handler.Instance.EnableComponentUI(position); }
        
        /// <summary>
        /// Disables all images in  UI_overlays except for given Texture2D
        /// </summary>
        /// <param name="whichImage">Image to keep enabled</param>
        public void EnableOnly(Texture2D whichImage) {
            DisableAllComponentsUI();
            EnableComponentUI(whichImage, true); }

        /// <summary>
        /// Sets Texture2D at given screen sxr_internal.UI_Position to the given Texture2D
        /// If override is true, it will replace any Texture2D already in the whichPosition
        /// If override is false and there is already a Texture2D at whichPosition, it will not replace the preexisting Texture2D
        /// </summary>
        /// <param name="whichPosition">Position to place image at</param>
        /// <param name="image">Texture2D to place at Position</param>
        /// <param name="overridePosition">Whether or not to override a preexisting Texture2D at whichPosition</param>
        public void SetPosition(sxr_internal.UI_Position whichPosition, Texture2D image, bool overridePosition=false) {
            if( UI_overlays[(int) whichPosition].texture == null || overridePosition)
                UI_overlays[(int) whichPosition].texture = image;
            else {
                Debug.Log("*** ERROR - UI Position " + whichPosition.ToString() +" already set. Use overridePosition:true or choose a different position"); } }
        
        /// <summary>
        /// Sets Texture2D at given screen sxr_internal.UI_Position to the given Texture2D
        /// If override is true, it will replace any Texture2D already in the whichPosition
        /// If override is false and there is already a Texture2D at whichPosition, it will not replace the preexisting Texture2D
        /// </summary>
        /// <param name="whichPosition">Position to place image at</param>
        /// <param name="image">Texture2D to place at Position</param>
        /// <param name="overridePosition">Whether or not to override a preexisting Texture2D at whichPosition</param>
        public void SetPosition(int whichPosition, Texture2D image, bool overridePosition){SetPosition(Enum.Parse<sxr_internal.UI_Position>(Enum.GetName(typeof(sxr_internal.UI_Position), whichPosition)), image, overridePosition);}
        /// <summary>
        /// Disables whichImage if it's enabled
        /// Enables whichImage if it's disabled
        /// </summary>
        /// <param name="whichImage">Image to switch enabled of</param>
        public void FlipComponentUI(Texture2D whichImage) {
            foreach (var image in UI_overlays) {
                if (image.texture != null)
                    if (image.texture.name == whichImage.name) {
                        image.enabled = !image.enabled; } } }
        /// <summary>
        /// Sets given image's enabled value to passed in enabled value
        /// </summary>
        /// <param name="whichImage">Image to set enable value of</param>
        /// <param name="enabled">Whether or not to enable whichImage</param>
        public void EnableComponentUI(Texture2D whichImage, bool enabled) {
            foreach (var image in UI_overlays) {
                if (image.texture != null)
                    if(image.texture.name == whichImage.name) {
                        image.enabled = enabled;
                        break; } } }

        /// <summary>
        /// Sets image at given position's enabled value to passed in enabled value
        /// </summary>
        /// <param name="whichPosition">Position of image to set enable value of</param>
        /// <param name="enabled">Whether or not to enable image at whichPosition</param>
        public void EnableComponentUI(sxr_internal.UI_Position whichPosition, bool enabled) {
            foreach (var component in UI_overlays)
                if (component.name == whichPosition.ToString())
                    component.enabled = enabled; } 
        
        /// <summary>
        /// Sets enabled value of image at whichPosition to true
        /// </summary>
        /// <param name="whichPosition">Position of image to enable</param>
        public void EnableComponentUI(sxr_internal.UI_Position whichPosition){ EnableComponentUI(whichPosition, true);}

        /// <summary>
        /// Disables all UI_overlays images
        /// </summary>
        public void DisableAllComponentsUI() {
            for (int i = 0; i < UI_overlays.Length; i++) {
                if (i != (int) sxr_internal.UI_Position.VRcamera) {
                    var image = UI_overlays[i];
                    if (image != null && image.texture != null)
                        image.enabled = false; } } }

        /// <summary>
        /// Disables the specified UI component
        /// </summary>
        /// <param name="position"></param>
        public void DisableComponentUI(sxr_internal.UI_Position position)
        { UI_overlays[(int) position].enabled = false;}
        
        /// <summary>
        /// Find out whether or not given image is enabled
        /// Returns false if image is not found on the UI
        /// </summary>
        /// <param name="whichImage">Image to find enabled value of</param>
        /// <returns>Returns true if the image is found and enabled, false otherwise</returns>
        public bool GetEnabled(Texture2D whichImage) {
            foreach (var image in UI_overlays)
                if (image.texture != null && image.texture.name == whichImage.name)
                    return image.enabled;

            Debug.Log("*** Searched image does not appear to be on any active UI Component: " +whichImage.name); 
            return false; }

        public RawImage GetRawImageAtPosition(sxr_internal.UI_Position pos) {
            foreach (var component in UI_overlays)
                if (component.name == pos.ToString())
                    return component;
            Debug.Log("No image found at " + pos);
            return null; }

        public void DisplayPrebuilt(sxr_internal.Prebuilt_Images image) {
            switch (image) {
                case sxr_internal.Prebuilt_Images.Stop:
                    UI_Handler.Instance.emergencyStop.enabled = true;
                    break;
                case sxr_internal.Prebuilt_Images.Finished:
                    UI_Handler.Instance.finished.enabled = true;
                    break;
                case sxr_internal.Prebuilt_Images.Loading:
                    UI_Handler.Instance.pleaseWait.enabled = true;
                    break;
                case sxr_internal.Prebuilt_Images.EyeError:
                    UI_Handler.Instance.eyeError.enabled = true;
                    break; } }

        private void Start()
        {
            sxr.SetIfNull(ref interactiveUI, "InteractiveUI"); 
            sxr.SetIfNull(ref submitButton , "SubmitButton");
            sxr.SetIfNull(ref inputSlider, "Slider");
            sxr.SetIfNull(ref inputDropdown , "Dropdown");
            sxr.SetIfNull(ref scrollTitle , "ScrollTitle");
            sxr.SetIfNull(ref scrollObject, "ScrollObject");
            sxr.SetIfNull(ref scrollText, "ScrollText");
            sxr.SetIfNull(ref buttonText, "ButtonText");
            sxr.SetIfNull(ref inputWindow, "InputWindow"); 

            inputSlider.SetActive(false);
            inputDropdown.SetActive(false);
            SetText(scrollTitle, "Instructions");
            SetText(scrollText, "Wait for the experimenter to start.");
            SetText(buttonText, "Start");

            if (!TagsAndLayers.LayerExists("InteractiveUI"))
                TagsAndLayers.CreateLayer("InteractiveUI");
            TagsAndLayers.SetLayerRecursively(interactiveUI, LayerMask.NameToLayer("InteractiveUI")); 

        }

        bool SetText(GameObject gameObj, string text)
        {
            if (!gameObj) return false;
            
            if (gameObj.GetComponent<Text>() != null)
                gameObj.GetComponent<Text>().text = text;
            else if (gameObj.GetComponent<TextMeshProUGUI>() != null)
                gameObj.GetComponent<TextMeshProUGUI>().text = text;
            else
                return false;
            
            return true; 
        }

        // Singleton initiated on Awake()
        public static UI_Handler Instance { get; private set; }
        private void Awake() {

            // Parse all UI_Handler components from Unity names
            var overlayComponents = gameObject.transform.Find("MainCanvas").GetComponentsInChildren<RawImage>();

            foreach (var component in overlayComponents) {
                if (component.name == "Finished")
                {
                    finished = component;
                    finished.texture = Resources.Load<Texture2D>("GUI_Images/finished");
                }
                else if (component.name == "PleaseWait")
                {
                    pleaseWait = component;
                    pleaseWait.texture = Resources.Load<Texture2D>("GUI_Images/PleaseWait");
                }
                else if (component.name == "EyeError")
                {
                    eyeError = component;
                    eyeError.texture = Resources.Load<Texture2D>("GUI_Images/EyeError");
                }
                else if (component.name == "EmergencyStop")
                {
                    emergencyStop = component;
                    emergencyStop.texture = Resources.Load<Texture2D>("GUI_Images/EmergencyStop");
                }
                else
                    for (int i = 0; i < Enum.GetNames(typeof(sxr_internal.UI_Position)).Length; i++) {
                        if(component.name == Enum.GetValues(typeof(sxr_internal.UI_Position)).GetValue(i).ToString()) 
                            UI_overlays[i] = component; } } 
            
            var experimenterTextComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            // Parse all UI_Handler experiment textboxes from Unity names 
            foreach (var textBox in experimenterTextComponents ) {
                textboxTop = textBox.name == "TextBox1" ? textBox : textboxTop;
                textboxTopMiddle = textBox.name == "TextBox2" ? textBox : textboxTopMiddle;
                textboxBottomMiddle = textBox.name == "TextBox3" ? textBox : textboxBottomMiddle;
                textboxBottom = textBox.name == "TextBox4" ? textBox : textboxBottom;
                textboxTopLeft = textBox.name == "Label" ? textBox : textboxTopLeft; }
            
            if ( Instance == null) { Instance = this; DontDestroyOnLoad(gameObject.transform.root); }
            else { Destroy(gameObject); } }
    }
}
