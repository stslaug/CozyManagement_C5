using UnityEngine;
using TMPro;
using Unity.VisualScripting; // Include this if you're using TextMeshPro

public class CurrencyTracker : MonoBehaviour
{
    public static CurrencyTracker Instance; // Singleton instance
    public int gold = 0; // Initial gold amount
    public TextMeshProUGUI goldText; // for the text box

    private void Awake()
    {
        // Implement Singleton
        if (Instance == null)
        {
            Instance = this; // Assign the instance
            DontDestroyOnLoad(gameObject); // Make the instance persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance, but do not destroy UI elements
        }
    }

    private void Start()
    {
        LoadGold();
        UpdateGoldDisplay();
    }
    // Load gold from PlayerPrefs
    public void LoadGold()
    {
        gold = PlayerPrefs.GetInt("PlayerGold", 0); // Load gold or default to 0
        UpdateGoldDisplay();
    }

    // Add gold and update the display
    public void AddGold(int amount)
    {

        gold += amount;
        SaveGold();
        Debug.Log("Added Gold: " + amount + " | New Gold Value: " + gold);
        UpdateGoldDisplay();
    }

    // Spend gold
    public bool SpendGold(int amount)
    {
        Debug.Log("Current Gold: " + gold + " | Attempting to Spend: " + amount);

        if (gold >= amount)
        {
            gold -= amount;
            SaveGold();
            UpdateGoldDisplay();
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    // Save gold to PlayerPrefs
    private void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", gold);
        PlayerPrefs.Save();
        Debug.Log("Saved Gold to PlayerPrefs: " + gold); // Log the saved value
    }

    // Update the UI Text with the current gold amount
    private void UpdateGoldDisplay()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold; // Update the displayed text
            Debug.Log("UI Updated to Display Gold: " + gold); // Log the UI update
        }
        else
        {
            Debug.LogWarning("goldText reference is not assigned!"); // Warning if UI reference is null
        }
    }






    public void AddGoldButton(int amount)
    {
        CurrencyTracker.Instance.AddGold(amount);
    }

    public void SpendGoldButton(int amount)
    {
        CurrencyTracker.Instance.SpendGold(amount);
    }

}

