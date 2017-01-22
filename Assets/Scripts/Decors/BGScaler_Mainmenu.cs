using UnityEngine;
using System.Collections;

// class used to rescale the background for multiple resolutions
public class BGScaler_Mainmenu : MonoBehaviour
{

    void Start()
    {
        // calculate the height
        var height = Camera.main.orthographicSize * 2f;
        var width = height * Screen.width / Screen.height;

        if (gameObject.name == "Background")
        {
            transform.localScale = new Vector3(width, height, 0);
        }

    }

}