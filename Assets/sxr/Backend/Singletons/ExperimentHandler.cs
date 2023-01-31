﻿using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace sxr_internal {
    public class ExperimentHandler : MonoBehaviour {
        public int subjectNumber;
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
        
        /// <summary>
        /// Offers a combined "Trigger" across joystick trigger, vr controller trigger, left mouse click, and keyboard spacebar
        /// </summary>
        /// <param name="frequency">Pause between returning true when trigger/spacebar/mouse is held</param>
        /// <returns>true if [frequency] seconds has passed since last trigger and trigger/space/mouse is pressed</returns>
        public bool GetTrigger(float frequency){
            if((sxr.CheckController( sxr.ControllerButton.Trigger) || 
                Input.GetKey(KeyCode.Space) || Input.GetAxis("Fire1")>0 || Input.GetMouseButton((int) MouseButton.Left))
               && Time.time-lastTriggerPress > frequency) {
                sxr.DebugLog("Valid trigger press detected: " + Time.time); 
                lastTriggerPress = Time.time;
                return true; }
            return false; }


        public void StartTimer(float duration) {
            if(trialTimer==null) trialTimer = new Timer("TRIAL_TIMER", duration);
            else {
                Debug.LogWarning("Experiment timer restarted with 'StartTimer' but already initialized. Overwriting previous timer");
                trialTimer.Restart(); } }

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
        public void StartExperiment(string experimentName, int subjectNumber) {
            this.experimentName = experimentName;
            this.subjectNumber = subjectNumber; 
            ParseFileNames();
            phase = phase == 0 ? 1 : phase;
        }

        /// <summary>
         /// Parses file names from specified directory/subject number
         /// </summary>
        private void ParseFileNames() {
            sxrSettings.Instance.subjectDataDirectory = sxrSettings.Instance.subjectDataDirectory == "" ? 
                Application.dataPath + Path.DirectorySeparatorChar + "Experiments" + Path.DirectorySeparatorChar + 
                experimentName + Path.DirectorySeparatorChar : sxrSettings.Instance.subjectDataDirectory;

            subjectFile = sxrSettings.Instance.subjectDataDirectory +  DateTime.Today.Date.Month + "_" + DateTime.Today.Date.Day +  
                          "_" + subjectNumber;
            backupFile = sxrSettings.Instance.backupDataDirectory != ""
                ? sxrSettings.Instance.backupDataDirectory + DateTime.Today.Date.Month + "_"  + DateTime.Today.Date.Day + "_" 
                  + subjectNumber
                : ""; }

        public void WriteHeaderToTaggedFile(string tag, string headerInfo) {
            headerInfo = "SubjectNumber,Time,Phase,BlockNumber,TrialNumber,Step,TrialTime," + headerInfo;
            fh.AppendLine(subjectFile + "_" + tag + ".csv", headerInfo);
            if (backupFile != "") fh.AppendLine(backupFile + "_" + tag + ".csv", headerInfo); }
        
        public void WriteToTaggedFile(string tag, string toWrite) {
            toWrite = subjectNumber + "," + Time.time + "," + phase + "," + block + "," + trial + "," 
                      + stepInTrial + "," + trialTimer.GetTimePassed() + "," + toWrite;
            fh.AppendLine(subjectFile + "_" + tag + ".csv", toWrite);
            if (backupFile != "") fh.AppendLine(backupFile + "_" + tag + ".csv", toWrite); }

        
        // Singleton initiated on Awake()
        public static ExperimentHandler Instance;
        void Awake() {
            if ( Instance == null) {Instance = this;  DontDestroyOnLoad(gameObject.transform.root);}
            else Destroy(gameObject); }
    }    
}
