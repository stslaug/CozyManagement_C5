using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SpellsManager : MonoBehaviour
{
    public static GameManager gameManager;

    public static SpellsManager Instance;

    private GameObject seasonPanel;

    public int seasonTimeRemaining; // Put remaining time. At 0 reset panel and update all flowers.

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSeason()
    {
        // Find the season panel by name
        GameObject panel = GameObject.Find("SeasonPanel"); // Ensure the name matches exactly

        if (panel != null)
        {
   
            // Get the Image component
            Image panelImage = panel.GetComponent<Image>();
            if (panelImage != null)
            {
               
                panelImage.color = new Vector4(0.4f, 0.3f, 0.9f, 0.2f);
                
                Debug.Log("SeasonPanel color changed to blue.");
            }
            else
            {
                Debug.LogError("Image component not found on SeasonPanel.");
            }
        }
        else
        {
            Debug.LogError("SeasonPanel GameObject not found.");
        }
    }
    /*
 * ex.
 * GameManager.Instance.UpdateAllFlowers(flowerData => flowerData.growthStep = 1);
*/
    public void setWinterBiome()
    {

            Debug.Log("Setting Wintertime");
           gameManager.playerData.spellCast = true;
            gameManager.UpdateAllFlowers(flowerData => flowerData.growthStep = 100);
        ChangeSeason();
       
    }
}
