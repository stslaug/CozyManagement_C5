using UnityEngine;
using TMPro;


public class DaysTracker : MonoBehaviour
{
    public static DaysTracker Instance; // Singleton instance
    public TextMeshProUGUI dayText; // Reference to the gold display text box
    public GameManager gameManager;

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
        if (dayText == null)
        {
            dayText = GameObject.Find("dayText").GetComponent<TextMeshProUGUI>();
            if (dayText == null)
            {
                Debug.LogWarning("dayText not found in the scene. Make sure the TextMeshProUGUI component is assigned.");
            }
        }
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        UpdateDayDisplay();
    }

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        if (dayText == null)
        {
            dayText = GameObject.Find("dayText").GetComponent<TextMeshProUGUI>();
        }


        UpdateDayDisplay();
    }

    private void Update()
    {
       

    }

    // Update the UI Text with the current gold amount
    public void UpdateDayDisplay()
    {
        if (dayText == null)
        {
            dayText = GameObject.Find("dayText").GetComponent<TextMeshProUGUI>();
        }
        if (dayText != null)
        {
            dayText.text = "Day: " + GameManager.Instance.playerData.currentDay; // Update the displayed text
            Debug.Log("in update Day Display");
        }
        else
        {
            Debug.Log("dayText reference is not assigned!");
        }
    }

}
