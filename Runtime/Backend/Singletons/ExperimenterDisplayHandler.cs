using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace sxr_internal {
    public class ExperimenterDisplayHandler : MonoBehaviour
    {
        
        public bool defaultDisplayTexts = true;

        private ExperimentHandler eh;
        public TMP_InputField subjID, phaseNum, blockNum, trialNum;
        public TextMeshProUGUI displayText1, displayText2, displayText3, displayText4, displayText5;

        private bool autoplayReplays; 

        /// <summary>
        /// Sets the experiment to specified subject/phase/block/trial
        /// Automatically parses experiment name from the project name
        /// </summary>
        public void StartButton()
        {
            eh.subjectID = subjID.text == "" ? "0" : subjID.text;
            Debug.Log("Using subject: ");
            if (!Int32.TryParse(phaseNum.text, out eh.phase))
                Debug.Log("Failed to parse phase number, using phase = 0");
            if (!Int32.TryParse(blockNum.text, out eh.block))
                Debug.Log("Failed to block phase number, using block = 0");
            if (!Int32.TryParse(trialNum.text, out eh.trial))
                Debug.Log("Fai" +
                          "" +
                          "led to parse trial number, using trial = 0");
            ExperimentHandler.Instance.StartExperiment(
                Application.dataPath.Split("/")[Application.dataPath.Split("/").Length-2], 
                eh.subjectID);
            UI_Handler.Instance.HideInputUI();
            sxr.GetObject("StartScreen").SetActive(false); }

        /// <summary>
        ///  In development, can be used with ReplayMode singleton to replay camera_tracker files
        /// </summary>
        public void ReplayButton()
        {
            StartButton();
            var replayMode = sxr.GetObject("vrCamera").AddComponent<ReplayMode>();
            string[] files = Directory.GetFiles(sxrSettings.Instance.subjectDataDirectory);
            Debug.Log("FILES in "+sxrSettings.Instance.subjectDataDirectory);
            List<string>cameraFiles = new List<string>() ; 
        
            foreach (var f in files)
            {
                if (subjID.text != "")
                {
                    var new_f = f.Substring(sxrSettings.Instance.subjectDataDirectory.Length);
                    Debug.Log(new_f);
                    Debug.Log(new_f.Split("_")[4]);
                    if (new_f.Split("_")[4] == subjID.text && f.Contains("camera_tracker") &&
                        !f.Contains(".meta")) {
                        replayMode.StartReplay(f, phase: eh.phase, block: eh.block, trial: eh.trial);
                        break; }
                }
                else
                {
                    if(f.Contains("camera_tracker"))
                        cameraFiles.Add(f); 
                }
            }

            if (subjID.text == "")
                replayMode.StartReplays(cameraFiles); 

        }

        private void Update() {
            if (defaultDisplayTexts && ExperimentHandler.Instance.phase > 0) {
                displayText1.text = "'Textbox1' [Phase] - Block: trial(step)  =  [" + eh.phase + "] - " + eh.block + ": " 
                                    + eh.trial +"(" + eh.stepInTrial + ")";
                displayText1.enabled = true; 
                
                displayText2.text = "'Textbox2' Player Position: " + sxrSettings.Instance.vrCamera.transform.position;
                displayText2.enabled = true;

                displayText3.text = "'Textbox3' Trial Timer: " + sxr.TimeRemaining();
                
            } }

        public void ChangeTextbox(int whichBox, string text) {
            if (whichBox > 0 && whichBox < 6) {
                var boxes = new[] {displayText1, displayText2, displayText3, displayText4, displayText5};
                boxes[whichBox - 1].text = text; }
            else
                Debug.LogWarning("Textbox objects on experimenter screen are numbered 1-5"); }
        
        private void Start() {
            if (subjID == null) subjID = sxr.GetObject("SubjectNumber").GetComponentInChildren<TMP_InputField>();
            if (phaseNum == null) phaseNum = sxr.GetObject("Phase").GetComponentInChildren<TMP_InputField>();
            if (blockNum == null) blockNum = sxr.GetObject("Block").GetComponentInChildren<TMP_InputField>();
            if (trialNum == null) trialNum = sxr.GetObject("Trial").GetComponentInChildren<TMP_InputField>();
            if (!displayText1) displayText1 = sxr.GetObject("DisplayText1").GetComponent<TextMeshProUGUI>();
            if (!displayText2) displayText2 = sxr.GetObject("DisplayText2").GetComponent<TextMeshProUGUI>();
            if (!displayText3) displayText3 = sxr.GetObject("DisplayText3").GetComponent<TextMeshProUGUI>();
            if (!displayText4) displayText4 = sxr.GetObject("DisplayText4").GetComponent<TextMeshProUGUI>();
            if (!displayText5) displayText5 = sxr.GetObject("DisplayText5").GetComponent<TextMeshProUGUI>();
            eh = ExperimentHandler.Instance;
            
            displayText4.text = "'Textbox4'";
            displayText5.text = "'Textbox5'"; 
            
            #if SXR_USE_STARTSCREEN
            #else
            StartButton();
            UI_Handler.Instance.UI_Submit();
            #endif
        }
        
        
        // Singleton initiated on Awake()
        public static ExperimenterDisplayHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }
}
