using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSelectionMenu : MonoBehaviour
{
    public SaveManager saveManager;
    public static SaveSelectionMenu Instance;// Reference to SaveManager
    public Button[] saveSlotButtons;              // Buttons for each save slot
    public Button[] deleteSlotButtons;            // Buttons for deleting saves
    public GameObject createGamePanel;            // Panel for creating a new save
    public TextMeshProUGUI[] saveSlotTexts;       // Text components for each save slot

    private int selectedSaveSlot = -1;            // Track the currently selected save slot
    private List<SaveData> allSaves;              // List to store all save data

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
    }


    private void Start()
    {
        if (saveManager == null) Debug.LogWarning("SaveManager reference is missing.");
        if (createGamePanel == null) Debug.LogWarning("CreateGamePanel reference is missing.");
        if (saveSlotButtons == null || saveSlotButtons.Length == 0) Debug.LogWarning("Save slot buttons are not assigned.");
        if (deleteSlotButtons == null || deleteSlotButtons.Length == 0) Debug.LogWarning("Delete slot buttons are not assigned.");
        if (saveSlotTexts == null || saveSlotTexts.Length == 0) Debug.LogWarning("Save slot texts are not assigned.");

        // Add these logs to catch null references
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            if (saveSlotButtons[i] == null)
                Debug.LogWarning($"Save Slot Button {i + 1} is not assigned.");
        }

        for (int i = 0; i < deleteSlotButtons.Length; i++)
        {
            if (deleteSlotButtons[i] == null)
                Debug.LogWarning($"Delete Slot Button {i + 1} is not assigned.");
        }

        for (int i = 0; i < saveSlotTexts.Length; i++)
        {
            if (saveSlotTexts[i] == null)
                Debug.LogWarning($"Save Slot Text {i + 1} is not assigned.");
        }

        // Load all the saves when the menu is initialized
        LoadAllSaves();
    }

    // Load the save slots and populate buttons with data
    private void LoadAllSaves()
    {
        if (saveManager == null)
        {
            Debug.LogError("SaveManager is not assigned!");
            return;
        }

        Debug.Log("Loading saves...");

        allSaves = saveManager.LoadAllSaves(); // Load saved data from SaveManager

        if (allSaves == null)
        {
            Debug.LogError("Loaded saves are null. Returning...");
            allSaves = new List<SaveData>(); // Fallback to empty list
        }

        if (saveSlotButtons.Length != saveSlotTexts.Length || saveSlotButtons.Length != deleteSlotButtons.Length)
        {
            Debug.LogError("Mismatch in save slot buttons, texts, and delete buttons array lengths.");
            return;
        }

        // Iterate through save slot buttons
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            Button slotButton = saveSlotButtons[i];
            TextMeshProUGUI slotText = saveSlotTexts[i];
            Button deleteButton = deleteSlotButtons[i];

            if (slotButton == null || slotText == null || deleteButton == null) continue;

            if (i < allSaves.Count && allSaves[i] != null)
            {
                // Display existing save data
                SaveData saveData = allSaves[i];
                slotText.text = $"Save {i + 1}:\nDay: {saveData.playerData.currentDay}\nGold: {saveData.playerData.goldCount}";
                Debug.Log($"Loaded Save {i + 1} with day {saveData.playerData.currentDay} and gold {saveData.playerData.goldCount}");

                int saveIndex = i;  // Capture the index for the button click
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => LoadSaveSlot(saveIndex)); // Load the selected save slot

                // Show delete button for filled slots
                deleteButton.gameObject.SetActive(true);
                deleteButton.onClick.RemoveAllListeners();
                deleteButton.onClick.AddListener(() => DeleteSave(saveIndex)); // Delete the selected save slot
            }
            else
            {
                // Empty slot - prompt the user to create a new game
                slotText.text = $"Save {i + 1}: Create a new game";
                Debug.Log($"Slot {i + 1} is empty.");

                int emptySlotIndex = i;
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => CreateNewSave(emptySlotIndex)); // Create a new save

                // Hide the delete button for empty slots
                deleteButton.gameObject.SetActive(false);
            }
        }
    }

    // Load the selected save slot and apply the data
    private void LoadSaveSlot(int saveIndex)
    {
        SaveData saveData = allSaves[saveIndex];
        selectedSaveSlot = saveIndex; // Set selected slot index

        // Load player data and set it in the GameManager or other relevant systems
        GameManager.Instance.LoadGame(saveData.saveSlot);  // Assuming the GameManager handles game loading
        SceneManager.LoadScene(saveData.playerData.lastScene);  // Load the scene the player was last in
    }

    // Create a new save in the selected slot
    public void CreateNewSave(int slotIndex)
    {
        SaveData newSaveData = new SaveData
        {
            playerData = new PlayerData
            {
                goldCount = 0,
                currentDay = 1,  // Starting on day 1
                creationDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                lastScene = "temp_rooftop",  // Default starting scene
                saveSlot = slotIndex
            },
            npcData = new List<NPCData>(),  // Initialize NPC data
            flowerData = new List<FlowerData>() // Initialize Flower data
        };

        saveManager.Save(newSaveData, slotIndex);  // Save the new game data in the selected slot
        SceneManager.LoadScene("temp_rooftop");  // Load the initial scene
    }

    // Delete the save at the specified slot index
    private void DeleteSave(int slotIndex)
    {
        saveManager.DeleteSave(slotIndex);  // Delete the save from the manager
        LoadAllSaves();  // Reload the save slots after deletion
    }

    // Save the game to the selected save slot
    public void SaveCurrentGame()
    {
        if (selectedSaveSlot == -1)
        {
            Debug.LogError("No save slot selected!");
            return;
        }

        SaveData saveData = new SaveData
        {
            playerData = new PlayerData
            {
                goldCount = GameManager.Instance.goldCount,  // Fetch gold count from GameManager
                currentDay = GameManager.Instance.currentDay,  // Fetch current day from GameManager
                lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                creationDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                lastScene = SceneManager.GetActiveScene().name,  // Current active scene
                saveSlot = selectedSaveSlot  // Save to the selected slot
            }
        };

        saveManager.Save(saveData, selectedSaveSlot);  // Save the current game state
        Debug.Log($"Game saved to Save {selectedSaveSlot + 1}");
    }
}
