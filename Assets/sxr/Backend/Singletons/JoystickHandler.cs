using System;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// [Singleton] Used to access joystick input
    /// Contains: 
    ///     JoyStickDirection GetDirection()
    ///     void DisplayDirectionUI()
    /// </summary>
    public class JoystickHandler : MonoBehaviour
    {
        private bool headerPrinted; 
        public void RecordJoystick(bool record)
        {
            if(record & !headerPrinted){
                sxr.WriteHeaderToTaggedFile("", "joystickHor,joysticVert");
                headerPrinted = true; }
            if(record && sxrSettings.Instance.RecordThisFrame())
                sxr.WriteToTaggedFile("", Input.GetAxis("Horizontal") + "," + Input.GetAxis("Vertical"));
                
        }
        
        /// <summary>
        /// Returns the direction the joystick is being pressed 
        /// </summary>
        /// <returns></returns>
        public sxr.JoyStickDirection GetDirection() {
            float horizontal_input = Input.GetAxis("Horizontal");
            float vertical_input = Input.GetAxis("Vertical");
            if (horizontal_input > .5 & Math.Abs(vertical_input) < .2) return sxr.JoyStickDirection.Right;
            if (horizontal_input > .5 & vertical_input < -.5) return sxr.JoyStickDirection.DownRight;
            if (Math.Abs(horizontal_input) < .2 & vertical_input < -.5) return sxr.JoyStickDirection.Down;
            if (horizontal_input < -.5 & vertical_input < -.5 ) return sxr.JoyStickDirection.DownLeft;
            if (horizontal_input < -.5 & Math.Abs(vertical_input) < .2) return sxr.JoyStickDirection.Left;
            if (horizontal_input < -.5 & vertical_input > .5) return sxr.JoyStickDirection.UpLeft;
            if (Math.Abs(horizontal_input) < .2 & vertical_input > .5) return sxr.JoyStickDirection.Up;
            if (horizontal_input > .5 & vertical_input > .5) return sxr.JoyStickDirection.UpRight;
            
            return sxr.JoyStickDirection.None; }
        
        /// <summary>
        /// Enables UI_Handler elements at the sides/corners of the screen if the joystick is
        /// pushed in that direction
        /// </summary>
        public void DisplayDirectionUI() {
            float horizontal_input = Input.GetAxis("Horizontal");
            float vertical_input = Input.GetAxis("Vertical");
            
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenRight, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottomRight, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottom, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottomLeft, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenLeft, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTopLeft, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTop, enabled:false);
            UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTopRight, enabled:false);
            
            if (horizontal_input > .5 & Math.Abs(vertical_input) < .2) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenRight);
            else if (horizontal_input > .5 & vertical_input < -.5)  UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottomRight);
            else if (Math.Abs(horizontal_input) < .2 & vertical_input < -.5) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottom);
            else if (horizontal_input < -.5 & vertical_input < -.5 )UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenBottomLeft);
            else if (horizontal_input < -.5 & Math.Abs(vertical_input) < .2) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenLeft);
            else if (horizontal_input < -.5 & vertical_input > .5) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTopLeft);
            else if (Math.Abs(horizontal_input) < .2 & vertical_input > .5) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTop);
            else if (horizontal_input > .5 & vertical_input > .5) UI_Handler.Instance.EnableComponentUI(sxr.UI_Position.PartialScreenTopRight); }

        // Singleton initiated on Awake()
        public static JoystickHandler Instance; 
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
