using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// [Singleton] Tracks all objects in the scene to allow for easy sxr.GetObject(name) calls.
    /// Also tracks all ObjectMotion instances until they have reached their destination,
    /// at which point they are cleared from memory. 
    /// </summary>
    public class SceneObjectsHandler : MonoBehaviour {
        
        private GameObject[] allObjects;
        private List<ObjectMotion> motionObjects = new List<ObjectMotion>();
        private List<ObjectResize> resizeObjects = new List<ObjectResize>(); 

        /// <summary>
        /// Returns first game object in hierarchy matching the specified name
        /// </summary>
        /// <param name="name">Name of object to find</param>
        /// <returns>GameObject if found, null otherwise</returns>
        public GameObject GetObjectByName(string name) {
            allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var obj in allObjects)
                if (obj.name == name) return obj;
            Debug.LogWarning("Search for object: \'"+name+"\' unsuccessful, check spelling and whitespace");
            Debug.LogWarning("Available objects: "+allObjects.ToArray().ToCommaSeparatedString());
            return null; }

        public bool ObjectExists(string name)
        {
            allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var obj in allObjects)
                if (obj.name == name) return true;
            return false; 
        }
        
        public void DebugListAllObjects(){ foreach(var obj in allObjects) Debug.Log(obj.name);}

        public void AddMotionObject(ObjectMotion objectMotion, bool cancelPrevious) {
            if (!CheckForObjectInMotion(objectMotion.GetGameObject()) || cancelPrevious) 
                motionObjects.Add(objectMotion);
            else
                Debug.Log("Object already in motion, set cancelPrevious=true in move function to override."); }

        public bool CheckForObjectInMotion(GameObject gameObject) {
            foreach(var objMot in motionObjects)
                if (gameObject == objMot.GetGameObject())
                    return true;
            return false; }
        
        public void AddMotionResize(ObjectResize objectResize, bool cancelPrevious) {
            if (!CheckForObjectInResize(objectResize.GetGameObject()) || cancelPrevious) 
                resizeObjects.Add(objectResize);
            else
                Debug.Log("Object already in resize, set cancelPrevious=true in move function to override."); }

        public bool CheckForObjectInResize(GameObject gameObject) {
            foreach(var objResize in resizeObjects)
                if (gameObject == objResize.GetGameObject())
                    return true;
            return false; }
        
        void Update() {
            List<ObjectMotion> toRemove = new List<ObjectMotion>();
            foreach (var obj in motionObjects)
                if (obj.UpdatePositionDisposeAtTargetLocation()) toRemove.Add(obj);
            foreach (var obj in toRemove)
                motionObjects.Remove(obj);

            List<ObjectResize> toRemoveResizes = new List<ObjectResize>(); 
            foreach (var obj in resizeObjects)
                if (obj.UpdateSizeDisposeAtTargetSize()) toRemoveResizes.Add(obj);
            foreach (var obj in toRemoveResizes)
                resizeObjects.Remove(obj);
        }
        
        void Start() { allObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);}
        
        // Singleton initiated on Awake()  
        public static SceneObjectsHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }   
}
