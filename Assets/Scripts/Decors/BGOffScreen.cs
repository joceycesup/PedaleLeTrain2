using UnityEngine;
using System.Collections;

public class BGOffScreen : MonoBehaviour {

	private void OnTriggerExit2D (Collider2D target) {
		if (target.tag == "Collector") {
			gameObject.SetActive (false);
		}
	}
}
