using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    public SaveManager saveManager;
    public PlayerData playerData;
    private int selectedSaveSlot = -1;
    public int goldCount;
    public int currentDay;

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

        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        // Initialize player data with default values (this will be overwritten when loading from save)
        playerData = new PlayerData
        {
            goldCount = 0,
            currentDay = 1,
            creationDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            lastScene = "MainMenu",
            saveSlot = -1
        };
    }

    // Call this method to load save data into the GameManager
    public void LoadGame(int saveSlot)
    {
        SaveData saveData = saveManager.Load(saveSlot);
        if (saveData != null)
        {
            playerData = saveData.playerData;
            Debug.Log("Game Loaded Successfully");
        }
        else
        {
            Debug.LogWarning("No save data found for slot " + saveSlot);
        }
    }

    // This method is called when the player decides to save the game
    public void SaveGame(int saveSlot)
    {
        if (saveSlot < 0 || saveSlot >= 3)  // Assuming you have 3 save slots
        {
            Debug.LogError("Invalid save slot!");
            return;
        }

        // Create a SaveData object from the current player data
        SaveData saveData = new SaveData
        {
            playerData = playerData
        };

        // Use SaveManager to save the data to the specified slot
        saveManager.Save(saveData, saveSlot);
        Debug.Log("Game Saved to Slot " + saveSlot);
    }

    // Optional method to set the selected save slot
    public void SetSelectedSaveSlot(int slotIndex)
    {
        selectedSaveSlot = slotIndex;
        Debug.Log("Selected Save Slot: " + selectedSaveSlot);
    }

    // For testing purposes: Simulate an update to player data
    public void UpdatePlayerData(int gold, int day)
    {
        playerData.goldCount = gold;
        playerData.currentDay = day;
        playerData.lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
