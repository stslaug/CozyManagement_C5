using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Links configs with buttons. Will trigger on click
public class FlowerSelectionManager : MonoBehaviour
{
    public FlowerConfig fireFlowerConfig;
    public FlowerConfig waterFlowerConfig;
    public FlowerConfig windFlowerConfig;

    private FlowerConfig selectedFlowerConfig;

    public void SelectFireFlower() => selectedFlowerConfig = fireFlowerConfig;
    public void SelectWaterFlower() => selectedFlowerConfig = waterFlowerConfig;
    public void SelectWindFlower() => selectedFlowerConfig = windFlowerConfig;

    public FlowerConfig GetSelectedFlowerConfig()
    {
        return selectedFlowerConfig;
    }
}
