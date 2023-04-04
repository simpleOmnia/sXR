using sxr_internal;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button, replayButton; 
    private void Start() {
        button = sxr.GetObject("StartButton").GetComponent<Button>(); 
        button.onClick.AddListener(()=>ExperimenterDisplayHandler.Instance.StartButton());

        replayButton = sxr.GetObject("ReplayButton").GetComponent<Button>();
        replayButton.onClick.AddListener(()=>ExperimenterDisplayHandler.Instance.ReplayButton());
    }
}
