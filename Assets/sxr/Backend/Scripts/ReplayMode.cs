using System;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using Debug = UnityEngine.Debug;

namespace sxr_internal
{
    public class ReplayMode : MonoBehaviour
    {
        public int downscaleFactor = 5; // if downscale was used, must change this value
        
        //TODO 'Action Recorder + Replay', record controller buttons and what frames
        private bool replayMode;
        private DataFrame replayData; 

        private float[] timeStampArray;
        private Vector3[] positionArray;
        private Vector3[] rotationArray;
        private Vector2[] gazeArray;
        private int[] phaseArray;
        private int[] blockArray;
        private int[] trialArray;
        private bool usingGaze;
        private bool autoContinue; 

        public int currentPhase;
        public int currentBlock;
        public int currentTrial;

        public int currentTimeStamp;
        public float replayTimer;

        public void SetAutoContinue(bool autoContinue)
        { this.autoContinue = autoContinue;}

        /// <summary>
        /// Replays the input cameraTracker file at the specified phase/block/trial.
        /// Default values are 0, overwrite with variable:#### (e.g. phase:1, block:1, trial:1)
        /// </summary>
        /// <param name="replayFile"></param>
        /// <param name="phase"></param>
        /// <param name="block"></param>
        /// <param name="trial"></param>
        public void StartReplay(string replayFile, int phase=0, int block=0, int trial=0, bool autoContinue=false)
        {
            sxrSettings.Instance.vrCamera.gameObject.GetComponent<AutomaticDesktopVsVR>().enabled = false; 
            sxrSettings.Instance.vrCamera.gameObject.GetComponent<SimpleFirstPersonMovement>().enabled = false; 
            Debug.Log("Start Replay for "+replayFile);
            replayMode = true;
            replayData = new DataFrame().LoadFromCSV(replayFile);
            //replayData.PrintDataFrame();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            timeStampArray = Array.ConvertAll(replayData.GetColumnByName("TrialTime"), float.Parse);
            phaseArray = Array.ConvertAll(replayData.GetColumnByName("Phase"), Int32.Parse);
            blockArray = Array.ConvertAll(replayData.GetColumnByName("BlockNumber"), Int32.Parse);
            trialArray = Array.ConvertAll(replayData.GetColumnByName("TrialNumber"), Int32.Parse);

            float[] xPosArray = Array.ConvertAll(replayData.GetColumnByName("xPos"),float.Parse);
            float[] yPosArray = Array.ConvertAll(replayData.GetColumnByName("yPos"),float.Parse);
            float[] zPosArray = Array.ConvertAll(replayData.GetColumnByName("zPos"),float.Parse);
            float[] xRotArray = Array.ConvertAll(replayData.GetColumnByName("xRot"),float.Parse);
            float[] yRotArray = Array.ConvertAll(replayData.GetColumnByName("yRot"),float.Parse);
            float[] zRotArray = Array.ConvertAll(replayData.GetColumnByName("zRot"),float.Parse);
            float[] gazeArrayX = new float[]{};
            float[] gazeArrayY = new float[] {}; 
            if (replayData.ColumnExists("gazeScreenPosX")) {
                usingGaze = true; 
                gazeArrayX = Array.ConvertAll(replayData.GetColumnByName("gazeScreenPosX"),float.Parse);
                gazeArrayY = Array.ConvertAll(replayData.GetColumnByName("gazeScreenPosY"),float.Parse); }

            positionArray = new Vector3[timeStampArray.Length];
            rotationArray = new Vector3[timeStampArray.Length];
            gazeArray = new Vector2[timeStampArray.Length];
            for (int i = 0; i < timeStampArray.Length; i++) {
                positionArray[i] = new Vector3(xPosArray[i], yPosArray[i], zPosArray[i]);
                rotationArray[i] = new Vector3(xRotArray[i], yRotArray[i], zRotArray[i]); 
                if (usingGaze)
                    gazeArray[i] = new Vector2(gazeArrayX[i], gazeArrayY[i]); }
            Debug.Log(phaseArray.Length);
            Debug.Log(blockArray.Length);
            currentPhase = phase==0 ? phaseArray[0] : phase;
            currentBlock = block==0 ? blockArray[0] : block;
            currentTrial = trial==0 ? trialArray[0] : trial;
            this.autoContinue = autoContinue;
            string printStuff = "";
            foreach (var thing in rotationArray)
                printStuff += "[ " + thing.ToString() + " ]";
            Debug.Log(printStuff); 
        }
        
        
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentPhase--;
                else currentPhase++;
                replayTimer = 0; }
            
            if(Input.GetKeyDown(KeyCode.B)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentBlock--;
                else currentBlock++;
                replayTimer = 0; }
            
            if(Input.GetKeyDown(KeyCode.T)) {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentTrial--;
                else currentTrial++;
                replayTimer = 0; }
            
            currentTimeStamp += replayTimer >= timeStampArray[currentTimeStamp + 1] ? 1 : 0;
            if (currentPhase == phaseArray[currentTimeStamp]
                && currentBlock == blockArray[currentTimeStamp]
                && currentTrial == trialArray[currentTimeStamp])
            {
                if (replayMode)
                {
                    float nextTimePercent = (replayTimer - timeStampArray[currentTimeStamp]) /
                                            (replayTimer - timeStampArray[currentTimeStamp + 1]);

                    sxrSettings.Instance.vrCamera.transform.position =
                        Vector3.Lerp(positionArray[currentTimeStamp], positionArray[currentTimeStamp + 1],
                            nextTimePercent);
                    sxrSettings.Instance.vrCamera.transform.rotation = Quaternion.Euler(
                        Vector3.Lerp(rotationArray[currentTimeStamp], rotationArray[currentTimeStamp + 1],
                            nextTimePercent));

                    var currGaze =
                        Vector3.Lerp(new Vector3(gazeArray[currentTimeStamp].x, gazeArray[currentTimeStamp].y, 0),
                            new Vector3(gazeArray[currentTimeStamp + 1].x, gazeArray[currentTimeStamp + 1].y, 0),
                            nextTimePercent);
                    currGaze.x *= Screen.width/downscaleFactor;
                    currGaze.y *= Screen.height/downscaleFactor;

                    Ray ray = sxrSettings.Instance.vrCamera.ScreenPointToRay(currGaze);
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow); 

                    replayTimer += Time.deltaTime;
                }
                else
                    replayMode = sxr.GetTrigger() || autoContinue;
            }
            else LoadNextMatch(); 
        }

        private Vector3Int lastLoad = Vector3Int.zero;
        private void LoadNextMatch() {
            if (lastLoad.x != currentPhase || lastLoad.y != currentBlock || lastLoad.z != currentTrial) {
                lastLoad = new Vector3Int(currentPhase, currentBlock, currentTrial);
                int startFrame = currentTimeStamp;
                while (true) {
                    
                    if (phaseArray[currentTimeStamp] == currentPhase && blockArray[currentTimeStamp] == currentBlock &&
                        trialArray[currentTimeStamp] == currentTrial)
                        return; 
                    
                    currentTimeStamp++; 
                    if (currentTimeStamp == phaseArray.Length)
                        currentTimeStamp = 0; 
                    
                    if (currentTimeStamp == startFrame) {
                        Debug.Log("Could not find any matching results for: "+currentPhase +", "+currentBlock+", "+currentTrial);
                        return;}
                } } }
        
    }
}

