using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject PauseMenuUI; // Reference to the pause menu UI
    private static bool isPaused;
    public Vector3 OriginalScale = Vector3.one;
    public Vector3 hidden = Vector3.zero;


    private void Awake()
    {
        PauseMenuBehaviour = GameObject.Find("PauseMenu");

        if (PauseMenuBehaviour != null)
        {
            PauseMenuBehaviour.SetActive(false); 
        }
        else
        {
            Debug.LogError("PauseMenu GameObject not found in the scene!");
        }

        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        OriginalScale = Vector3.one;
        hidden = Vector3.zero;
        if (PauseMenuUI == null)
        {
            PauseMenuUI = GameObject.Find("PauseMenu");
        }

        if (PauseMenuUI != null)
        {
            isPaused = false;
            PauseMenuUI.transform.localScale = hidden;
        }
        else if (SceneManager.GetActiveScene().name != "mainMenu")
        {
            Debug.LogError("PauseMenu GameObject not found in the scene!");
        }
    }

    private void Update()
    {
        // Toggle the menu with the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
       
    }

    // Toggles the position between visible and off-screen
    public void TogglePauseMenu()
    {
        if (PauseMenuUI == null)
        {
            PauseMenuUI = GameObject.Find("PauseMenu");
        }

        isPaused = !isPaused;
        if (isPaused) PauseMenuUI.transform.localScale = OriginalScale;
        else PauseMenuUI.transform.localScale = hidden;

    }
}
