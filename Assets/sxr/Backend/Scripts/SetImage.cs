using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Automatically sets a UI RawImage object's texture to the image in sxr/Resources/GUI_Images/ with the same name
/// </summary>
public class SetImage : MonoBehaviour {
    
    void Start()
    { GetComponent<RawImage>().texture = Resources.Load<Texture2D>("GUI_Images/" + name); }
}
