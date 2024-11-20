using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
[System.Serializable]
public class PlayerData
{
    public int goldCount;
    public int currentDay = 1;
    public string lastTimePlayed;
    public string creationDate;
    public string lastScene;
    public bool spellCast;
    //public Season rooftopSeason; // Season not implemented yet?

    public PlayerData()
    {
        this.goldCount = 500;
        this.currentDay = 1;
        this.lastTimePlayed = DateTime.Today.ToString();
        this.creationDate = DateTime.Today.ToString();
        this.lastScene = "temp_shop";
        this.spellCast = false;
        //this.rooftopSeason = Season.Spring; // Season not implemented yet?
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
    public int growthStep = 1; // Tracks the growth step for each flower instance
    public string flowerType = "";
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
    public List<GameObject> allFlowers;
    public InventoryData inventoryData;
}
}