using UnityEngine;


[CreateAssetMenu(fileName = "NewFlower", menuName = "ScriptableObjects/Flower")]
public class FlowerConfig : ScriptableObject
{
    public string flowerType;
    public int growthStep;
    public float growthRate;
    public bool canGrowYearRound;
    public bool canGrowWinter;
    public bool canGrowSummer;
    public bool canGrowFall;
    public bool canGrowSpring;
    public bool needWater;
    public bool needSun;
    public int maxGrowthStage;
}
