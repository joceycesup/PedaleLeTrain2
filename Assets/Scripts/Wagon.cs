using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wagon : MonoBehaviour {
	public float driftSpeed = -4f;
	public List<AudioClip> clips;
	private AudioSource audio;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
		clips = new List<AudioClip>();
		for (int i = 1; ; i++)
		{
			var clip = Resources.Load("Audio/Finals/Shaker_" + i);
			if (clip == null)
				break;
			clips.Add(clip as AudioClip);
		}
	}

	public void Detach () {
		transform.parent = null;
		StartCoroutine ("GoAway");
	}

	IEnumerator GoAway () {
		for (;;) {
			transform.Translate (driftSpeed*Time.deltaTime, 0f, 0f);
			if (Train.lost) {
				Destroy (gameObject);
				break;
			}
			yield return null;
		}
		yield return null;
	}

	void Update()
	{
		if (audio.isPlaying) return;
		audio.clip = clips[Random.Range(0, clips.Count)];
		audio.Play();
	}
}
