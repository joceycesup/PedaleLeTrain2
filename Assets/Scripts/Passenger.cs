using UnityEngine;
using System.Collections.Generic;

public class Passenger : MonoBehaviour {
	private bool tired;
	public List<AudioClip> clips;
	private AudioSource audio;

	void Awake () {
		ChangeSpeed ();
		audio = GetComponent<AudioSource>();
		clips = new List<AudioClip>();
		for (int i = 1; ; i++)
		{
			var clip = Resources.Load("Audio/Finals/Souffle" + i);
			if (clip == null)
				break;
			clips.Add(clip as AudioClip);
		}
	}

	public void SetTired (bool value) {
		if (tired != (tired = value)) {
			GetComponent<Animator> ().Play (tired ? "tired" : "pedalling");
			if (!tired)
				ChangeSpeed ();
		}
	}

	public void ChangeSpeed () {
		GetComponent<Animator> ().speed = Random.Range (.25f, 2f);
	}

	void Update()
	{
		if (tired) return;
		if (audio.isPlaying) return;
		audio.clip = clips[Random.Range(0, clips.Count)];
		audio.Play();
	}
}
