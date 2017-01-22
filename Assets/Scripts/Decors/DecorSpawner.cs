using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// will spawn the decor elements randomly in the scene
public class DecorSpawner : MonoBehaviour {
    [SerializeField]
    private GameObject[] decors;
    public float beginTimeRandom = 1.5f; 
    public float endTimeRandom = 4.5f;
    public float changeZIndex = -2f;

    private List<GameObject> decorSpawning = new List<GameObject>();

	private float nextSpawn = float.MaxValue;

    private void Awake()
    {
        InitializeDecorElements();
    }

    void Start()
    {
		//StartCoroutine(SpawnRandomDecor());
		nextSpawn = Time.time + Random.Range (beginTimeRandom, endTimeRandom);
	}

	void Update () {
		if (Time.time >= nextSpawn) {
			int index = Random.Range (0, decorSpawning.Count);
			while (true) {
				if (!decorSpawning[index].activeInHierarchy) {
					decorSpawning[index].SetActive (true);
					decorSpawning[index].transform.position = new Vector3 (transform.position.x, transform.position.y, changeZIndex);
					break;
				} else {
					index = Random.Range (0, decorSpawning.Count);
				}
			}
			nextSpawn = Time.time + Random.Range (beginTimeRandom, endTimeRandom);
		}
	}

    void InitializeDecorElements()
    {
        int index = 0;
        for (int i=0; i < decors.Length * 3; i++)
        {
            GameObject obj = Instantiate(decors[index], new Vector3(transform.position.x, transform.position.y, -2f), Quaternion.identity) as GameObject;
            decorSpawning.Add(obj);
           // Debug.Log("Adding obj "+ obj+" \n");
            decorSpawning[i].SetActive(false); // get element i from the list and only activates the decor element when we want it
           // Debug.Log("the object "+""+"has been correctly added to the queue");
            index++;
            // preventing index out of bounds exception
            if(index == decors.Length)
            {
                index = 0;
            }
        }
    }

    // randomize the list of elements
    void Shuffle()
    {
        for(int i = 0; i < decorSpawning.Count; i++)
        {
            GameObject temp = decorSpawning[i]; // temporary GameObject stored to shuffle it with a random object
            int random = Random.Range(i, decorSpawning.Count);
            //element at i is replaced by the random index
            decorSpawning[i] = decorSpawning[random];
            decorSpawning[random] = temp;
        }
    }

    // infinite co-routine for the spawn of decor elements
    IEnumerator SpawnRandomDecor()
    {
        yield return new WaitForSeconds(Random.Range(beginTimeRandom, endTimeRandom));

        int index = Random.Range(0, decorSpawning.Count);

        while (true)
        {
            if (!decorSpawning[index].activeInHierarchy)
            {
                decorSpawning[index].SetActive(true);
                decorSpawning[index].transform.position = new Vector3(transform.position.x, transform.position.y, changeZIndex);
                break;
            }
            else
            {
                index = Random.Range(0, decorSpawning.Count);
            }
        }
        StartCoroutine(SpawnRandomDecor());
    }

}
