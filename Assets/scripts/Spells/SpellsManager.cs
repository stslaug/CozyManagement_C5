using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SpellsManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static SeasonManager seasonManager;

    public static SpellsManager Instance;

    private GameObject seasonPanel;

    public static int seasonTimeRemaining; // Put remaining time. At 0 reset panel and update all flowers.

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

    /*
 * ex.
 * GameManager.Instance.UpdateAllFlowers(flowerData => flowerData.growthStep = 1);
*/
    public void setWinterBiome()
    {

        if (seasonManager == null) seasonManager = GameObject.Find("SeasonManager").GetComponent<SeasonManager>();
        Debug.Log("Setting Wintertime");
        gameManager.playerData.spellCast = true;
        seasonManager.ChangeSeason(Season.Winter);
       
       
    }

    public void growAllFlowers()
    {

        Debug.Log("Growing All Flowers");
        gameManager.playerData.spellCast = true;
        gameManager.UpdateAllFlowers(flowerData => flowerData.growthStep = 10);
        

    }
}
