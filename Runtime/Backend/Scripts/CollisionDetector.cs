using System.Collections.Generic;
using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Keeps a list of all objects in contact with the GameObject with
    /// this component attached
    /// </summary>
    public class CollisionDetector : MonoBehaviour {
        public List<GameObject> objectsInContact = new List<GameObject>();
        
        void OnCollisionEnter(Collision other) {Debug.Log(other.gameObject.name); objectsInContact.Add(other.gameObject); }
        private void OnCollisionExit(Collision other) { if(objectsInContact.Contains(other.gameObject)) objectsInContact.Remove(other.gameObject); }
        void OnTriggerEnter(Collider other) { objectsInContact.Add(other.gameObject); }
        void OnTriggerExit(Collider other) {if(objectsInContact.Contains(other.gameObject)) objectsInContact.Remove(other.gameObject); }

        public bool CheckIfStillColliding(GameObject obj2)
        {
            if (gameObject.GetComponent<Collider>().bounds.Intersects(obj2.GetComponent<Collider>().bounds))
                return true;
            objectsInContact.Remove(obj2);
            return false; 
        }
    }
}