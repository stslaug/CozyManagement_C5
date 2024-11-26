using UnityEngine;
using TMPro;


public class DaysTracker : MonoBehaviour
{

    public TextMeshProUGUI dayText; // Reference to the gold display text box


    private int currDay;
    public GameManager gameManager;

    private void Awake()
    {

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
        currDay = 0;
    }

    private void Update()
    {
        UpdateDayDisplay();
    }

    // Update the UI Text with the current gold amount
    public void UpdateDayDisplay()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (currDay != gameManager.saveData.playerData.goldCount)
        {
            if (dayText == null)
            {
                dayText = GameObject.Find("dayText").GetComponent<TextMeshProUGUI>();
            }
            if (dayText != null)
            {
                dayText.text = "Day: " + gameManager.saveData.playerData.currentDay; // Update the displayed text
            }
            else
            {
                Debug.Log("dayText reference is not assigned!");
            }
        }
    }

}