using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// will spawn the pole elements randomly in the scene, 
// with random heights to add some spicy challenge(s) *wink wink ;)*
public class PoleSpawner : MonoBehaviour
{

	[SerializeField]
	private GameObject[] poles = new GameObject[7];
	public float beginTimeRandom = 2f;
	public float endTimeRandom = 3f;
	public float changeZIndex = -2f;
	private int firstPole = 0;

	[SerializeField]
	public GameObject[] lines = new GameObject[7];

	public GameObject[] patterns = new GameObject[3];

	GameObject lastPole = null;

	//private List<GameObject> polesSpawning = new List<GameObject>();

	public void PoleDisappeared(GameObject pole)
	{
		//mettre lastPole a droite et replacer sa ligne

		ElectricalLine el = lines[firstPole == poles.Length - 1 ? 0 : firstPole].GetComponent<ElectricalLine>();

		el.leftPole = poles[firstPole - 1 >= 0 ? firstPole - 1 : poles.Length - 1].transform;
		el.rightPole = poles[firstPole].transform;
		//random
		float randX = Random.Range(4f, 5f);// Random.Range(Camera.main.gameObject.GetComponent<BoxCollider2D>().size.x/2f, Camera.main.gameObject.GetComponent<BoxCollider2D>().size.x/1.5f);
		float randY = Random.Range(-2f, 2.5f);
		lastPole.transform.position = poles[firstPole - 1 >= 0 ? firstPole - 1 : poles.Length - 1].transform.position + new Vector3(randX, 0f, 0f);
		lastPole.transform.position = new Vector3(lastPole.transform.position.x, randY, 0f);

		GameObject ground = GameObject.Find("Parallaxe_1");
		BoxCollider2D groundBounds = ground.GetComponent<BoxCollider2D>();
		float groundHeight = ground.transform.position.y + groundBounds.offset.y * ground.transform.localScale.y + groundBounds.size.y * ground.transform.localScale.y / 2;
		float poleHeight = lastPole.transform.position.y - groundHeight;
		lastPole.transform.localScale = Vector3.one;
		float poleScale = poleHeight / lastPole.GetComponent<BoxCollider2D>().size.y;
		lastPole.transform.localScale = new Vector3(1f, poleScale);

		if (Random.Range (0f, 1f) > .5f) {
			Vector3 pos = poles[firstPole - 1 >= 0 ? firstPole - 1 : poles.Length - 1].transform.position;
			pos += lastPole.transform.position;
			pos /= 2f;
			pos += new Vector3 (0f, Random.Range (.5f, 1.5f));
			GameObject obj = Instantiate (patterns[Random.Range (0, patterns.Length)], pos, Quaternion.identity) as GameObject;
		}

		lastPole = pole;

		firstPole++;
		if (firstPole >= poles.Length)
		{
			lines[lines.Length - 1] = lines[0];
			for (int i = 0; i < lines.Length - 1; i++)
			{
				lines[i] = lines[i + 1];
			}
			firstPole = 0;
		}
		el.ResetLine();
	}

	private void Start()
	{
		InitializePoles();
		InitializeLines();
		lastPole = poles[0];
	}

	void InitializeLines()
	{
		for (int i = 0; i < lines.Length - 1; i++)
		{
			if (lines[i] == null)
			{
				// random
				GameObject obj = Instantiate(lines[0], Vector3.zero, Quaternion.identity) as GameObject;
				obj.SetActive(true);
				lines[i] = obj;
			}
			ElectricalLine el = lines[i].GetComponent<ElectricalLine>();
			el.leftPole = poles[i].transform;
			el.rightPole = poles[i + 1].transform;
			//el.ResetLine();
		}
		lines[lines.Length - 1] = lines[0];
	}


	void InitializePoles()
	{
		float lastPosX = transform.position.x;
		float lastPosY = transform.position.y;
		// random

		GameObject ground = GameObject.Find("Parallaxe_1");
		BoxCollider2D groundBounds = ground.GetComponent<BoxCollider2D>();
		float groundHeight = ground.transform.position.y + groundBounds.offset.y * ground.transform.localScale.y + groundBounds.size.y * ground.transform.localScale.y / 2;

		for (int i = 0; i < poles.Length; i++)
		{
			if (poles[i] == null)
			{
				lastPosX = lastPosX + Random.Range(4f, 5f);
				lastPosY = lastPosY + Random.Range(lastPosY, 1.5f);
				GameObject obj = Instantiate(poles[0], new Vector3(lastPosX, lastPosY, changeZIndex), Quaternion.identity) as GameObject;

				obj.transform.localScale = Vector3.one;
				float poleHeight = obj.transform.position.y - groundHeight;
				float poleScale = poleHeight / obj.GetComponent<BoxCollider2D>().size.y;
				obj.transform.localScale = new Vector3(1f, poleScale);

				obj.SetActive(true);
				poles[i] = obj;
			}
			else
			{
				lastPosX = poles[i].transform.position.x;
			}
		}
	}
}
