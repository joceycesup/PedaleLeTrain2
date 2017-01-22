using UnityEngine;
using System.Collections;

public class DecorOffScreen : MonoBehaviour {
    public PoleSpawner ps;

	private void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Pole") {
			ps.PoleDisappeared (other.gameObject);
		} else if (other.tag == "Collector") {
			Destroy (other.gameObject);
		} else if (other.tag == "Decor") {
			other.gameObject.SetActive (false);
		}
	}
}
