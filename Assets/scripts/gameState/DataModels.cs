using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int goldCount;
    public int currentDay = 1;
    public string lastTimePlayed;
    public string creationDate;
    public string lastScene;
    public bool spellCast;
    public Season rooftopSeason;

    public PlayerData()
    {
        this.goldCount = 500;
        this.currentDay = 1;
        this.lastTimePlayed = DateTime.Today.ToString();
        this.creationDate = DateTime.Today.ToString();
        this.lastScene = "temp_shop";
        this.spellCast = false;
        this.rooftopSeason = Season.Spring;
    }
}

[System.Serializable]
public class NPCData
{
    public string name;
    public int relationship = 50;
    public int tasksCompleted = 0;
    public int tasksFailed = 0;
}

[System.Serializable]
public class FlowerData
{
    public Vector3 position;
    public string scene_name;
    public int growthStep = 1;
    public float growthRate = 1f;
    public FlowerType flowerType;
    public List<Season> seasonsAllowed; // Allows multiple seasons
    public List<Need> currentNeeds;      // Allows multiple needs

    public FlowerData()
    { }
    

    public FlowerData(Vector3 position, string scene_name, FlowerType flowerType)
    {
        this.position = position;
        this.scene_name = scene_name;
        this.growthStep = 1;
        this.growthRate = 1f;
        this.flowerType = flowerType;
        this.seasonsAllowed = new List<Season>();
        this.currentNeeds = new List<Need>();
    }

    public FlowerData(Vector3 position, string scene_name, int growthStep, float growthRate, FlowerType flowerType, List<Season> seasonsAllowed, List<Need> currentNeeds)
    {
        this.position = position;
        this.scene_name = scene_name;
        this.growthStep = growthStep;
        this.growthRate = growthRate;
        this.flowerType = flowerType;
        this.seasonsAllowed = seasonsAllowed;
        this.currentNeeds = currentNeeds;
    }
}

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}

public enum Need
{
    Water,
    Sunlight,
}

public enum FlowerType
{
    Fire, // 0
    Water, // 1
    Wind // 2
}


[System.Serializable]
public class InventoryData
{ // These are sort of like seeds.
    public int fireFlowerCount = 0;
    public int windFlowerCount = 0;
    public int iceFlowerCount = 0;
    public int waterFlowerCount = 0;

    public InventoryData()
    {
        this.fireFlowerCount = 0;
        this.windFlowerCount = 0;
        this.iceFlowerCount = 0;
        this.waterFlowerCount = 0;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is InventoryData))
            return false;

        InventoryData other = (InventoryData)obj;
        return this.fireFlowerCount == other.fireFlowerCount 
            && this.windFlowerCount == other.windFlowerCount
            && this.iceFlowerCount == other.iceFlowerCount
            && this.waterFlowerCount == other.waterFlowerCount;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(fireFlowerCount, windFlowerCount, iceFlowerCount, waterFlowerCount);
    }

    //We will add more.
}

[System.Serializable]
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcData;
    public List<FlowerData> flowerData;
    public InventoryData inventoryData;

    public SaveData()
    {
        this.playerData = new PlayerData();
        this.npcData = new List<NPCData>();
        this.flowerData = new List<FlowerData>();
        this.inventoryData = new InventoryData();
    }
}
