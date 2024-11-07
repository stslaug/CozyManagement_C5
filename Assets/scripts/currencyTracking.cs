using UnityEngine;
using TMPro;

public class CurrencyTracker : MonoBehaviour
{
    public static CurrencyTracker Instance; // Singleton instance
    public int gold = 0; // Initial gold amount
    public TextMeshProUGUI goldText; // Reference to the gold display text box
    private PlayerData currentPlayerData; // Reference to loaded player data

    private void Awake()
    {
        // Implement Singleton
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null)
            {
                transform.SetParent(null);  // Make this object a root object
            }
            DontDestroyOnLoad(gameObject); // Persist instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    private void Start()
    {
        if (currentPlayerData == null)
        {
            Debug.LogWarning("PlayerData not set. Call SetPlayerData to initialize.");
        }
        UpdateGoldDisplay();
    }

    // Set the current player data and load gold
    public void SetPlayerData(PlayerData playerData)
    {
        currentPlayerData = playerData;
        LoadGold();
    }

    // Load gold from player data
    private void LoadGold()
    {
        if (currentPlayerData != null)
        {
            gold = currentPlayerData.goldCount;
            UpdateGoldDisplay();
        }
        else
        {
            Debug.LogWarning("Attempted to load gold, but PlayerData is null.");
        }
    }

    // Add gold and update the display
    public void AddGold(int amount)
    {
        gold += amount;
        if (currentPlayerData != null)
        {
            currentPlayerData.goldCount = gold; // Update the player data
        }
        UpdateGoldDisplay();
    }

    // Spend gold
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            if (currentPlayerData != null)
            {
                currentPlayerData.goldCount = gold; // Update the player data
            }
            UpdateGoldDisplay();
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough gold to complete transaction.");
            return false;
        }
    }

    // Update the UI Text with the current gold amount
    private void UpdateGoldDisplay()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold; // Update the displayed text
        }
        else
        {
            Debug.LogWarning("goldText reference is not assigned!");
        }
    }

    public void AddGoldButton(int amount)
    {
        if (Instance != null)
        {
            Instance.AddGold(amount);
        }
        else
        {
            Debug.LogWarning("CurrencyTracker Instance is not set.");
        }
    }

    public void SpendGoldButton(int amount)
    {
        if (Instance != null)
        {
            Instance.SpendGold(amount);
        }
        else
        {
            Debug.LogWarning("CurrencyTracker Instance is not set.");
        }
    }
}
