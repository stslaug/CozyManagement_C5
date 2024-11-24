using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CurrencyTracker : MonoBehaviour
{
    public TextMeshProUGUI goldText; // Reference to the gold display text box
    public GameManager gameManager;
    private int currGold;


    private void Awake()
    {

        currGold = 0;
    }

    private void Update()
    {
       
        UpdateGoldDisplay();

    }



    // Add gold and update the display
    public void AddGold(int amount)
    {
        if (goldText == null)
        {
            goldText = GameObject.Find("goldText").GetComponent<TextMeshProUGUI>();
            if (goldText == null)
            {
                Debug.LogError("goldText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
            }
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (gameManager != null)
        {
            gameManager.saveData.playerData.goldCount += amount; // Save gold to GameManager
        }
        UpdateGoldDisplay();
    }

    // Spend gold
    public void SpendGold(int amount)
    {
        if (goldText == null)
        {
            goldText = GameObject.Find("goldText").GetComponent<TextMeshProUGUI>();
            if (goldText == null)
            {
                Debug.LogError("goldText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
                return;

            }
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (gameManager != null)
        {
            if (gameManager.saveData.playerData.goldCount >= amount)
            {
                gameManager.saveData.playerData.goldCount -= amount; // Save gold to GameManager

            }
            else
            {
                Debug.LogWarning("Not enough gold to complete transaction.");

            }
        }
        else
        {
            Debug.LogError("GameManager not Initialized.");

        }
    }

    // Update the UI Text with the current gold amount
    public void UpdateGoldDisplay()
    {
        if (gameManager != null)
        {
            
                if (goldText == null)
                {
                    goldText = GameObject.Find("goldText").GetComponent<TextMeshProUGUI>();
                    if (goldText == null)
                    {
                        Debug.LogError("goldText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
                        return;
                    }
                    if (goldText.text != ("Gold: " + (gameManager.saveData.playerData.goldCount).ToString()))
                    {
                        currGold = gameManager.saveData.playerData.goldCount;

                        goldText.text = "Gold: " + gameManager.saveData.playerData.goldCount; // Update the displayed text
                    }
            }
               
        }
    }



}