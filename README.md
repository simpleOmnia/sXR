![sXR Logo](https://github.com/unity-sXR/ReadmeImages/blob/main/sxrlogo.png)

[Background](#background) | [Installation](#installation) | [Beginners](#for-beginners) | [Tutorials](#tutorials) | [Features](#features) | [Commands List](#commands-list) | [Coming Soon...](#coming-soon) | [Requested Features](#requested-features) | [Version History](#version-history)


# Background
simpleXR (sXR) is a software package designed to facilitate rapid development of XR experiments. Researchers in many different fields are starting to use virtual/augmented reality for studying things like learning, navigation, vision, or fear. However, the packages previously available for developing in XR were directed at computer scientists or people with a strong background in programming. sXR makes programming as simple as possible by providing one easy to use library with single line commands for more complicated tasks. The package is built for Unity and can be downloaded as a template project or added to previous projects with little effort. Just replace the scene's camera with the sXR_prefab and you'll gain access to multiple user interfaces and a plethora of commands that will allow you to start gathering data in days, not months. Extended reality is hard...  simpleXR is simple.

# Installation
Installation is now available through the package manager.  Add from Git URL (with '.git' at the end), then right click to add sxr_prefab to your scene. For a full guide, see the wiki [Installation page](https://github.com/simpleOmnia/sXR/wiki/Installation)

Once installed, "sxr." commands should be automatically available in your IDE without any further setup: 
![ideAutofill](https://github.com/simpleOmnia/ReadmeImages/blob/main/ideAutofills.gif)

# Reporting Bugs, Contributing, and Requesting Features
While every attempt has been made to ensure sXR is compatible with all devices/Unity projects and bug free, there is the chance of encountering problems when using sXR. Please make a new issue on Github if you run into anything that doesn't seem to work how it should, or if there's a new feature you would like to see:
![sxr_issue](https://github.com/simpleOmnia/ReadmeImages/blob/main/sxr_issue.png)

simpleXR is open-source, meaning the community is free to contribute. If you have a forked repository and have added a feature you'd like to see in the main repository, please make sure your code can be merged with the main repository before submitting a pull request. If it cannot merge, please include detailed comments on what has been changed in the main repository that is causing a conflict. To avoid conflicts, it's best to frequently update your working repository to the most up to date version of the package. 

# For Beginners
While sXR makes Unity much simpler, it can still be complicated if you're just starting out. The project contains a sample experiment with a step-by-step video walkthrough [(youtube link)](https://youtu.be/cZrVEEOJrkM?si=MmjcHS71fh5Lg3Xy). If you don't understand the ExperimentScript.cs file of the sample experiment, I recommend watching the entire video as it breaks down the development process. Feel free to reach out if you get stuck!

# Tutorials
[The Basics](https://github.com/unity-sXR/sXR/wiki/The-Basics)

[Storing and accessing variables](https://github.com/simpleOmnia/sXR/wiki/Storing-and-Accessing-Variables)

[Sample experiment video walkthrough (for beginners)](https://youtu.be/cZrVEEOJrkM?si=MmjcHS71fh5Lg3Xy)

[Vive Pro Eye Setup](https://github.com/unity-sXR/sXR/wiki/Vive-Pro-Eye-Setup)

[Replay Mode](https://github.com/unity-sXR/sXR/wiki/Replay-Mode)

# Features
**Autosave** - Enabled by default, can be turned off in the sXR tab of the toolbar.  Automatically saves the scene when "Play" is pressed.  Be sure to use "File->Save As" to name your scene or it will ask where you want to save.  

**Automatic VR** - Enabled by default, can be turned off in the sXR tab of the toolbar.  Automatically switches between moving the camera with keyboard controls (arrow keys/WASD) or with HMD tracking.

**Safety Wall** - Enabled by default, can be turned off in the sXR tab of the toolbar. Sometimes we don't want participants to see the wireframe boundary of SteamVR and need a bit more control over where the HMD will stop you.  Declare the size of your space in the sXR tab and a "Stop" message will appear when the headset reaches the border. Do not rely solely on this feature since HMD tracking can be unstable. Always have participants walk carefully when they're wearing a headset.  

**Automatic position/eye-tracker recording** - Using 'sxr.StartRecordingCameraPos()' or 'sxr.StartRecordingEyeTrackerInfo()' will enable automatic recording of the position/eye-tracker info at the frequency you specify in the sXR tab of the toolbar. Synchronizes eye-tracker and position information allowing for the scene to be replayed with the participant's gaze highlighted

**Variable management** - Allows you to access/change variables globally with sxr.SetInt/SetFloat/SetString/SetBool and sxr.GetInt/GetFloat/GetString/GetBool. The variables are saved to the computer and will load automatically with Unity. These variables can also be viewed/added/changed with the Unity editor in the sXR tab. 


# Commands List
[Variable Managment](#variable-management)

[Experiment Control](#experiment-control)

[Data Recording](#data-recording)

[User Interface](#user-interface)

[Input Devices](#input-devices)

[Object Manipulation](#object-manipulation)

[Extras](#extras)

## Variable Management
**SetPref()** Sets a Unity PlayerPref (See https://docs.unity3d.com/ScriptReference/PlayerPrefs.html). Variable persists upon restarting Unity

**SetInt()/SetBool()/SetFloat()/SetString()** Sets the specified type of variable as a PlayerPref

**GetInt()/GetBool()/GetFloat/GetString()** Returns the variable with the specified name

## Experiment Control
**SetExperimentName()** - Used to override automatic naming scheme

**ChangeExperimenterTextbox()** - Chooses what to display on the specified textbox of the experimenter's screen

**DefaultExperimenterTextboxes()** - Specifies whether or not to use the defaults for textboxes 1-3 on the experimenter screen

**ExperimenterTextboxEnabled()** - Used to disable/re-enable textboxes on the experimenter's screen.  

**GetPhase()** - Returns the current phase of the experiment. The experiment flow hierarchy is Phase > Block > Trial > Step

**GetBlock()** - Returns the current block number of the experiment

**GetTrial()** - Returns the current trial number of the experiment

**GetStepInTrial()** - Returns the current step of the trial

**NextPhase()** - Increments to the next phase, resets block/trial/step to zero

**NextBlock()** - Increments to the next block, resets trial/step to zero

**NextTrial()** - Increments to the next trial, resets step to zero

**SetStep()** - Sets the step to the specified number

**StartTimer()** - Starts a timer with the provided name. If no name is provided, starts the default trial timer. Requires a specified duration to use CheckTimer()

**PauseTimer()** - Pauses the timer with the provided name. If no name is provided, pauses the default trial timer.

**CheckTimer()** - Checks if the duration of a timer has passed. If it is a named timer, deletes the timer once duration is reached and CheckTimer() is called

**RestartTimer()** - Restarts the timer with the provided name. If no name is provided, restarts the default trial timer

**TimePassed()** - Checks how much time has passed on the timer with the provided name. If no name is provided, checks the default trial timer

**TimeRemaining()** - Checks how much time is left on the timer with the provided name. If no name is provided, checks the default trial timer

## Data Recording

**WriteHeaderToTaggedFile()** - Writes the header line to the csv file with the specified tag. Any columns that will be made in WriteToTaggedFile() should have a column title declared with this function

**WriteToTaggedFile()** - Writes the line provided to the csv file with the specified tag

**StartRecordingCameraPos()** - Starts recording the world position of the vrCamera at the interval specified in sXR_settings

**PauseRecordingCameraPos()** - Pauses recording the camera until StartRecordingCameraPos() is called again

**StartRecordingJoystick()** - Starts recording the joystick position at the interval specified in sXR_settings

**PauseRecordingJoystic()** - Pauses recording the joystick until StartRecordingJoystick() is called again

**StartRecordingEyeTrackerInfo()** - Starts recording the information provided by an eyetracker at the interval specified in sXR_settings. Supports screenFixationX, screenFixationY, gazeFixationX, gazeFixationY, gazeFixationZ, leftEyePositionX, leftEyePositionY, leftEyePositionZ, rightEyePositionX, rightEyePositionY, rightEyePositionZ, leftEyeRotationX, leftEyeRotationY,leftEyeRotationZ, rightEyeRotationX, rightEyeRotationY, rightEyeRotationZ, leftEyePupilSize, rightEyePupilSize, leftEyeOpenAmount, and rightEyeOpenAmount (if these options are supported by the headset eyetracker, pupil size and eye open amount are not available through OpenXR). 

**PauseRecordingEyeTrackerInfo()** - Pauses recording the eyetracker until StartRecordingEyeTrackerInfo() is called again

**StartTrackingObject()** - Starts recording the position of the object with the provided name at the interval specified in sXR_settings

**PauseTrackingObject()** - Pauses recording the position of the object with the provided name until StartTrackingObject() is called again

## User Interface
**InputSlider()** - Creates and displays a slider that the participant can manipulate with the controller laser

**InputDropdown()** - Creates and displays a dropdown menu that the participant can manipulate with the controller laser 

**ParseInputUI()** - Gets the response from an open InputSlider or InputDropdown

**DisplayText()** - Displays text to the VR headset at the specified position

**HideText()** - Hides text on the VR headset at the specified position

**HideAllText()** - Hides all text locations displayed on the VR headset

**DisplayImage()** - Displays an image at the specified location on the VR headset

**DisplayPrebuilt()** - Displays prebuilt "STOP", "Loading", "Finished", and "Eye Error" screens

**HideImageUI()** - Hides an image at the specified location on the VR headset

**HideImagesUI()** - Hides all images displayed on the VR headset

## Input Devices
**GetTrigger()** - Returns true if the VR controller trigger, a joystick trigger, or the keyboard's spacebar are pressed

**KeyHeld()** - Returns true continuously with the specified delay if the specified keyboard key is pressed

**InitialKeyPress()** - Returns true only on the initial frame a keyboard key is pressed

**KeyReleased()** - Returns true only on the initial frame a keyboard key is released

**GetJoystickDirection()** - Returns which direction the joystick is pushed to (Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None)

**CheckController()** - Checks to see if the specified VR controller button is pressed. LH for lefthand, RH for righthand. Unhanded designations return true if left or right buttons are pressed (LH_Trigger, LH_SideButton, LH_TrackPadRight, LH_TrackPadLeft, LH_TrackPadUp, LH_TrackPadDown, LH_ButtonA, LH_ButtonB, RH_Trigger, RH_SideButton, RH_TrackPadRight, RH_TrackPadLeft, RH_TrackPadUp, RH_TrackPadDown, RH_ButtonA, RH_ButtonB, Trigger, SideButton, TrackPadRight, TrackPadLeft, TrackPadUp, TrackPadDown, ButtonA, ButtonB)

## Object Manipulation
**GetObject()** - Returns the Unity GameObject with the provided name

**ObjectExists()** - Checks if an object exists with the provided name

**MoveObject()** - Moves the object with the provided name from it's current location by the amount specified. If a time is given, the object will take that duration to reach the target location

**MoveObjectTo()** - Moves the object with the provided name to the specified location. If a time is given, the object will take that duration to reach the target location

**MoveObjectAtSpeedTo()** - Moves the object to the specified location moving at the given speed

**RotateObject()** - Rotates the object with the provided name by the specified amount. If a time is given, the object will take that duration to reach the target rotation

**RotateObjectTo()** - Rotates the object with the provided name to the specified rotation values. If a time is given, the object will take that duration to reach the target rotation

**RotateObjectAtSpeedTo()** - Rotates the object to the specified rotation moving at the given speed

**FollowParabola()** - Moves object along the specified parabola

**ObjectMoving()** - Returns true if the object is currently in controlled motion

**ResizeObject()** - Resizes the object over the specified timeframe

**ObjectResizing()** - Returns true if object is currently being resized

**SpawnObject()** - Can be used to spawn a Unity PrimitiveType (Sphere, Cube, Plane, etc.).  Can also be used to copy an object if an object name is provided. 

**ObjectVisibility()** - Sets the object's and all children object's renderers to be enabled/disabled

**IsObjectVisible()** - Returns true if the object's renderers are enabled

**MakeObjectGrabbable()** - Checks for a collision between the object with the provided name and either of the controllers. If the controller is touching the object and the trigger is pulled, the item can be moved with the controller. 

**EnableObjectPhysics()** - Attaches a rigidbody to the object, enabling Unity physics to apply to the object. Gravity can be turned off by passing useGravity=false

**ModifyObjectsPhysics()** - Allows you to stet bounciness, friction, and dynamic friction of the GameObject

**CheckCollision()** - Checks if the two specified objects have collided.  Automatically adds a mesh collider if an object doesn't have any other collider

## Extras
**PlaySound()** - Plays the sound with the specified name. Can also play 'sxr.ProvidedSounds' (Beep, Buzz, Ding, and Stop)

**PlayRandomSoundInFolder()** - Selects a random sound in the specified folder and plays it

**ApplyShaders()** - Activates the shaders with the names provided.  

**ApplyShader()** - Activates the shader with the provided name. Will de-activate all other shaders if turnOffOthers=true is passed

**SetShaderVariables()** - Used to modify variables for the specified shader

**LaunchEyeCalibration()** - If using SRanipal, will launch the eye calibration tool and return true if calibration is successful

**SendHaptic()** - Sends a vibration to the controller

**ControllerVisual** - Turn on/off the controller/controller laser visualization

**GetFullGazeInfo()** - Returns all available gaze info. Supports screenFixationX, screenFixationY, gazeFixationX, gazeFixationY, gazeFixationZ, leftEyePositionX, leftEyePositionY, leftEyePositionZ, rightEyePositionX, rightEyePositionY, rightEyePositionZ, leftEyeRotationX, leftEyeRotationY,leftEyeRotationZ, rightEyeRotationX, rightEyeRotationY, rightEyeRotationZ, leftEyePupilSize, rightEyePupilSize, leftEyeOpenAmount, and rightEyeOpenAmount (if these options are supported by the headset eyetracker, pupil size and eye open amount are not available through OpenXR).

**SetIfNull()** - Checks if the referenced GameObject is null. If null, sets the GameObject to the first object with the specified string.

**DebugLog()** - Displays a debug every message based on the frequency specified in sXR_settings and passed in as the 'frameFrequency' 


# Coming Very Soon...
~~Camera passthrough for augmented reality (on supported headsets)~~ Augmented Reality sample scene available, requires SteamVR for now

~~Playback mode - Replay the participants view and highlight their gaze)~~ Playback mode is available, look at the Github wiki for more info

Eye-tracking/shader tutorial + sxr.commands for assigning shaders to objects

Gaze-ray objects - record when the user looks at certain objects

Command Recorder - record when the user presses buttons (for replaying scene)

VR Controller "Touch" option for buttons (i.e. when the controller button is  touched but not pressed)

Hand-tracking without controllers (for supported devices- Vive Pro Eye, Quest 2, etc)

# Requested Features (May implement if enough people request)
Computer Vision prefab - train ONYXX models and use them with Unity's Barracuda

