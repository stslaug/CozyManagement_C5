using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveDirectory;  // Directory where saves are stored
    public static SaveManager Instance;  // Singleton instance
    public int CurrentSaveSlotIndex = -1;  // Index of the current save slot

    // Ensure the singleton pattern is followed
    private void Awake()
    {
        // Singleton pattern: ensures only one SaveManager exists in the scene
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

        // Initialize the save directory based on the user's Documents folder
        saveDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "My Games", "CozyManagement");

        // Ensure the save directory exists, create it if not
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log("Save directory created: " + saveDirectory);
        }
    }


    // Load all saves from the save directory
    public List<SaveData> LoadAllSaves()
    {
        List<SaveData> allSaves = new List<SaveData>();

        // Loop through all possible save slots (assuming 3 save slots, adjust as needed)
        for (int i = 0; i < 3; i++)  // You can change the 3 to the number of save slots you want
        {
            string filePath = Path.Combine(saveDirectory, $"save_{i + 1}.json");

            // Check if the save file exists
            if (File.Exists(filePath))
            {
                try
                {
                    // Read the JSON from the file
                    string json = File.ReadAllText(filePath);
                    SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                    if (saveData != null)
                    {
                        // Set the save slot index to the loaded save data
                        saveData.saveSlot = i;
                        allSaves.Add(saveData);
                    }
                    else
                    {
                        Debug.LogWarning($"Loaded data from {filePath} is null.");
                        allSaves.Add(null);  // Add null to represent an empty slot if the data is invalid
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error loading save file {filePath}: {e.Message}");
                    allSaves.Add(null);  // Add null if loading the file fails
                }
            }
            else
            {
                // If there's no save file, add a null entry to represent an empty slot
                allSaves.Add(null);
            }
        }

        return allSaves;
    }

    // Save a SaveData object to the corresponding slot (using index)
    public void Save(SaveData saveData, int slotIndex)
    {
        string filePath = Path.Combine(saveDirectory, $"save_{slotIndex + 1}.json");

        // Convert the SaveData object to a JSON string
        string json = JsonUtility.ToJson(saveData, true);

        try
        {
            // Write the JSON to the file
            File.WriteAllText(filePath, json);
            Debug.Log($"Save successful to slot {slotIndex + 1}");

            // Set the current save slot
            CurrentSaveSlotIndex = slotIndex;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving to {filePath}: {e.Message}");
        }
    }

    // Load the save data for a specific slot
    public SaveData Load(int saveSlot)
    {
        string savePath = GetSavePath(saveSlot);
        if (File.Exists(savePath))
        {
            try
            {
                // If save file exists, load and return the data
                string json = File.ReadAllText(savePath);
                SaveData loadedSaveData = JsonUtility.FromJson<SaveData>(json);
                loadedSaveData.saveSlot = saveSlot;  // Ensure save slot is set correctly
                CurrentSaveSlotIndex = saveSlot;
                return loadedSaveData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading save file {savePath}: {e.Message}");
                return null;  // Return null if loading the file fails
            }
        }
        else
        {
            // Return null if no save exists for the slot
            Debug.LogWarning($"Save file for slot {saveSlot} not found.");
            return null;
        }
    }

    // Helper function to get the path of a save file based on slot index
    private string GetSavePath(int saveSlot)
    {
        return Path.Combine(saveDirectory, $"save_{saveSlot + 1}.json");
    }

    // Delete the save data at the specified slot index
    public void DeleteSave(int slotIndex)
    {
        string filePath = Path.Combine(saveDirectory, $"save_{slotIndex + 1}.json");

        // Delete the save file if it exists
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Save file {filePath} deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error deleting save file {filePath}: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"No save file found at {filePath} to delete.");
        }
    }
}
