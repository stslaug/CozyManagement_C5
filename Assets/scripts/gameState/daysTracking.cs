using UnityEngine;
using TMPro;

public class DaysTracker : MonoBehaviour
{
    public TextMeshProUGUI dayText; // Reference to the day display text box
    private int currDay;
    public GameManager gameManager;

    private void Start()
    {
        // Initialize GameManager reference
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in the scene!");
            }
        }

        // Initialize dayText reference
        if (dayText == null)
        {
            GameObject dayTextObject = GameObject.Find("dayText");
            if (dayTextObject != null)
            {
                dayText = dayTextObject.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("dayText GameObject not found in the scene!");
            }
        }

        if (gameManager != null && gameManager.getSaveData().playerData != null)
        {
            currDay = gameManager.getSaveData().playerData.currentDay;
            UpdateDayDisplay(currDay);

            // Subscribe to the day change event
            gameManager.OnDayChanged += HandleDayChanged;
        }
        else
        {
            Debug.LogError("SaveData or PlayerData is null!");
        }
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnDayChanged -= HandleDayChanged;
        }
    }

    // Event handler for day changes
    private void HandleDayChanged(int newDay)
    {
        currDay = newDay;
        UpdateDayDisplay(currDay);
    }

    // Update the UI Text with the current day
    private void UpdateDayDisplay(int day)
    {
        if (dayText != null)
        {
            dayText.text = "Day: " + day.ToString(); // Update the displayed text
        }
        else
        {
            Debug.LogWarning("dayText reference is not assigned!");
        }
    }
}
