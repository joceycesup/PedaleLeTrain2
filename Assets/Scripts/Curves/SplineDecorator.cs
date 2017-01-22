﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineDecorator : MonoBehaviour
{

	public BezierSpline spline;

	public int precision;
	public float overlap = 1.0f;
	//public float sideDisplacement = 1.0f;

	public bool lookForward;

	public GameObject[] items;
	public Vector3[] extents;
	private List<GameObject> links;
	private List<float> linksT;

	void Awake () {
		links = new List<GameObject> ();
		linksT = new List<float> ();
		extents = new Vector3[items.Length];
		for (int i = 0; i < items.Length; ++i) {
			items[i] = Instantiate (items[i]);
			items[i].transform.parent = transform;
			items [i].SetActive (false);

			extents [i] = items [i].GetComponent<BoxCollider2D> ().size * overlap;
			if (extents[i].y <= 0.0f) {
				items = null;
				Debug.Log ("Incorrect extents");
				break;
			}
		}
	}

	public void Decorate () {
		if (precision <= 0 || items == null || items.Length == 0 || overlap <= 0.0f) {
			return;
		}

		float t = 0.0f;
		//float d = 0.0f;
		//Vector3 displacement = new Vector3 ();
		int i = 0;
		int n = 0;
		while (t <= 1.0f) {
			float tmpT = t;

			GameObject item;
			if (links.Count <= n) {
				item = Instantiate (items [i]);
				item.transform.parent = transform;
				links.Add (item);
				linksT.Add (t);
			} else {
				item = links [n];
			}
			item.SetActive (true);
			Vector3 position = spline.GetPoint (t);
			if (lookForward) {
				Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, spline.GetDirection (t));
				item.transform.rotation = tmpRot;
			}
			item.transform.position = position;
			t = spline.GetT (t, extents [i].y, precision);

			n++;
			i++;
			i %= items.Length;

			if (tmpT == t) {
				Debug.Log ("t is not getting bigger");
				break;
			}
		}
		for (i = n; i < links.Count; i++) {
			links[i].SetActive (false);
		}
	}
	/*
	void Update () {
		if (precision <= 0 || items == null || items.Length == 0 || overlap <= 0.0f) {
			return;
		}

		float t = 0.0f;
		//float d = 0.0f;
		//Vector3 displacement = new Vector3 ();
		int i = 0;
		int n = 0;
		while (t <= 1.0f) {
			GameObject item;
			if (links.Count <= n) {
				item = Instantiate (items [i]);
				item.transform.parent = transform;
				links.Add (item);
				linksT.Add (t);
			} else {
				item = links [n];
			}
			links [n].SetActive (true);
			Vector3 position = spline.GetPoint (t);
			if (lookForward) {
				Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, spline.GetDirection (t));
				item.transform.rotation = tmpRot;
			}
			item.transform.position = position;
			t = spline.GetT (t, extents [i].y, 10);

			n++;
			i++;
			i %= items.Length;
		}
		for (; n < links.Count; ++n)
			links [n].SetActive (false);
	}//*/
}