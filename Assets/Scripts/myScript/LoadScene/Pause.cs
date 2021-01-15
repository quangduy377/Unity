using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool paused;
    public GameObject pauseMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
    }
    public void Resume()
    {
        Debug.Log("resume pressed");
        paused = false;
    }
    public void Quit()
    {
        Application.LoadLevel("Intro");
    }
}
