using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedData", menuName = "ScriptableObjects/SeedData", order = 1)]
public class SeedData : ScriptableObject
{
    [Header("Seed Properties")]
    public string seedName;            // Unique name of the seed
    public Sprite seedIcon;            // Icon to display in the UI
    public GameObject flowerPrefab;    // Prefab of the flower to instantiate
    public float growthTime;           // Time it takes to grow (in seconds or game days)
    public int harvestValue;           // Gold awarded upon harvesting

    [Header("Growth Requirements")]
    public int requiredWater;          // Water needed for growth
    public int requiredSunlight;       // Sunlight needed for growth
    // Add additional properties as needed, such as soil type, fertilizer requirements, etc.
}