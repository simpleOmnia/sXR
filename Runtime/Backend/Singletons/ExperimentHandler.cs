using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace sxr_internal {
    public class ExperimentHandler : MonoBehaviour {
        public string subjectID;
        public int phase;
        public int block;
        public int trial;
        public int stepInTrial; 
        
        private string experimentName = "";
        private string subjectFile = "";
        private string backupFile = "";

        private Timer trialTimer=null;
        private FileHandler fh = new FileHandler();

        private float lastTriggerPress;
        private int lastTrueFrame; 
        
        /// <summary>
        /// Offers a combined "Trigger" across joystick trigger, vr controller trigger, left mouse click, and keyboard spacebar
        /// </summary>
        /// <param name="frequency">Pause between returning true when trigger/spacebar/mouse is held</param>
        /// <returns>true if [frequency] seconds has passed since last trigger and trigger/space/mouse is pressed</returns>
        public bool GetTrigger(float frequency) {
            if (sxrSettings.Instance.GetCurrentFrame() == lastTrueFrame)
                return true; 
            
            if((sxr.CheckController( sxr_internal.ControllerButton.Trigger) || 
                Input.GetKey(KeyCode.Space) || Input.GetAxis("Fire1")>0 || Input.GetMouseButton((int) MouseButton.Left))
               && Time.time-lastTriggerPress > frequency) {
                sxr.DebugLog("Valid trigger press detected: " + Time.time); 
                lastTriggerPress = Time.time;
                lastTrueFrame = sxrSettings.Instance.GetCurrentFrame(); 
                return true; }
            
            return false; }


        public void StartTimer(float duration = 99999) {
            trialTimer = TimerHandler.Instance.StartTimer("TRIAL_TIMER", duration:duration);  }
        private void Start() { StartTimer(); }

        public void PauseTimer()
        { trialTimer.PauseTimer(); }

        public bool CheckTimer() { return trialTimer.GetTimePassed() > trialTimer.GetDuration();}
       
        public void RestartTimer(){trialTimer.Restart();}

        public float GetTimeRemaining() { return trialTimer != null ? trialTimer.GetTimeRemaining() : 0;}

        public float GetTimePassed() { return trialTimer.GetTimePassed(); }

        /// <summary>
        /// Sets experiment name (overrides the automatic naming when the "Start" button is pressed)
        /// </summary>
        /// <param name="name"></param>
        public void SetExperimentName(string name) { experimentName = name; ParseFileNames(); }
        
        /// <summary>
        /// Starts the experiment and parses 
        /// </summary>
        /// <param name="experimentName"></param>
        /// <param name="subjectNumber"></param>
        public void StartExperiment(string experimentName, string subjectID) {
            this.experimentName = experimentName;
            this.subjectID = subjectID; 
            ParseFileNames();
            phase = phase == 0 ? 1 : phase;
        }

        /// <summary>
         /// Parses file names from specified directory/subject number
         /// </summary>
        private void ParseFileNames()
        {
            if (experimentName == "")
                experimentName = Application.dataPath.Split("/")[Application.dataPath.Split("/").Length - 2];
            sxrSettings.Instance.subjectDataDirectory = sxrSettings.Instance.subjectDataDirectory == "" ? 
                Application.dataPath + Path.DirectorySeparatorChar + "Experiments" + Path.DirectorySeparatorChar + 
                experimentName + Path.DirectorySeparatorChar : sxrSettings.Instance.subjectDataDirectory;

            subjectFile = sxrSettings.Instance.subjectDataDirectory +  DateTime.Today.Date.Year + "_" 
                          + DateTime.Today.Date.Month + "_" + DateTime.Today.Date.Day +  
                          "_" +  DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + "_" + subjectID;
            backupFile = sxrSettings.Instance.backupDataDirectory != ""
                ? sxrSettings.Instance.backupDataDirectory + Path.DirectorySeparatorChar +  DateTime.Today.Date.Year + "_" 
                  + DateTime.Today.Date.Month + "_"  + DateTime.Today.Date.Day + "_" + DateTime.Now.Hour 
                  + DateTime.Now.Minute + "_" +subjectID : ""; 
            StartTimer(); }

        public void WriteHeaderToTaggedFile(string tag, string headerInfo) {
            if (subjectFile == "") { ParseFileNames();}
            
            headerInfo = "SubjectID,Date,LocalTime,UnityTime,Phase,BlockNumber,TrialNumber,Step,TrialTime," + headerInfo;
            fh.AppendLine(subjectFile + "_" + tag + ".csv", headerInfo);
            if (backupFile != "") fh.AppendLine(backupFile + "_" + tag + ".csv", headerInfo); }
        
        public void WriteToTaggedFile(string tag, string toWrite, bool includeTimeStepInfo=true) {
            if (subjectFile == "") { ParseFileNames();}
            
            toWrite = (includeTimeStepInfo ? timeStepToWriteInfo() : "") + toWrite;
            fh.AppendLine(subjectFile + "_" + tag + ".csv", toWrite);
            if (backupFile != "") fh.AppendLine(backupFile + "_" + tag + ".csv", toWrite); }

        public string timeStepToWriteInfo()
        {
            return subjectID + "," + DateTime.Today.Month + "_" + DateTime.Today.Day + "," + DateTime.Now.Hour + "_" +
                   DateTime.Now.Minute + "_" + DateTime.Now.Second + "," + Time.time + "," + phase + "," + block + "," +
                   trial + "," + stepInTrial + "," + trialTimer.GetTimePassed() + ",";
        }
        
        // Singleton initiated on Awake()
        public static ExperimentHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }    
}
