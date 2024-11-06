using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class nextScene : MonoBehaviour
{
    GameManager gameManager;
    // Define the scenes to switch between
    private string[] sceneNames = { "temp_rooftop", "temp_shop" }; // Add more scene names here

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        GetComponent<Button>().onClick.AddListener(ChangeScene);
    }

    private void ChangeScene()
    {
        // Get the current scene's index from the defined list
        int currentSceneIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);

        gameManager.SaveGame();
        // Determine the next scene index
        int nextSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;

        // Load the next scene using the scene name from the list
        SceneManager.LoadScene(sceneNames[nextSceneIndex]);
        gameManager.LoadGame();
    }
}
