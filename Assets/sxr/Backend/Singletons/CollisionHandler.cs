using UnityEngine;

namespace sxr_internal {
    public class CollisionHandler : MonoBehaviour {
        public bool ObjectWithinDistance(GameObject obj1, GameObject obj2, float distance) {
            return Vector3.Distance(obj1.transform.position, obj2.transform.position) < distance; }

        public bool ObjectsCollidersTouching(GameObject obj1, GameObject obj2)
        {
            bool triggerObj = false;
            foreach (var obj in new[] {obj1, obj2}) {
                if (obj.GetComponents<Collider>().Length == 0 && obj.GetComponentsInChildren<Collider>().Length == 0)
                    obj.AddComponent<MeshCollider>();
                
                if (obj.GetComponents<Rigidbody>().Length == 0 && obj.GetComponentsInChildren<Rigidbody>().Length == 0){
                    obj.AddComponent<Rigidbody>();
                    obj.GetComponent<Rigidbody>().isKinematic = true; }

                if (obj.GetComponents<CollisionDetector>().Length == 0 && obj.GetComponentsInChildren<CollisionDetector>().Length == 0)
                    obj.AddComponent<CollisionDetector>();

                if (triggerObj || (obj.GetComponents<Collider>().Length == 0
                    ? obj.GetComponentsInChildren<Collider>()[0]
                    : obj.GetComponent<Collider>()).isTrigger)
                    triggerObj = true; }

            if (!triggerObj) // if neither object has a trigger, adds a trigger to the first object
                (obj1.GetComponents<Collider>().Length == 0
                    ? obj1.GetComponentsInChildren<Collider>()[0]
                    : obj1.GetComponent<Collider>()).isTrigger = true; 


            var obj1_collisions = obj1.GetComponents<CollisionDetector>().Length > 0
                ? obj1.GetComponents<CollisionDetector>()[0]
                : obj1.GetComponentsInChildren<CollisionDetector>()[0];
            
            if (obj1_collisions.objectsInContact.Contains(obj2))
                return true;

            return false; }

        // Singleton initiated on Awake()
        public static CollisionHandler Instance;
        private void Awake() {
            if (Instance == null) {Instance = this; DontDestroyOnLoad(gameObject.transform.root); }
            else Destroy(gameObject); }
    }
}
