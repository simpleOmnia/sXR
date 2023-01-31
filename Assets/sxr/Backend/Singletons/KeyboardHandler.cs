using System.Collections.Generic;
using UnityEngine;

namespace sxr_internal {
    public class KeyboardHandler : MonoBehaviour {
        
        public enum InputMode{Both, Legacy, System}
        private InputMode inputMode;
        public InputMode GetUnityInputMode() { return inputMode; }

        private class KeyTimer {
            public float lastTrueReturned;
            public KeyCode keyCode;

            public KeyTimer(float lastTrueReturned, KeyCode keyCode) {
                this.lastTrueReturned = lastTrueReturned;
                this.keyCode = keyCode;} }

        private List<KeyTimer> keyTimers = new List<KeyTimer>();

        public bool KeyHeld(KeyCode whichKey, float frequency) {
            for (int i=0; i<keyTimers.Count; i++) {
                if (keyTimers[i].keyCode == whichKey) {
                    if (Input.GetKey(whichKey)) {
                        if (Time.time - keyTimers[i].lastTrueReturned > frequency) {
                            keyTimers[i].lastTrueReturned = Time.time;
                            sxr.DebugLog("Input KeyCode " + whichKey + " detected at " +
                                         Time.time + " (freq=" + frequency + ")");
                            return true; }
                        else {
                            sxr.DebugLog("Input KeyCode " + whichKey +
                                         " detected before delay (frequency=" + frequency + ")");
                            return false; } }
                    else {
                        sxr.DebugLog("Input KeyCode " + whichKey + "found in KeyTimer list but is not pressed");
                        return false; } } }

            keyTimers.Add(new KeyTimer(Time.time, whichKey));
            sxr.DebugLog("Input KeyCode " + whichKey + " not found in KeyTimer list, adding and checking for key pressed");
            if (Input.GetKey(whichKey))
                return true; 
            return false; }

        void Start() {
        #if ENABLE_LEGACY_INPUT_MANAGER
                 inputMode = InputMode.Legacy; 
        #endif
        #if ENABLE_INPUT_SYSTEM
                inputMode = inputMode==InputMode.Legacy ? InputMode.Both : InputMode.System;    
        #endif

            Debug.Log("Input Mode: " + inputMode);
            if (inputMode == InputMode.Legacy)
                Debug.LogWarning("Using Legacy Input may cause some sXRfeatures " +
                                 "(such as eye-tracking or controller pose) to no longer work. " +
                                 "Currently, we recommend using both input modes by going to: \n" +
                                 "Edit->Project Settings->Player->Other Settings->Active Input Handling");
            if(inputMode == InputMode.System)
                Debug.LogWarning("Using the new Input System may cause some sXRfeatures " +
                                 "(such as keyboard and joystick controls) to no longer work. " +
                                 "Currently, we recommend using both input modes by going to: \n" +
                                 "Edit->Project Settings->Player->Other Settings->Active Input Handling"); }

        // Singleton initiated on Awake()
        public static KeyboardHandler Instance; 
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
