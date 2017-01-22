using UnityEngine;
using System.Collections;

public class CameraCollider : MonoBehaviour {
    
	void Awake () {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height, Camera.main.orthographicSize * 2.0f);
    }
}
