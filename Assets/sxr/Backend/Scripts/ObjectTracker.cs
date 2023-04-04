using sxr_internal;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    public bool trackerActive=true;
    private bool headerWritten; 
    private string toWrite;



    public void StartTracker() {
        if (!headerWritten) {
            sxr.WriteHeaderToTaggedFile(gameObject.name+"_tracker", "PosX,PosY,PosZ,RotX,RotY,RotZ");
            headerWritten = true; }
        trackerActive = true; }

    public void StopTracker() {
        ExperimentHandler.Instance.WriteToTaggedFile(gameObject.name+"_tracker", toWrite);
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
        if(toWrite != "")
            ExperimentHandler.Instance.WriteToTaggedFile(gameObject.name+"_tracker", toWrite);}

}