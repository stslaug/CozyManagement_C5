using UnityEngine;
using TMPro;


public class CurrencyTracker : MonoBehaviour
{
    public static CurrencyTracker Instance; // Singleton instance
    public TextMeshProUGUI goldText; // Reference to the gold display text box
    public GameManager gameManager;
    private int currGold;
    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist instance across scenes
        }
        else{
            Destroy(gameObject); // Destroy duplicate instance
        }
        if (goldText == null)
        {
            goldText = GameObject.Find("goldText").GetComponent<TextMeshProUGUI>();
            if (goldText == null)
            {
                Debug.LogWarning("goldText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
            }
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        if (goldText == null)
        {
            goldText = GameObject.Find("goldText").GetComponent<TextMeshProUGUI>();
            if (goldText == null)
            {
                Debug.LogWarning("goldText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
            }
        }
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

                UpdateGoldDisplay();
               
            }
            else
            {
                Debug.LogWarning("Not enough gold to complete transaction.");
                
            }
        }
        else { Debug.LogError("GameManager not Initialized."); }
    }

    // Update the UI Text with the current gold amount
    public void UpdateGoldDisplay()
    {
        if (gameManager != null)
        {
            if (gameManager.saveData.playerData.goldCount != currGold)
            {
                currGold = gameManager.saveData.playerData.goldCount;
                if (goldText != null)
                {
                    goldText.text = "Gold: " + gameManager.saveData.playerData.goldCount; // Update the displayed text
                }
                else
                {
                    Debug.Log("goldText reference is not assigned!");
                }
            }
        }
    }

}
