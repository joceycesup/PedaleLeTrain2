using UnityEngine;
using System.Collections;

public class TextScale : MonoBehaviour {

    public float baseWidth = 1920.00f;
    public float baseHeight = 1080.00f;

    // Use this for initialization
    void Start()
    {
        float scaleX = (float)(Screen.width) / baseWidth;
        float scaleY = (float)(Screen.height) / baseHeight;
        GUIText[] texts = FindObjectsOfType<GUIText>();
        foreach (GUIText myText in texts) {
            Vector2 pixOff = myText.pixelOffset;
            int originSizeText = myText.fontSize;
            myText.pixelOffset = new Vector2(pixOff.x * scaleX, pixOff.y * scaleY);
            myText.fontSize = (int)(originSizeText * scaleX);
        }
    }
}
