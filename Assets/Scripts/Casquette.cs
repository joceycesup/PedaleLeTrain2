using UnityEngine;
using System.Collections;

public class Casquette : MonoBehaviour {
	private float turnRate;

	void Start () {
		turnRate = Random.Range (-360f, 360f);
	}

	void Update () {
		transform.Rotate (Vector3.forward, turnRate*Time.deltaTime);
	}

	void OnDestroy () {
		if (transform.parent.childCount <= 1)
			Destroy (transform.parent.gameObject);
	}
}
