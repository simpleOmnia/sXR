using UnityEngine;

namespace sxr_internal
{
    [CreateAssetMenu(fileName = "sxrInitializedState", menuName = "Editor/sxrInitializedState", order=1)]
    public class sXR_Initialized_State : ScriptableObject
    {
        public bool hasBeenInitialized; 
    }
}