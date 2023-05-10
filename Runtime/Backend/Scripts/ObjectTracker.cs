using sxr_internal;
using UnityEngine;

namespace sxr_internal{
    public class ObjectTracker : MonoBehaviour
    {
        public bool trackerActive=true;
        private bool headerPrinted; 
        private string toWrite = "";



        public void StartTracker() {
            if (!headerPrinted) {
                sxr.WriteHeaderToTaggedFile(gameObject.name+"_tracker", "PosX,PosY,PosZ,RotX,RotY,RotZ");
                headerPrinted = true; }
            trackerActive = true; }

        public void StopTracker() {
            if(toWrite != "") ExperimentHandler.Instance.WriteToTaggedFile(gameObject.name+"_tracker", toWrite, includeTimeStepInfo:false);
            toWrite = ""; }
     
        void Update() {
            if (sxrSettings.Instance.RecordThisFrame() && trackerActive) {
                var trans = transform; 
                var pos = trans.position;
                var rot = trans.rotation; 
                toWrite +=  ExperimentHandler.Instance.timeStepToWriteInfo() + pos.x + "," +
                            pos.y + "," +
                            pos.z + "," +
                            rot.eulerAngles.x + "," + 
                            rot.eulerAngles.y + "," + 
                            rot.eulerAngles.z + "\n"; } }
        
        private void OnApplicationQuit(){
            if(headerPrinted && toWrite != "")
                ExperimentHandler.Instance.WriteToTaggedFile(gameObject.name+"_tracker", toWrite, includeTimeStepInfo:false);}

    }
}