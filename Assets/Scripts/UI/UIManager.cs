using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public bool paused;
    private GameObject textPauseGame;
    private GameObject pauseRestartInput;

    private UIManager() { }

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        paused = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        textPauseGame = GameObject.Find("PauseTextMessageOnScreen");
        pauseRestartInput = GameObject.Find("PauseRestartInput");
        textPauseGame.GetComponent<Text>().enabled = false;
    }

    //Reloads the Level
    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            textPauseGame.GetComponent<Text>().enabled = true;
            pauseRestartInput.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon_RePlay");
        }
        else if (!paused)
        {
            Time.timeScale = 1;
            textPauseGame.GetComponent<Text>().enabled = false;
            pauseRestartInput.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon_Pause");
        }

    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
