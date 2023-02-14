using System;
using System.Collections.Generic;
using UnityEngine;
using sxr_internal;
using Unity.VisualScripting;
using UnityEngine.UIElements;

/// <summary>
/// Contains static calls to allow for simple, easy to understand access to sxr's functions...
/// Designed to allow for typing "sxr." + whatever it is you're trying to do into your IDE.
/// For example, typing "sxr.move" should show options to move objects. Optional parameters
/// are intentionally specified as separate method invocations meaning you can leave them out
/// to use the default value, or specify the variable to use another value. (Do not use the ":"
/// operator for optional parameters.)
/// </summary>
public static class sxr {
   
// ****   USER INTERFACE   ****

    /// <summary>
    /// Positions available to display UI Images. Higher numbers will display over lower numbers.
    /// </summary>
    public enum UI_Position {
        FullScreen1, FullScreen2, FullScreen3, FullScreen4, FullScreen5, 
        PartialScreenMiddle1, PartialScreenMiddle2, PartialScreenMiddle3, PartialScreenMiddle4,
        PartialScreenBottomLeft, PartialScreenBottom, PartialScreenBottomRight,
        PartialScreenTopLeft, PartialScreenTop, PartialScreenTopRight,
        PartialScreenLeft, PartialScreenRight, VRcamera }
    
    /// <summary>
    /// Images included by default in sXR
    /// </summary>
    public enum Prebuilt_Images{Stop, Loading, Finished, EyeError}
    
    /// <summary>
    /// Displays one of the "prebuilt" images that comes with sXR.
    /// Use 'sxr.Prebuilt_Images' (.Stop, .Loading, .Finished, .EyeError) to specify image
    /// </summary>
    /// <param name="image"></param>
    public static void DisplayPrebuilt(Prebuilt_Images image)
    {UI_Handler.Instance.DisplayPrebuilt(image); }
    
    /// <summary>
    /// Displays an input slider on the user interface that can be
    /// moved with controller lasers
    /// </summary>
    /// <param name="min">Minimum value of slider</param>
    /// <param name="max">Maximum value of slider</param>
    /// <param name="questionText">Question to display above slider</param>
    public static void InputSlider(int min, int max, string questionText, bool wholeNumbers) 
    { UI_Handler.Instance.InputSlider(min, max, questionText); }
    public static void InputSlider(int min, int max, string questionText) { InputSlider(min, max, questionText, true); }
    
    /// <summary>
    /// Displays a dropdown UI element with the provided options
    /// </summary>
    /// <param name="options">List of options for the dropdown menu</param>
    /// <param name="questionText">Text to display above the dropdown menu</param>
    public static void InputDropdown(string[] options, string questionText)
    {UI_Handler.Instance.InputDropdown(options, questionText);}

    /// <summary>
    /// Parses values from the active InputSlider or InputDropdown.
    /// Can only parse float or int values for the slider and
    /// int or string values for the dropdown. Will return "false"
    /// until the submit button on the UI interface is pressed. When button is
    /// pressed, returns "true" and sets the referenced output (reference using 'out' keyword)
    /// to the value provided by the InputSlider or InputDropdown
    /// </summary>
    /// <param name="output"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool ParseInputUI<T>(out T output)
    { return UI_Handler.Instance.ParseInputUI(out output); }

    
    /// <summary>
    /// Pre-built positions to display text 
    /// </summary>
    public enum TextPosition{Top, MiddleTop, MiddleBottom, Bottom, TopLeft}
    
    
    /// <summary>
    /// Displays the provided text to the specified position on the VR user interface.
    /// Use 'sxr.TextPosition' to specify position (optional, default is 'MiddleTop')
    /// </summary>
    /// <param name="text"></param>
    /// <param name="position"></param>
    public static void DisplayText(string text, TextPosition position) {
        switch (position) {
            case TextPosition.Top: UI_Handler.Instance.textboxTop.text = text;
                UI_Handler.Instance.textboxTop.enabled = true; 
                break;
            case TextPosition.MiddleTop: UI_Handler.Instance.textboxTopMiddle.text = text; 
                UI_Handler.Instance.textboxTopMiddle.enabled = true;
                break; 
            case TextPosition.MiddleBottom: UI_Handler.Instance.textboxBottomMiddle.text = text; 
                UI_Handler.Instance.textboxBottomMiddle.enabled = true;
                break; 
            case TextPosition.Bottom: UI_Handler.Instance.textboxBottom.text = text; 
                UI_Handler.Instance.textboxBottom.enabled = true;
                break;     
            case TextPosition.TopLeft: UI_Handler.Instance.textboxTopLeft.text = text; 
                UI_Handler.Instance.textboxTopMiddle.enabled = true;
                break; 
            default: sxr.DebugLog("Text position not found");
                break; } }
    public static void DisplayText(string text){DisplayText(text, TextPosition.MiddleTop);}

    /// <summary>
    /// Hides the specified textbox displayed to the VR user interface. Use 'sxr.TextPosition' to specify position
    /// </summary>
    public static void HideText(TextPosition position) {
        switch (position) {
            case TextPosition.Top: UI_Handler.Instance.textboxTop.enabled = false; 
                break;
            case TextPosition.MiddleTop: UI_Handler.Instance.textboxTopMiddle.enabled = false; 
                break; 
            case TextPosition.MiddleBottom: UI_Handler.Instance.textboxBottomMiddle.enabled = false; 
                break; 
            case TextPosition.Bottom: UI_Handler.Instance.textboxBottom.enabled = false; 
                break;     
            case TextPosition.TopLeft: UI_Handler.Instance.textboxTopMiddle.enabled = false; 
                break; 
            default: sxr.DebugLog("Text position not found");
                break; } }
    
    /// <summary>
    /// Hides all textboxes displayed to the VR user interface
    /// </summary>
    public static void HideAllText() {
        UI_Handler.Instance.textboxTop.enabled = false;
        UI_Handler.Instance.textboxTopMiddle.enabled = false;
        UI_Handler.Instance.textboxBottomMiddle.enabled = false;
        UI_Handler.Instance.textboxBottom.enabled = false;
        UI_Handler.Instance.textboxTopLeft.enabled = false; }
    
    /// <summary>
    /// Displays the specified image (searches by image name without the extension, e.g. "myImage" not "myImage.jpeg".
    /// </summary>
    /// <param name="imageName">Name of the image to display</param>
    /// <param name="position">Position on the UI to display image</param>
    /// <param name="overridePrevious">If there is a previous image, overwrite it with the new image</param>
    public static void DisplayImage(string imageName, UI_Position position, bool overridePrevious)
    { UI_Handler.Instance.DisplayImage(imageName, position, overridePrevious);}
    public static void DisplayImage(string imageName, UI_Position position){DisplayImage(imageName, position, true);}
    public static void DisplayImage(string imageName){DisplayImage(imageName, UI_Position.FullScreen1);}
    public static void DisplayImage(Texture2D image, UI_Position position)
    {UI_Handler.Instance.DisplayImage(image.name, position, true);}
    
    public static void HideImageUI(UI_Position position){UI_Handler.Instance.DisableComponentUI(position);}
    public static void HideImagesUI(){UI_Handler.Instance.DisableAllComponentsUI();}
    
// ****   INPUT DEVICES   ****
    /// <summary>
    /// Offers a combined "Trigger" across joystick trigger, vr controller trigger, left mouse click, and keyboard spacebar
    /// </summary>
    /// <param name="frequency">Pause between returning true when trigger/spacebar is held. Optional, default
    /// value is 1 second </param>
    /// <returns>true if [frequency] seconds has passed since last trigger and trigger/space is pressed</returns>
    public static bool GetTrigger(float frequency) { return ExperimentHandler.Instance.GetTrigger(frequency);}
    public static bool GetTrigger() { return GetTrigger(1.0f);}

    /// <summary>
    /// Will return true at specified [frequency] if the specified key
    /// is held
    /// </summary>
    /// <param name="whichKey">Choose with KeyCode.[Key]. e.g. KeyCode.A, KeyCode.Space</param>
    /// <param name="frequency">Pause between returning true values when a key is held (in seconds).
    /// Optional, default is 0 seconds (continuous input)</param>
    /// <returns></returns>
    public static bool KeyHeld(KeyCode whichKey, float frequency) { return KeyboardHandler.Instance.KeyHeld(whichKey, frequency); }
    public static bool KeyHeld(KeyCode whichKey) { return KeyHeld(whichKey, .5f);}

    /// <summary>
    /// Returns true on the initial frame that KeyCode [whichKey] is pressed
    /// </summary>
    public static bool InitialKeyPress(KeyCode whichKey) { return Input.GetKeyDown(whichKey);}
    /// <summary>
    /// Returns true on the initial frame that KeyCode [whichKey] is released
    /// </summary>
    public static bool KeyReleased(KeyCode whichKey) { return Input.GetKeyUp(whichKey);}

    public enum JoyStickDirection { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None }

    /// <summary>
    /// Returns the direction the joystick is being pushed in. Can be any of the 9 possible
    /// directions  
    /// </summary>
    /// <returns>JoystickDirection (Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None</returns>
    public static JoyStickDirection GetJoystickDirection() { return JoystickHandler.Instance.GetDirection();}

    public enum ControllerButton{
        LH_Trigger, LH_SideButton, LH_TrackPadRight, 
        LH_TrackPadLeft, LH_TrackPadUp, LH_TrackPadDown,
        LH_ButtonA, LH_ButtonB,
    
        RH_Trigger, RH_SideButton, RH_TrackPadRight,
        RH_TrackPadLeft, RH_TrackPadUp, RH_TrackPadDown,
        RH_ButtonA, RH_ButtonB,
    
        Trigger, SideButton, TrackPadRight,
        TrackPadLeft, TrackPadUp, TrackPadDown,
        ButtonA, ButtonB
    }
    
    /// <summary>
    /// Marks the time  of a "true" value for each check of the  button.
    /// Returns true if ControllerButton [whichButton] is pressed and the
    /// specified amount of time (frequency) has passed. 
    /// </summary>
    /// <param name="whichButton">Use sxr.ControllerButton to choose which button to listen for.
    /// e.g. sxr.ControllerVR.LH_TriggerPressed for left handed trigger. Controller options
    /// without RH/LH return true if either hand is pressed</param>
    /// <param name="frequency">Delay before returning true when button is held. Optional, default is 0 seconds (continuous input)</param>
    /// <returns>true if button VR controller button is pressed and delay has passed</returns>
    public static bool CheckController(ControllerButton whichButton, float frequency) {
        #if SXR_USE_STEAMVR
        ControllerVR controller = SteamControllerVR.Instance; 
        #else
        ControllerVR controller = OpenXR_Controller.Instance; 
        #endif 
        controller.EnableControllers();
        
        if (controller.buttonPressed[(int) whichButton]) {
            if (Time.time - controller.buttonTimers[(int) whichButton] > frequency) {
                DebugLog("Controller Button: " + whichButton + " pressed after delay of: " + frequency, 5000);
                controller.buttonTimers[(int) whichButton] = Time.time; 
                return true; }

            DebugLog("Controller Button: " + whichButton + "pressed before delay of: " + frequency, 5000); }
        else 
            DebugLog("Controller Button: " + whichButton + " not pressed", 5000);
        return false; }
    public static bool CheckController(ControllerButton whichButton) {
        return CheckController(whichButton, 0f); }

// ****  EXPERIMENT FLOW COMMANDS   ****
    /// <summary>
    /// Call at beginning of experiment to set data path
    /// </summary>
    /// <param name="experimentName"></param>
    /// <param name="subjectNumber"></param>
    public static void StartExperiment(string experimentName, int subjectNumber) {ExperimentHandler.Instance.StartExperiment(experimentName, subjectNumber); }

    /// <summary>
    /// Returns the current "phase" in the experiment. (Phase > Block > Trial > Step)
    /// </summary>
    /// <returns></returns>
    public static int GetPhase() { return ExperimentHandler.Instance.phase; }
    
    /// <summary>
    /// Returns the current "block" in the experiment. (Phase > Block > Trial > Step)
    /// </summary>
    /// <returns></returns>
    public static int GetBlock(){return ExperimentHandler.Instance.block; }
    
    /// <summary>
    /// Returns the current "trial" in the experiment. (Phase > Block > Trial > Step)
    /// </summary>
    /// <returns></returns>
    public static int GetTrial(){return ExperimentHandler.Instance.trial; }
    
    /// <summary>
    /// Returns the current "step" in the experiment. (Phase > Block > Trial > Step)
    /// </summary>
    /// <returns></returns>
    public static int GetStepInTrial() { return ExperimentHandler.Instance.stepInTrial;}

    /// <summary>
    /// Increments phase number by 1 and sets block/trial/step numbers to 0
    /// </summary>
    public static void NextPhase() {
        ExperimentHandler.Instance.phase++;
        ExperimentHandler.Instance.block = 0;
        ExperimentHandler.Instance.trial = 0;
        ExperimentHandler.Instance.stepInTrial = 0; }

    /// <summary>
    /// Increments block number by 1 and sets trial/step number to 0.
    /// </summary>
    public static void NextBlock() {
        ExperimentHandler.Instance.block++;
        ExperimentHandler.Instance.trial = 0;
        ExperimentHandler.Instance.stepInTrial = 0; }

    /// <summary>
    /// Incremented trial number by 1 and sets step to 0. 
    /// </summary>
    public static void NextTrial() {
        ExperimentHandler.Instance.trial++;
        ExperimentHandler.Instance.stepInTrial = 0; }
    
    public static void NextStep() { ExperimentHandler.Instance.stepInTrial++;}

    /// <summary>
    /// Sets the current "step" to the specified number
    /// </summary>
    /// <param name="stepNumber"></param>
    public static void SetStep(int stepNumber) { ExperimentHandler.Instance.stepInTrial = stepNumber;}

    /// <summary>
    /// Starts a timer with the provided name. Will return "true" and the timer will be
    /// deleted if CheckTimer() is used. If no name is provided, uses the default trial
    /// timer. (Default timer is never deleted)
    /// </summary>
    /// <param name="timerName"></param>
    /// <param name="duration"></param>
    public static void StartTimer(string timerName, float duration)
    {TimerHandler.Instance.AddTimer(timerName, duration);}
    public static void StartTimer(float duration)
    { ExperimentHandler.Instance.StartTimer(duration);}
    
    /// <summary>
    /// Checks if the named timer has reached the specified duration.
    ///  If no name is provided, uses the default trial timer
    /// </summary>
    /// <param name="timerName"> Optional, name assigned when using StartTimer() </param>
    /// <returns></returns>
    public static bool CheckTimer(string timerName) {
        return TimerHandler.Instance.CheckTimer(timerName); }
    public static bool CheckTimer()
    { return ExperimentHandler.Instance.CheckTimer();}

    /// <summary>
    /// Restarts the named timer. If no name is provided, uses the default trial timer
    /// </summary>
    /// <param name="timerName"> Optional, name assigned when using StartTimer() </param>
    public static void RestartTimer(string timerName) { TimerHandler.Instance.RestartTimer(timerName); }
    public static void RestartTimer(){ExperimentHandler.Instance.RestartTimer();}

    /// <summary>
    /// Returns the amount of timer passed since the named timer was started.
    /// If no name is provided, uses the default trial timer
    /// </summary>
    /// <param name="timerName">Optional, name assigned when using StartTimer()</param>
    /// <returns></returns>
    public static float TimePassed(string timerName)
    { return TimerHandler.Instance.GetTimePassed(timerName);}
    public static float TimePassed()
    { return ExperimentHandler.Instance.GetTimePassed();}
    
    /// <summary>
    /// Returns the time remaining on the named timer. If no name is provided, uses the default
    /// trial timer
    /// </summary>
    /// <param name="timerName"> Optional, name assigned when using StartTimer()</param>
    /// <returns></returns>
    public static float TimeRemaining(string timerName) { return TimerHandler.Instance.GetTimeRemaining(timerName);}
    public static float TimeRemaining() { return ExperimentHandler.Instance.GetTimeRemaining();}
   
    /// <summary>
    /// Appends a line to the tagged csv file. Uses the subject file's name but adds _[tag]
    /// </summary>
    /// <param name="tag"> Tag to use in filename </param>
    /// <param name="text"> Line to append</param>
    public static void WriteToTaggedFile(string tag, string text)
    {ExperimentHandler.Instance.WriteToTaggedFile(tag, text);}
    public static void WriteHeaderToTaggedFile(string tag, string text)
    {ExperimentHandler.Instance.WriteHeaderToTaggedFile(tag, text);}

    /// <summary>
    /// Used to override automatic naming scheme
    /// </summary>
    /// <param name="experimentName"></param>
    public static void SetExperimentName(string experimentName)
    { ExperimentHandler.Instance.SetExperimentName(experimentName); }

    /// <summary>
    /// Changes the specified textbox on the experimenter's screen (is not displayed to VR)
    /// </summary>
    /// <param name="whichBox"></param>
    /// <param name="text"></param>
    public static void ChangeExperimenterTextbox(int whichBox, string text)
    {ExperimenterDisplayHandler.Instance.ChangeTextbox(whichBox, text);}

    /// <summary>
    /// Specifies if user wants to use default experimenter textboxes 0-3 
    /// </summary>
    /// <param name="useDefaults"></param>
    public static void DefaultExperimenterTextboxes(bool useDefaults)
    { ExperimenterDisplayHandler.Instance.defaultDisplayTexts = useDefaults; }

    public static void ExperimenterTextboxesEnabled(bool enabled)
    {
        DefaultExperimenterTextboxes(false);
        ChangeExperimenterTextbox(1, "");
        ChangeExperimenterTextbox(2, "");
        ChangeExperimenterTextbox(3, "");
        ChangeExperimenterTextbox(4, "");
        ChangeExperimenterTextbox(5, "");
    }
    
// ****   DATA RECORDING COMMANDS   ****
    /// <summary>
    /// Start recording the camera position every time sxrSettings.recordFrame==currentFrame. Updates automatically
    /// based on sxrSettings.recordFrequency or can be manually called by setting sxrSettings.recordFrame=[frame to record]
    /// Default output location is "Assets/Experiments/[experimentName]/[subject_number]_camera_tracker.csv
    /// </summary>
    /// <param name="recordGaze"></param> Optional (default=false), if true will record gaze screenFixation point
    public static void StartRecordingCameraPos(bool recordGaze) { CameraTracker.Instance.StartRecording(recordGaze); }
    public static void StartRecordingCameraPos() { CameraTracker.Instance.StartRecording(false);}
    
    /// <summary>
    /// Used with StartRecordingCameraPos. Used to pause recording between trials or during rest periods
    /// </summary>
    public static void PauseRecordingCameraPos() {CameraTracker.Instance.PauseRecording();}
    public static void StartRecordingJoystick(){JoystickHandler.Instance.RecordJoystick(true);}
    public static void PauseRecordingJoystick(){JoystickHandler.Instance.RecordJoystick(false);}
    /// <summary>
    /// Start recording the eyetracker information every time sxrSettings.recordFrame==currentFrame. Updates automatically
    /// based on sxrSettings.recordFrequency or can be manually called by setting sxrSettings.recordFrame=[frame to record]
    /// Default output location is "Assets/Experiments/[experimentName]/[subject_number]_eyetracker.csv
    /// </summary>
    public static void StartRecordingEyeTrackerInfo() { GazeHandler.Instance.StartRecording(); }
    /// <summary>
    /// Used with StartRecordingEyeTrackerInfo. Used to pause recording between trials or during rest periods
    /// </summary>
    public static void PauseRecordingEyeTrackerInfo() {GazeHandler.Instance.PauseRecording();}
    
    /// <summary>
    /// *In progress* Adds a tracker to an object to record it's location every time sxrSettings.recordFrame==currentFrame.
    /// Updates automatically based on sxrSettings.recordFrequency or can be manually called by setting
    /// sxrSettings.recordFrame=[frame to record]
    /// Default output location is "Assets/Experiments/[experimentName]/[subject_number]_[object_name]_tracker.csv
    /// </summary>
    /// <param name="name"></param>
    /// <param name="active"></param>
    public static void StartTrackingObject(GameObject gameObj)
    {
        if (gameObj.TryGetComponent(out ObjectTracker objTracker)) objTracker.trackerActive = true;
        else gameObj.transform.AddComponent<ObjectTracker>(); 
    } 
    
    /// <summary>
    /// Used with StartTrackingObject. Used to pause recording between trials or during rest periods
    /// </summary>
    public static void PauseTrackingObject(GameObject gameObj)
    {if (gameObj.TryGetComponent(out ObjectTracker objTracker)) objTracker.trackerActive = false;}
   
// ****   OBJECT MANIPULATION   ****
    /// <summary>
    /// Gets a Unity GameObject using the name, returns the highest object with that name in the hierarchy
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetObject(string name) { return SceneObjectsHandler.Instance.GetObjectByName(name); }

    
    /// <summary>
    /// Moves object to the specified x/y/z distance over [time] seconds
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delta_x"></param>
    /// <param name="delta_y"></param>
    /// <param name="delta_z"></param>
    /// <param name="time"> Optional, instantly moves object if no time is specified</param>
    /// /// <param name="cancelPrevious"> Optional, can be used to override previous movement command</param>
    public static void MoveObjectTo(GameObject obj, float dest_x, float dest_y, float dest_z, float time, bool cancelPrevious) {
        if (time > 0) {
            SceneObjectsHandler.Instance.AddMotionObject(new ObjectMotion(obj,
                new Vector3(dest_x, dest_y, dest_z), time), cancelPrevious); }
        else
            obj.transform.position = new Vector3(dest_x, dest_y, dest_z); }
    public static void MoveObjectTo(GameObject obj, float dest_x, float dest_y, float dest_z, float time)
        { MoveObjectTo(obj, dest_x, dest_y, dest_z, time, false); }
    public static void MoveObjectTo(string objName, float dest_x, float dest_y, float dest_z, float time,
        bool cancelPrevious)
        { MoveObjectTo(GetObject(objName), dest_x, dest_y, dest_z, time, cancelPrevious); }
    public static void MoveObjectTo(string objectName, float dest_x, float dest_y, float dest_z, float time) 
        { MoveObjectTo(objectName, dest_x, dest_y, dest_z, time, false);} 
    public static void MoveObjectTo(string name, float dest_x, float dest_y, float dest_z)
        { MoveObjectTo(name, dest_x, dest_y, dest_z, 0); }

    public static void MoveObjectTo(GameObject gameObject, Vector3 vec, float time)
    { MoveObjectTo(gameObject.name, vec.x, vec.y, vec.z, time); }

    /// <summary>
    /// Moves the specified object to the destination location at the specified speed
    /// </summary>
    /// <param name="gameObject""> Object to move, can be entered as a GameObject or string </param>
    /// <param name="dest_x"> destination x-coordinate</param>
    /// <param name="dest_y">destination y-coordinate</param>
    /// <param name="dest_z">destination z-coordinate</param>
    /// <param name="speed">speed (m/s)</param>
    /// <param name="cancelPrevious">Optional, if true will override any previous move commands. Default is false</param>
    public static void MoveObjectAtSpeedTo(GameObject gameObject, float dest_x, float dest_y, float dest_z, float speed,
        bool cancelPrevious) {
        var distance = Vector3.Distance(gameObject.transform.position, new Vector3(dest_x, dest_y, dest_z)); 
        MoveObjectTo(gameObject, dest_x, dest_y, dest_z, distance/speed, cancelPrevious); }
    public static void MoveObjectAtSpeedTo(GameObject gameObject, float dest_x, float dest_y, float dest_z, float speed) 
        { MoveObjectAtSpeedTo(gameObject, dest_x, dest_y, dest_z, speed, false); }
    public static void MoveObjectAtSpeedTo(string objName, float dest_x, float dest_y, float dest_z, float speed, bool cancelPrevious)
        { MoveObjectAtSpeedTo(GetObject(objName), dest_x, dest_y, dest_z, speed, cancelPrevious); }
    public static void MoveObjectAtSpeedTo(string objName, float dest_x, float dest_y, float dest_z, float speed)
        {MoveObjectAtSpeedTo(objName, dest_x, dest_y, dest_z, speed, false);}
    public static void MoveObjectAtSpeedTo(GameObject gameObject, Vector3 vec, float speed)
    {MoveObjectAtSpeedTo(gameObject, vec.x, vec.y, vec.z, speed, false);}

    /// <summary>
    /// Moves object by the specified x/y/z distance over [time] milliseconds
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delta_x"></param>
    /// <param name="delta_y"></param>
    /// <param name="delta_z"></param>
    /// <param name="time"></param>
    /// /// <param name="cancelPrevious">Optional, if true will override any previous move commands. Default is false</param>
    public static void MoveObject(GameObject gameObject, float delta_x, float delta_y, float delta_z, float time, bool cancelPrevious) {
        var currentPos = gameObject.transform.position;

        if (time > 0) MoveObjectTo(gameObject, currentPos.x + delta_x, currentPos.y + delta_y, currentPos.z + delta_z, time, cancelPrevious);
        else gameObject.transform.position = currentPos + new Vector3(delta_x, delta_y, delta_z); 
    }
    public static void MoveObject(GameObject gameObject, float delta_x, float delta_y, float delta_z, float time)
        { MoveObject(gameObject, delta_x, delta_y, delta_z, time, false);}
    public static void MoveObject(string objName, float delta_x, float delta_y, float delta_z, float time, bool cancelPrevious)
        { MoveObject(GetObject(objName), delta_x, delta_y, delta_z, time, cancelPrevious);}
    public static void MoveObject(string objName, float delta_x, float delta_y, float delta_z, float time)
        { MoveObject(GetObject(objName), delta_x, delta_y, delta_z, time, false);}
    
    /// <summary>
    /// Moves object by the specified x/y/z distance over [time] milliseconds
    /// </summary>
    /// <param name="name"></param>
    /// <param name="delta_x"></param>
    /// <param name="delta_y"></param>
    /// <param name="delta_z"></param>
    /// <param name="speed"> how fast to move (m/s)</param>
    /// /// <param name="cancelPrevious">Optional, if true will override any previous move commands. Default is false</param>
    public static void MoveObjectAtSpeed(GameObject gameObject, float delta_x, float delta_y, float delta_z, float speed, bool cancelPrevious) {
        var currentPos = gameObject.transform.position;
        var destination = currentPos + new Vector3(delta_x, delta_y, delta_z);
        var time = Vector3.Distance(currentPos, destination) / speed; 
        MoveObjectTo(gameObject, currentPos.x + delta_x, currentPos.y + delta_y, currentPos.z + delta_z, time, cancelPrevious);
    }
    public static void MoveObjectAtSpeed(GameObject gameObject, float delta_x, float delta_y, float delta_z,
        float speed) 
        { MoveObjectAtSpeed(gameObject, delta_x, delta_y, delta_z, speed, false); }
    public static void MoveObjectAtSpeed(string objName, float delta_x, float delta_y, float delta_z,
        float speed, bool cancelPrevious
        
        
        ) 
        { MoveObjectAtSpeed(GetObject(objName), delta_x, delta_y, delta_z, speed, cancelPrevious); }
    public static void MoveObjectAtSpeed(string objName, float delta_x, float delta_y, float delta_z,
        float speed)
        { MoveObjectAtSpeed(GetObject(objName), delta_x, delta_y, delta_z, speed, false); }

    
    //TODO Add string based and non-vector inputs
    /// <summary>
    /// Resizes object over the input time (seconds)
    /// </summary>
    /// <param name="gameObj"></param>
    /// <param name="newSize"></param>
    /// <param name="timeTaken"></param>
    public static void ResizeObject(GameObject gameObj, Vector3 newSize, float timeTaken) {
        if (timeTaken == 0)
            gameObj.transform.localScale = newSize; 
        else
            SceneObjectsHandler.Instance.AddMotionResize(new ObjectResize(gameObj, newSize, timeTaken), true); }

    /// <summary>
    /// Checks if the object is being resized
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static bool ObjectResizing(GameObject gameObject)
    { return SceneObjectsHandler.Instance.CheckForObjectInResize(gameObject);}
    
    /// <summary>
    /// Follows the specified parabola in world space coordinates.  Matches the tangent of curve at current x-value
    /// </summary>
    /// <param name="gameObj"></param>
    /// <param name="negCurve"></param>
    /// <param name="speed"></param>
    /// <param name="verticalStretch"></param>
    /// <param name="horizontalShift"></param>
    public static void FollowParabola(GameObject gameObj, bool negCurve, float speed, float verticalStretch, float horizontalShift)
    {
        var rt = gameObj.transform.rotation.eulerAngles;
        float rotation = rt.y  + (negCurve ? -1 : 1) * 
            (float) Math.Atan( ((gameObj.transform.position.x*verticalStretch)-horizontalShift)); // Set rotation to tangent of curve 
        rotation = Math.Clamp(rotation, 0f, 180f); 
        gameObj.transform.rotation = Quaternion.Euler(rt.x, rotation, rt.z); // rotate to tangent at position
        gameObj.transform.position += speed * Time.deltaTime * gameObj.transform.forward ; // move forward 
    }

    public static bool ObjectMoving(GameObject gameObj)
    { return SceneObjectsHandler.Instance.CheckForObjectInMotion(gameObj);}
    
    /// <summary>
    /// Spawns a primitive game object at the specified location. Default location is 0,0,0
    /// </summary>
    /// <param name="type"> Use UnityEngine.PrimitiveType (.Sphere, .Cube, etc)</param>
    /// <param name="name"> Name to give the spawned object</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void SpawnObject(PrimitiveType type, string name, float x, float y, float z) {
        var obj = GameObject.CreatePrimitive(type);
        obj.name = name; 
        MoveObjectTo(obj, x, y, z, 0); }
    public static void SpawnObject(PrimitiveType type, string name)
        { SpawnObject(type, name, 0, 0, 0); }

    /// <summary>
    /// Copys the object and assigns the provided name to the copy
    /// </summary>
    /// <param name="objectToCopy"></param>
    /// <param name="name"></param>
    public static void SpawnObject(GameObject objectToCopy, string name) {
        var obj = GameObject.Instantiate(objectToCopy);
        obj.name = name; }

    /// <summary>
    /// Checks if the item with the provided name exists
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool ObjectExists(string name)
        { return SceneObjectsHandler.Instance.ObjectExists(name); }
    
    /// <summary>
    /// Sets the object's visibility through (de)activating MeshRenderer
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="visible"></param>
    public static void ObjectVisibility(GameObject obj, bool visible) 
    { obj.GetComponent<MeshRenderer>().enabled = visible; }

    public static void ObjectVisibility(string objName, bool visible) 
    { ObjectVisibility(sxr.GetObject(objName), visible); }
    
    /// <summary>
    /// Allows the object to be grabbed by touching the
    /// object with a VR controller and pulling trigger.
    /// Can take in object name (string) or Unity GameObject
    /// </summary>
    /// <param name="objectName"></param>
    public static void MakeObjectGrabbable(GameObject gameObject) {
        if (gameObject.GetComponents<GrabbableObject>().Length == 0)
            gameObject.AddComponent<GrabbableObject>(); } 
    public static void MakeObjectGrabbable(string objectName)
        {MakeObjectGrabbable(GetObject(objectName)); }

    /// <summary>
    /// Adds RigidBody (Physics) to the specified object. Object can be passed by name or
    /// as a Unity GameObject
    /// </summary>
    /// <param name="objectName"> Name of the object</param>
    /// <param name="useGravity"> If the object will be affected by gravity.  Optional - Default=true</param>
    public static void EnableObjectPhysics(GameObject gameObject, bool useGravity) {
            if (gameObject.GetComponents<Rigidbody>().Length == 0)
                gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = useGravity; }
    public static void EnableObjectPhysics(GameObject gameObject)
        { EnableObjectPhysics(gameObject, true);}
    public static void EnableObjectPhysics(string objectName, bool useGravity)
        { EnableObjectPhysics(GetObject(objectName), useGravity);}
    public static void EnableObjectPhysics(string objectName)
        {EnableObjectPhysics(objectName, true);}

    /// <summary>
    /// Checks if the two specified objects are touching. Can take in object names as strings,
    /// or Unity GameObjects
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public static bool CheckCollision(GameObject obj1, GameObject obj2)
        { return CollisionHandler.Instance.ObjectsCollidersTouching(obj1, obj2);}
    public static bool CheckCollision(string obj1, string obj2)
        { return CheckCollision(GetObject(obj1), GetObject(obj2));}
    
// *****   Extras   **** 
    /// <summary>
    /// Royalty free sounds provided with sXR
    /// </summary>
    public enum ProvidedSounds{Beep, Buzz, Ding, Stop}
    
    /// <summary>
    /// Plays one of the sounds provided by sXR. Use sxr.ProvidedSounds (.Beep, .Buzz, .Ding, .Stop)
    /// </summary>
    /// <param name="sound"></param>
    public static void PlaySound(ProvidedSounds sound){}
    
    /// <summary>
    /// Searches all 'Resources' folders to find the sound with the specified filename.
    /// Do not include the file extension (e.g. 'mySound' and not 'mySound.mp3'
    /// </summary>
    /// <param name="soundName"></param>
    public static void PlaySound(string soundName){SoundHandler.Instance.CustomSound(soundName);}
    
    /// <summary>
    /// Applies the list of specified full-screen shaders. Can take shader indexes (int) or shader names (string)
    /// </summary>
    /// <param name="shaderNames"></param>
    public static void ApplyShaders(List<string> shaderNames)
        {ShaderHandler.Instance.ActivateShaders(shaderNames);}
    public static void ApplyShaders(List<int> shaderIndexes)
        {ShaderHandler.Instance.ActivateShaders(shaderIndexes);}

    /// <summary>
    /// Applies a specific shader specified by name
    /// </summary>
    /// <param name="shaderName"></param>
    /// <param name="turnOffOthers"> if true, turns off all other full screen shaders</param>
    public static void ApplyShader(string shaderName, bool turnOffOthers) {
        if (turnOffOthers) ApplyShaders(new List<string> {shaderName});
        else ShaderHandler.Instance.currentActiveNames.Add(shaderName); }

    public static bool SetShaderVariables<T>(string shaderName, string variableName, T value)
    {
        return ShaderHandler.Instance.ModifyShader(shaderName, variableName, value); 
    }

    /// <summary>
    /// Launches the SRanipal Eye Calibration tool.  Returns true if calibration is successful
    /// </summary>
    /// <returns></returns>
    public static bool LaunchEyeCalibration() {
        #if SXR_USE_SRANIPAL
            return GazeHandler.Instance.LaunchEyeCalibration(); 
        #endif
        return false; }

    public static string GetFullGazeInfo() { return GazeHandler.Instance.GetFullGazeInfo(); }

    public static Vector2 GetGazeScreenPos() { return GazeHandler.Instance.GetScreenFixationPoint(); }
    
// *****   DEBUG COMMANDS   **** 4
    /// <summary>
    /// Displays a debug message every [frameFrequency] frames if sxrSettings.debugMode==Frequent or every
    /// frame if sxrSettings.DebugMode==Framewise
    /// </summary>
    /// <param name="toWrite"></param>
    /// <param name="frameFrequency"></param> Optional, how often to display message, default=50 frames
    public static void DebugLog(string toWrite, int frameFrequency) {
       if (sxrSettings.Instance.debugMode == sxrSettings.DebugMode.Framewise ||
           (sxrSettings.Instance.debugMode == sxrSettings.DebugMode.Frequent &&
            sxrSettings.Instance.GetCurrentFrame() % frameFrequency == 0))
           Debug.Log("Frame " + sxrSettings.Instance.GetCurrentFrame() + ": " + toWrite); }
    public static void DebugLog(string toWrite) { DebugLog(toWrite, 50); }
    
   
}   

