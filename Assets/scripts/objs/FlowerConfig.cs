using UnityEngine;


[CreateAssetMenu(fileName = "NewFlower", menuName = "ScriptableObjects/Flower")]
public class FlowerConfig : ScriptableObject
{
    public string flowerType;
    public int maxGrowthStage;
    public float growthRate;
    public bool diesIfWinter;
    public int cost;
}
