using System;
using System.Globalization;
using UnityEngine;

namespace sxr_internal
{
    public class ReplayMode : MonoBehaviour
    {
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

        private int currentPhase;
        private int currentBlock;
        private int currentTrial;

        private int currentTimeStamp;
        private float replayTimer; 
        

        /// <summary>
        /// Replays the input cameraTracker file at the specified phase/block/trial.
        /// Default values are 0, overwrite with variable:#### (e.g. phase:1, block:1, trial:1)
        /// </summary>
        /// <param name="replayFile"></param>
        /// <param name="phase"></param>
        /// <param name="block"></param>
        /// <param name="trial"></param>
        public void StartReplay(string replayFile, int phase=0, int block=0, int trial=0, bool autoContinue=false) {
            replayMode = true;
            replayData = new DataFrame().LoadFromCSV(replayFile);
            
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture; 
            timeStampArray = Array.ConvertAll(replayData.GetColumnByName("Time"),float.Parse);
            int[] phaseArray = Array.ConvertAll(replayData.GetColumnByName("Phase"), Int32.Parse);
            int[] blockArray = Array.ConvertAll(replayData.GetColumnByName("BlockNumber"), Int32.Parse);
            int[] trialArray = Array.ConvertAll(replayData.GetColumnByName("TrialNumber"), Int32.Parse);

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
            
            
            currentPhase = phase==0 ? phaseArray[0] : phase;
            currentBlock = block==0 ? blockArray[0] : block;
            currentTrial = trial==0 ? trialArray[0] : trial;
            this.autoContinue = autoContinue; 
        }
        
        
        private void Update() {
            currentTimeStamp += replayTimer >= timeStampArray[currentTimeStamp + 1] ? 1 : 0;
            if (replayMode && currentPhase == phaseArray[currentTimeStamp] 
                           && currentBlock == blockArray[currentTimeStamp]
                           && currentTrial == trialArray[currentTimeStamp])
            {
                float nextTimePercent = (replayTimer - timeStampArray[currentTimeStamp]) /
                                        (replayTimer - timeStampArray[currentTimeStamp + 1]);
                sxrSettings.Instance.vrCamera.transform.position =
                    Vector3.Lerp(positionArray[currentTimeStamp], positionArray[currentTimeStamp + 1], nextTimePercent);
                sxrSettings.Instance.vrCamera.transform.rotation.eulerAngles = 
                    Vector3.Lerp(rotationArray[currentTimeStamp], rotationArray[currentTimeStamp + 1], nextTimePercent);
                

                replayTimer += Time.deltaTime;
            }
            else 
                replayMode = sxr.GetTrigger() || autoContinue;
        }
    }
}