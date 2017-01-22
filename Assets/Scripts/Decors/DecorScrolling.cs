using UnityEngine;
using System.Collections;

public class DecorScrolling : MonoBehaviour
{

    public float speed = -3f;
	public float scale = 1f;

    void Update()
    {
        transform.Translate(speed * scale * Time.deltaTime, 0f, 0f);
    }
}
