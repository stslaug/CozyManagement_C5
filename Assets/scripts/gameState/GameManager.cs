using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DataModels;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject pauseMenu;

    private List<string> sceneNames = new List<string> { "mainMenu", "morning_shop", "night_shop", "rooftop_garden" };
    public int currentSceneIndex = 0;

    public InventoryData inventoryData;
    public List<GameObject> allFlowers = new List<GameObject>();

    // Reference to the season panel and its Image component
    private GameObject seasonPanel;
      

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

//******************************************* SAVE FUNCTIONALITY CAN GO HERE
    // Exit or return to main menu
    public void ExitFunction()
    {
        if (SceneManager.GetSceneByName("mainMenu") != SceneManager.GetActiveScene())
        {
            Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("mainMenu");

            
        }
        else
        {
            Debug.Log("User has initiated exiting the game");
            Application.Quit();
        }
    }

//******************************************* SCENE HANDLING
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //public void LoadSceneAsync() //implement if the scenes take a long time to load

//******************************************* MANAGE ALL INSTANCE DATA
    // Add a new flower instance to the list
    public void AddFlower(GameObject newFlower)
    {
        allFlowers.Add(newFlower);
        Debug.Log("Flower added to the garden.");
    }
    // Find the flower in the list and remove it
    public void RemoveFlower(GameObject flower)
    {
        allFlowers.Remove(flower);
        Debug.Log("Flower removed from the garden.");
    }

}
