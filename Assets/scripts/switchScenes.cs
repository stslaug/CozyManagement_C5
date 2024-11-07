using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class switchScenes : MonoBehaviour
{
    [SerializeField] public string sceneName;
    private int saveSlotIndex;
  

    void Start()
    {
        // Assuming SaveManager or SaveLoader is responsible for loading and storing the current saveSlotIndex
        if (SaveManager.Instance != null)
        {
            // Retrieve the current save slot index from the loaded save (if any)
            saveSlotIndex = SaveManager.Instance.CurrentSaveSlotIndex;
        }

        // Add listener for button click to change scene and save the game
        GetComponent<Button>().onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        // Save the game before switching scenes
        SaveSelectionMenu.Instance.SaveCurrentGame();

        Debug.Log("Saving game... Exiting...");
        // Load the new scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}