using Unity.VisualScripting;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// ObjectMotion class is used to track objects that are currently moving to a destination
    /// On each update, each instance of ObjectMotion (tracked in SceneObjects.Instance.motionObjects)
    /// will call UpdatePositionAndDispose.  If destination is reached(within 0.01m), True is returned
    /// and the ObjectMotion instance is cleared from memory. 
    /// </summary>
    public class ObjectMotion {
        private GameObject gameObj;
        private Vector3 objectDestination;
        private Vector3 initialPosition; 
        private float arrivalTime;
        private float timeTaken; 

        public ObjectMotion(GameObject gameObj, Vector3 objectDestination, float timeTaken) {
            this.gameObj = gameObj;
            this.objectDestination = objectDestination;
            this.arrivalTime = Time.time + timeTaken;
            this.timeTaken = timeTaken; 
            initialPosition = gameObj.transform.position; }

        public GameObject GetGameObject() { return gameObj; }
        
        /// <summary>
        /// Moves object closer to target location, ending at the destination after
        /// the specified amount of time.  Returns true when movement is finished
        /// so SceneObjectHandler can clear ObjectMotion from memory
        /// </summary>
        /// <returns>True once allotted time has passed</returns>
        public bool UpdatePositionDisposeAtTargetLocation() {
            if (Time.time >= arrivalTime) {
                gameObj.transform.position = objectDestination;
                return true; }
            gameObj.transform.position = (objectDestination - initialPosition) * Time.deltaTime / timeTaken;
            return false ; }
    }
}