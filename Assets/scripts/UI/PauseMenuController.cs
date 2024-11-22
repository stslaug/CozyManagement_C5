using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private static bool isPaused = false;

    private GameObject pauseMenuUI;
    private CanvasGroup pauseMenuCanvasGroup;

    private void Awake()
    {

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPauseMenuUI();
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
            FindPauseMenuUI();
            if (pauseMenuUI == null)
            {
                Debug.LogWarning("PauseMenu not found in the current scene. Cannot toggle pause menu.");
                return;
            }
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenuUI.transform.localScale = Vector3.one;
            if (pauseMenuCanvasGroup != null)
            {
                pauseMenuCanvasGroup.interactable = true;
                pauseMenuCanvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            pauseMenuUI.transform.localScale = Vector3.zero;
            if (pauseMenuCanvasGroup != null)
            {
                pauseMenuCanvasGroup.interactable = false;
                pauseMenuCanvasGroup.blocksRaycasts = false;
            }
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public bool getPause()
    {
        return isPaused;
    }

    private void FindPauseMenuUI()
    {
        pauseMenuUI = GameObject.Find("PauseMenu");

        if (pauseMenuUI == null)
        {
            Debug.Log("PauseMenu panel not found in the current scene.");
        }
        else
        {
            pauseMenuUI.transform.localScale = Vector3.zero;

            pauseMenuCanvasGroup = pauseMenuUI.GetComponent<CanvasGroup>();
            if (pauseMenuCanvasGroup == null)
            {
                pauseMenuCanvasGroup = pauseMenuUI.AddComponent<CanvasGroup>();
            }

            pauseMenuCanvasGroup.interactable = false;
            pauseMenuCanvasGroup.blocksRaycasts = false;
        }
    }
}