using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private static bool isPaused = false;

    [SerializeField] private GameObject pauseMenuUI; // Assign via Inspector


    private void Awake()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenuUI is not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuUI == null)
        {
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenuUI.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuUI.SetActive(false);

            Time.timeScale = 1f;
        }

        Debug.Log($"Pause Menu is now {(isPaused ? "Active" : "Inactive")}");
    }

    public bool GetPauseState()
    {
        return isPaused;
    }
}
