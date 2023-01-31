using UnityEngine;

namespace sxr_internal {
    /// <summary>
    /// Attach to any object that will not be disabled... Can use keyboard to switch listed objects on/off
    /// </summary>
    public class EnableByKeyboard : MonoBehaviour {
        [SerializeField] private KeyCode keycode = KeyCode.Space;
        [SerializeField] private GameObject[] objectsToSwitchEnabled;

        void Update() {
            if(Input.GetKeyDown(keycode))
                foreach (var gameObj in objectsToSwitchEnabled)
                    gameObj.SetActive(!gameObj.activeSelf); }
    }
}