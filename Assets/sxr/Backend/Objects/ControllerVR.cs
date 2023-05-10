using System;
using UnityEngine;

namespace sxr_internal
{
    /// <summary>
    /// Parent class for controllers, contains the last time each button
    /// returns "true" and if the button is currently pressed
    /// </summary>
    public class ControllerVR : MonoBehaviour {
        public float[] buttonTimers = new float[Enum.GetNames(typeof(sxr_internal.ControllerButton)).Length];
        public bool[] buttonPressed = new bool[Enum.GetNames(typeof(sxr_internal.ControllerButton)).Length];

        public bool useController;

        public void EnableControllers()
        { useController = true; } 
    }
}
