using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// ObjectMotion class is used to track objects that are currently moving to a destination
    /// On each update, each instance of ObjectMotion (tracked in SceneObjects.Instance.motionObjects)
    /// will call UpdatePositionAndDispose.  If destination is reached(within 0.01m), True is returned
    /// and the ObjectMotion instance is cleared from memory. 
    /// </summary>
    public class ObjectResize {
        private GameObject gameObj;
        private Vector3 newSize;
        private Vector3 startSize; 
        private float arrivalTime;
        private float timeTaken; 

        public ObjectResize(GameObject gameObj, Vector3 newSize, float timeTaken) {
            this.gameObj = gameObj;
            this.newSize = newSize;
            this.arrivalTime = Time.time + timeTaken;
            this.timeTaken = timeTaken; 
            startSize = gameObj.transform.localScale; }

        public GameObject GetGameObject() { return gameObj; }
        
        /// <summary>
        /// Moves object closer to target location, ending at the destination after
        /// the specified amount of time.  Returns true when movement is finished
        /// so SceneObjectHandler can clear ObjectMotion from memory
        /// </summary>
        /// <returns>True once allotted time has passed</returns>
        public bool UpdateSizeDisposeAtTargetSize()
        {
            Vector3 diff = (newSize - startSize) * Time.deltaTime/timeTaken;

            if (Time.time >= arrivalTime)
            {
                gameObj.transform.localScale = newSize;
                return true; }
            gameObj.transform.localScale += diff;
            return false ; }
    }
}