using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train : MonoBehaviour {
	public Wagon[] wagons = new Wagon[3];
	public Passenger[] passengers = new Passenger[6];
	private int currentWagon = 0;
	public static int level {
		get;
		private set;
	}
	public static int score {
		get;
		private set;
	}
	public static bool lost {
		get;
		private set;
	}
	public float[] scales = new float[6] { .5f, .6f, .7f, .8f, .9f, 1f };

	private List<AudioClip> boos;
	private List<AudioClip> cheers;
	private AudioSource audio;

	private void Awake()
	{
		audio = GetComponent<AudioSource>();
		boos = new List<AudioClip>();
		cheers = new List<AudioClip>();
		for (int i = 1; ; i++)
		{
			var clip = Resources.Load("Audio/Finals/Boo" + i);
			if (clip == null)
				break;
			boos.Add(clip as AudioClip);
		}
		for (int i = 1; ; i++)
		{
			var clip = Resources.Load("Audio/Finals/Cheer" + i);
			if (clip == null)
				break;
			cheers.Add(clip as AudioClip);
		}
	}

	void Start () {
		level = 0;
		score = 0;
		lost = false;

		StartCoroutine (LateStart (.1f));
	}

	IEnumerator LateStart (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		ChangeSpeed ();
	}

	public void DetachWagon () {
		wagons[currentWagon].Detach ();
		currentWagon++;
		if (currentWagon >= wagons.Length) {
			Debug.Log ("Lost : " + score);
			lost = true;
			Time.timeScale = 0f;
		}
	}

	public void Trick (bool success, int value) {/*
		Debug.Log ("------------------");
		Debug.Log ("Level : " + level);
		Debug.Log ("Wagon : " + currentWagon);//*/
		if (success) {
			audio.clip = cheers[Random.Range(0, cheers.Count)];
			audio.Play();
			if (value > 0 && level > 0 && level / 2 == (level - 1) / 2) {
				level--;
				passengers[level].SetTired (false);
			}
			score += value;
		} else
		{
			audio.clip = boos[Random.Range(0, boos.Count)];
			audio.Play();
			passengers[level].SetTired (true);
			if (level / 2 != (level + 1) / 2) {
				DetachWagon ();
			}
			level++;
		}
	}

	public void ChangeSpeed () {
		DecorScrolling[] scrolling = GameObject.FindObjectsOfType<DecorScrolling> ();
		for (int i = 0; i < scrolling.Length; i++) {
			scrolling[i].scale = scales[level];
		}
	}

	public static void AddScore (int value) {
		score += value;
	}
}
