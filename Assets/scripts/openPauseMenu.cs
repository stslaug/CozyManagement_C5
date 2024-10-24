using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void ButtonAction();
public class openPauseMenu : MonoBehaviour
{
    public static bool isGamePaused;
    // Start is called before the first frame update

    public GameObject PauseMenuBehaviour;

    void Start()
    {
        PauseMenuBehaviour = GameObject.Find("MenuComponents_Canvas");
        PauseMenuBehaviour.SetActive(false);
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown("escape"))
        {
            if (!PauseMenuBehaviour.activeSelf) PauseGame();
            else ResumeGame();
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0; // Stop Time
        PauseMenuBehaviour.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming Session");
        // If Game is already paused
        Time.timeScale = 1;
        isGamePaused = false;
        PauseMenuBehaviour.SetActive(false);
    }
}


