namespace sxr_internal{
#if SXR_USE_STEAMVR
    using UnityEngine;
    using Valve.VR;


    /// <summary>
    /// [Singleton] Easy boolean access to SteamVR controller buttons
    /// Can be attached to any GameObject but the parent object of SteamVR
    /// controllers is recommended. Custom button configurations require SteamVR actions to be properly
    /// set 
    /// 
    /// On Awake:
    ///     Initializes singleton
    /// On Start:
    ///     Adds OnStateDownListeners for all buttons on both controllers
    /// On Update:
    ///     N/A
    /// </summary>
    public class SteamControllerVR : ControllerVR {

        [SerializeField] SteamVR_Action_Boolean 
            TriggerOnOff,
            SideButtonOnOff,
            TrackPadRightOnOff,
            TrackPadLeftOnOff,
            TrackPadUpOnOff,
            TrackPadDownOnOff;
        
        // * Left Hand * //
        void LH_TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
            buttonPressed[(int) ControllerButton.LH_Trigger] = true; }

         void LH_SideButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_SideButton] = true; }

         void LH_TrackPadUpDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadUp] = true; }

         void LH_TrackPadLeftDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadLeft] = true; }

         void LH_TrackPadRightDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadRight] = true; }

         void LH_TrackPadDownDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadDown] = true; }

         void LH_TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_Trigger] = false; }

         void LH_SideButtonUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_SideButton] = false; }

         void LH_TrackPadUpUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadUp] = false; }

         void LH_TrackPadLeftUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadLeft] = false; }

         void LH_TrackPadRightUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadRight] = false; }

         void LH_TrackPadDownUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.LH_TrackPadDown] = false; }

        // * Right Hand * //
        void RH_TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
            buttonPressed[(int) ControllerButton.RH_Trigger] = true; }

         void RH_SideButtonDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_SideButton] = true; }

         void RH_TrackPadUpDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadUp] = true; }

         void RH_TrackPadLeftDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadLeft] = true; }

         void RH_TrackPadRightDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadRight] = true; }

         void RH_TrackPadDownDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadDown] = true; }

         void RH_TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_Trigger] = false; }

         void RH_SideButtonUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_SideButton] = false; }

          void RH_TrackPadUpUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
              buttonPressed[(int) ControllerButton.RH_TrackPadUp] = false; }

         void RH_TrackPadLeftUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadLeft] = false; }

         void RH_TrackPadRightUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadRight] = false; }

         void RH_TrackPadDownUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
             buttonPressed[(int) ControllerButton.RH_TrackPadDown] = false; }

        
        void Start() {
            TriggerOnOff.AddOnStateDownListener(LH_TriggerDown, SteamVR_Input_Sources.LeftHand);
            SideButtonOnOff.AddOnStateDownListener(LH_SideButtonDown, SteamVR_Input_Sources.LeftHand);
            TrackPadLeftOnOff.AddOnStateDownListener(LH_TrackPadLeftDown, SteamVR_Input_Sources.LeftHand);
            TrackPadRightOnOff.AddOnStateDownListener(LH_TrackPadRightDown, SteamVR_Input_Sources.LeftHand); 
            TrackPadUpOnOff.AddOnStateDownListener(LH_TrackPadUpDown, SteamVR_Input_Sources.LeftHand);
            TrackPadDownOnOff.AddOnStateDownListener(LH_TrackPadDownDown, SteamVR_Input_Sources.LeftHand);
            
            TriggerOnOff.AddOnStateUpListener(LH_TriggerUp, SteamVR_Input_Sources.LeftHand);
            SideButtonOnOff.AddOnStateUpListener(LH_SideButtonUp, SteamVR_Input_Sources.LeftHand);
            TrackPadLeftOnOff.AddOnStateUpListener(LH_TrackPadLeftUp, SteamVR_Input_Sources.LeftHand);
            TrackPadRightOnOff.AddOnStateUpListener(LH_TrackPadRightUp, SteamVR_Input_Sources.LeftHand); 
            TrackPadUpOnOff.AddOnStateUpListener(LH_TrackPadUpUp, SteamVR_Input_Sources.LeftHand);
            TrackPadDownOnOff.AddOnStateUpListener(LH_TrackPadDownUp, SteamVR_Input_Sources.LeftHand);
            
            TriggerOnOff.AddOnStateDownListener(RH_TriggerDown, SteamVR_Input_Sources.RightHand);
            SideButtonOnOff.AddOnStateDownListener(RH_SideButtonDown, SteamVR_Input_Sources.RightHand);
            TrackPadLeftOnOff.AddOnStateDownListener(RH_TrackPadLeftDown, SteamVR_Input_Sources.RightHand);
            TrackPadRightOnOff.AddOnStateDownListener(RH_TrackPadRightDown, SteamVR_Input_Sources.RightHand); 
            TrackPadUpOnOff.AddOnStateDownListener(RH_TrackPadUpDown, SteamVR_Input_Sources.RightHand);
            TrackPadDownOnOff.AddOnStateDownListener(RH_TrackPadDownDown, SteamVR_Input_Sources.RightHand);
            
            TriggerOnOff.AddOnStateUpListener(RH_TriggerUp, SteamVR_Input_Sources.RightHand);
            SideButtonOnOff.AddOnStateUpListener(RH_SideButtonUp, SteamVR_Input_Sources.RightHand);
            TrackPadLeftOnOff.AddOnStateUpListener(RH_TrackPadLeftUp, SteamVR_Input_Sources.RightHand);
            TrackPadRightOnOff.AddOnStateUpListener(RH_TrackPadRightUp, SteamVR_Input_Sources.RightHand); 
            TrackPadUpOnOff.AddOnStateUpListener(RH_TrackPadUpUp, SteamVR_Input_Sources.RightHand);
            TrackPadDownOnOff.AddOnStateUpListener(RH_TrackPadDownUp, SteamVR_Input_Sources.RightHand); }

        void Update() {
            buttonPressed[(int) ControllerButton.Trigger] = 
                buttonPressed[(int) ControllerButton.LH_Trigger] || 
                buttonPressed[(int) ControllerButton.RH_Trigger]; 
            
            buttonPressed[(int) ControllerButton.SideButton] = 
                buttonPressed[(int) ControllerButton.LH_SideButton] || 
                buttonPressed[(int) ControllerButton.RH_SideButton];
            
            buttonPressed[(int) ControllerButton.TrackPadRight] = 
                buttonPressed[(int) ControllerButton.LH_TrackPadRight] || 
                buttonPressed[(int) ControllerButton.RH_TrackPadRight]; 
            
            buttonPressed[(int) ControllerButton.TrackPadLeft] = 
                buttonPressed[(int) ControllerButton.LH_TrackPadLeft] || 
                buttonPressed[(int) ControllerButton.RH_TrackPadLeft];
            
            buttonPressed[(int) ControllerButton.TrackPadUp] = 
                buttonPressed[(int) ControllerButton.LH_TrackPadUp] || 
                buttonPressed[(int) ControllerButton.RH_TrackPadUp]; 
            
            buttonPressed[(int) ControllerButton.TrackPadDown] = 
                buttonPressed[(int) ControllerButton.LH_TrackPadDown] || 
                buttonPressed[(int) ControllerButton.RH_TrackPadDown]; }
        

        // Singleton initiated on Awake()
        public static SteamControllerVR Instance;
        private void Awake()
        {
            if (TriggerOnOff == null) TriggerOnOff = SteamVR_Actions._default.GrabPinch;
            if (SideButtonOnOff == null) SideButtonOnOff = SteamVR_Actions._default.GrabGrip;
            if (TrackPadRightOnOff == null) TrackPadRightOnOff = SteamVR_Actions._default.SnapTurnRight;
            if (TrackPadLeftOnOff == null) TrackPadLeftOnOff = SteamVR_Actions._default.SnapTurnLeft;
            if (TrackPadUpOnOff == null) TrackPadUpOnOff = SteamVR_Actions._default.Teleport;
            if (TrackPadDownOnOff == null) TrackPadRightOnOff = SteamVR_Actions._default.Teleport;
            if(TrackPadDownOnOff == SteamVR_Actions._default.Teleport) Debug.Log("SteamVR Trackpad Down set as default" +
                " (Teleport), set to custom key to enable.");
            if (Instance == null) {Instance  = this; DontDestroyOnLoad(gameObject.transform.root); }else { Destroy(gameObject); } }
    }
#else
    public class SteamControllerVR : ControllerVR { }
#endif
} 

