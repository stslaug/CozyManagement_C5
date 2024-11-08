using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoader : MonoBehaviour
{
    public GameObject flowerPrefab;         // Flower prefab for instantiation
    private SaveData loadedSaveData;        // Loaded save data
    public SaveLoader Instance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Make the SaveManager a root GameObject if it's not already
            if (transform.parent != null)
            {
                transform.SetParent(null);  // Make this object a root object
            }

            DontDestroyOnLoad(gameObject);  // Prevent it from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }


    // Updated method to load player data using slotIndex
    public void LoadPlayerData(int slotIndex)
    {
        // Load save using slotIndex
        loadedSaveData = SaveManager.Instance.Load(slotIndex);

        if (loadedSaveData == null)
        {
            Debug.LogError("Failed to load save data.");
            return;
        }

        // Set the player's data
        PlayerData playerData = loadedSaveData.playerData;

        // Load the player's last saved scene and place flowers
        StartCoroutine(LoadSceneAndPlaceFlowers(playerData.lastScene, loadedSaveData.flowerData));
    }

    private IEnumerator LoadSceneAndPlaceFlowers(string sceneName, List<FlowerData> flowerDataList)
    {
        // Ensure the flowerPrefab is assigned
        if (flowerPrefab == null)
        {
            Debug.LogError("Flower prefab is not assigned!");
            yield break;
        }

        // Load the player's last scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Ensure flowerDataList is not null and has data to place
        if (flowerDataList != null && flowerDataList.Count > 0)
        {
            foreach (FlowerData flowerData in flowerDataList)
            {
                // Set position for the flower based on saved coordinates
                Vector3 position = new Vector3(flowerData.positionX, flowerData.positionY, 0);
                GameObject flower = Instantiate(flowerPrefab, position, Quaternion.identity);

                // Set the growth step of the flower if the Flower script is available
                Flower flowerScript = flower.GetComponent<Flower>();
                if (flowerScript != null)
                {
                    flowerScript.SetGrowthStep(flowerData.growthStep);
                }
                else
                {
                    Debug.LogError("Flower script not found on flower prefab.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No flower data found in the loaded save.");
        }
    }
}
