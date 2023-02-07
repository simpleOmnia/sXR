using System.Collections;
using System.Collections.Generic;
using sxr_internal;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    public bool trackerActive=true; 
    void Update()
    {
        if (sxrSettings.Instance.RecordThisFrame() && trackerActive)
        {
            ExperimentHandler.Instance.WriteToTaggedFile(gameObject.name + "_tracker",
                transform.position.x +","+ 
                transform.position.y +","+ 
                transform.position.z +","+ 
                transform.rotation.eulerAngles.x +","+
                transform.rotation.eulerAngles.y +","+
                transform.rotation.eulerAngles.z); 
        }
    }
}
