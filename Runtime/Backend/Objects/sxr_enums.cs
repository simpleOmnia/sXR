namespace sxr_internal
{
    /// enum Types used in sxr commands
   
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
    /// Pre-built positions to display text 
    /// </summary>
    public enum TextPosition{Top, MiddleTop, MiddleBottom, Bottom, TopLeft}

    /// <summary>
    /// Eight directions of joystick + 'None'
    /// </summary>
    public enum JoyStickDirection { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight, None }

    /// <summary>
    /// sxr supported controller buttons
    /// LH designates LeftHand
    /// RH designates RightHand
    /// No designation will be true if either controller is pressed
    /// </summary>
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
    /// Royalty free sounds provided with sXR
    /// </summary>
    public enum ProvidedSounds{Beep, Buzz, Ding, Stop}
    
    /// <summary>
    /// The visual aspects of the VR controllers, used with ControllerVisual()
    /// </summary>
    public enum ControlVisualType
    { LeftControllerCapsule, RightControllerCapsule, LeftUI, RightUI, LeftEnvironment, RightEnvironment }
}