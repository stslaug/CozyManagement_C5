using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SpellsManager : MonoBehaviour
{


    private void Awake()
    {

        
    }
    // Start is called before the first frame update
    void Start()
    {


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

        Debug.Log("Setting Wintertime // NOT IMPLEMENTED SpellsManager.cs");
        //gameManager.playerData.spellCast = true;
        //gameManager.UpdateAllFlowers(flowerData => flowerData.growthStep = 100);
        //ChangeSeason();
       
    }
}
