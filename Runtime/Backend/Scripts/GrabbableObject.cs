using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Turns the object this component is attached to into a grabbable object that can be moved
    /// by pressing the VR trigger while touching the object
    /// </summary>
    public class GrabbableObject : MonoBehaviour {
        bool grabbedLastFrame = false; 
        Vector3 lastControllerPos = new Vector3(); 
        bool usesGravity;
        private GameObject leftController, rightController; 
        
        void Update() {
            bool isCurrentlyGrabbed = false;
            
            GameObject[] controllers = new[]
                {rightController, leftController};
            
            foreach(var controller in controllers)
                if (controller!=null && controller.activeSelf && CollisionHandler.Instance.ObjectsCollidersTouching(this.gameObject, controller) 
                    && sxr.GetTrigger(0)) {
                    GetComponent<Rigidbody>().useGravity = false;

                    if (grabbedLastFrame)
                        transform.position += controller.transform.position - lastControllerPos;
                    lastControllerPos = controller.transform.position; 
                    isCurrentlyGrabbed = true;
                    break; }

            grabbedLastFrame = isCurrentlyGrabbed;
            GetComponent<Rigidbody>().useGravity = !isCurrentlyGrabbed && usesGravity; }

        void Start() {
            if (GetComponent<Rigidbody>() == null)
                gameObject.AddComponent<Rigidbody>();

            usesGravity = GetComponent<Rigidbody>().useGravity;

            rightController = sxr.GetObject("RightControllerCapsule");
            leftController = sxr.GetObject("LeftControllerCapsule"); } 
    }
}