using UnityEngine;
using System.Collections;

// class used to rescale the background for multiple resolutions
public class BGScaler : MonoBehaviour
{

    void Awake()
    {
        // calculate the height
        var ratio = transform.localScale.x / transform.localScale.y;
        var height = Camera.main.orthographicSize * 2f;

        if (gameObject.tag == "Background")
        {
            transform.localScale = new Vector3(height * ratio, height, 0);
        }
    }

}