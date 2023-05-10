using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using Debug = UnityEngine.Debug;

namespace sxr_internal
{
    
    /// <summary>
    /// In development, can be used to replay camera_tracker files and display gaze
    /// </summary>
    public class ReplayMode : MonoBehaviour
    {
        public int numTrials = 50;
        public int numBlocks = 5;
        public int numPhases = 3;
        public int startPhase = 2; 
        
        //TODO 'Action Recorder + Replay', record controller buttons and what frames
        private bool replayMode;
        private DataFrame replayData; 

        private float[] timeStampArray;
        private Vector3[] positionArray;
        private Vector3[] rotationArray;
        private Vector3[] gazeArray;
        private int[] phaseArray;
        private int[] blockArray;
        private int[] trialArray;
        private bool usingGaze;
        private bool autoContinue;

        private Plane m_plane;
        private Vector3 centerPoint;
        public GameObject hitSphere;
        private Collider objCollider; 
        
        private List<string> fileList;

        public int currentFile = 0; 
        public int currentPhase;
        public int currentBlock;
        public int currentTrial;

        public int currentTimeStamp;
        public float replayTimer;

        public void SetAutoContinue(bool autoContinue)
        { this.autoContinue = autoContinue;}



        public void StartReplays(List<string> fileList)
        {
            hitSphere = sxr.GetObject("hitSphere");
            Debug.Log(fileList.Count); 
            this.fileList = fileList; 
            StartReplay(this.fileList[currentFile], phase: startPhase, autoContinue: true); 
        }
        
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
            float[] gazeArrayZ = new float[] {}; 
            if (replayData.ColumnExists("gazeScreenPosX")) {
                usingGaze = true; 
                gazeArrayX = Array.ConvertAll(replayData.GetColumnByName("WorldGazeX"),float.Parse);
                gazeArrayY = Array.ConvertAll(replayData.GetColumnByName("WorldGazeY"),float.Parse); 
                gazeArrayZ = Array.ConvertAll(replayData.GetColumnByName("WorldGazeZ"),float.Parse); 
            }

            positionArray = new Vector3[timeStampArray.Length];
            rotationArray = new Vector3[timeStampArray.Length];
            gazeArray = new Vector3[timeStampArray.Length];
            for (int i = 0; i < timeStampArray.Length; i++) {
                positionArray[i] = new Vector3(xPosArray[i], yPosArray[i], zPosArray[i]);
                rotationArray[i] = new Vector3(xRotArray[i], yRotArray[i], zRotArray[i]); 
                if (usingGaze)
                    gazeArray[i] = new Vector3(gazeArrayX[i], gazeArrayY[i], gazeArrayZ[i]); }
            Debug.Log(phaseArray.Length);
            Debug.Log(blockArray.Length);
            currentPhase = phase==0 ? phaseArray[0] : phase;
            currentBlock = block==0 ? blockArray[0] : block;
            currentTrial = trial==0 ? trialArray[0] : trial;
            this.autoContinue = autoContinue;
            
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                autoContinue = !autoContinue;
                replayTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentPhase--;
                else currentPhase++;
                replayTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentBlock--;
                else currentBlock++;
                replayTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    currentTrial--;
                else currentTrial++;
                replayTimer = 0;
            }

            if (replayTimer == 0)
                LoadNextMatch();


            if (phaseArray.Length > currentTimeStamp+1 && currentPhase == phaseArray[currentTimeStamp + 1]
                && currentBlock == blockArray[currentTimeStamp + 1]
                && currentTrial == trialArray[currentTimeStamp + 1])
            {
                currentTimeStamp += replayTimer >= timeStampArray[currentTimeStamp + 1] ? 1 : 0;
                if (replayMode)
                {
                    float nextTimePercent = (replayTimer - timeStampArray[currentTimeStamp]) /
                                            (timeStampArray[currentTimeStamp + 1] - timeStampArray[currentTimeStamp]);

                    sxrSettings.Instance.vrCamera.transform.position =
                        Vector3.Lerp(positionArray[currentTimeStamp], positionArray[currentTimeStamp + 1],
                            nextTimePercent);
                   
                    sxrSettings.Instance.vrCamera.transform.rotation = Quaternion.Euler(
                        Vector3.Lerp(rotationArray[currentTimeStamp], rotationArray[currentTimeStamp + 1],
                            nextTimePercent));

                    var currGaze =
                        Vector3.Lerp(gazeArray[currentTimeStamp], gazeArray[currentTimeStamp + 1],
                            nextTimePercent);

                    Ray ray = new Ray(sxrSettings.Instance.vrCamera.transform.position,
                        currGaze); // sxrSettings.Instance.vrCamera.ScreenPointToRay(currGaze);
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                    float intersectDist = 0f;
                    Vector3 hitPoint = new Vector3(); 
                    if (objCollider.bounds.IntersectRay(ray, out intersectDist))
                    {
                        hitPoint = ray.GetPoint(intersectDist);
                        new FileHandler().AppendLine(fileList[currentFile].Replace("camera_tracker", "gazePos"),
                            (hitPoint - centerPoint).ToString().Replace("(", "").Replace(")", "")); 
                    }

                    hitSphere.transform.position = hitPoint; 

                    replayTimer += Time.deltaTime;
                }

            }
            else
            {
                if (autoContinue)
                {

                    currentTrial++;
                    if (currentTrial >= numTrials)
                    {
                        currentTrial = 0;
                        currentBlock++;
                    }

                    if (currentBlock == numBlocks)
                    {
                        currentBlock = 0;
                        currentTrial = 0; 
                        currentPhase++;
                    }

                    if (currentPhase == numPhases)
                    {
                        currentFile++;
                        StartReplay(fileList[currentFile], phase: startPhase, autoContinue: true);
                    }
                }

                LoadNextMatch();
            }
        }

        private Vector3Int lastLoad = Vector3Int.zero;
        private void LoadNextMatch() {
            replayTimer = 0; 
            replayMode = true;
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
                } }

        }
        
    }
}

