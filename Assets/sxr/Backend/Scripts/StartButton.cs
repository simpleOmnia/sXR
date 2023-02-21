using sxr_internal;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button; 
    private void Start() {
        button = sxr.GetObject("StartButton").GetComponent<Button>(); 
        button.onClick.AddListener(()=>ExperimenterDisplayHandler.Instance.StartButton()); }
}
