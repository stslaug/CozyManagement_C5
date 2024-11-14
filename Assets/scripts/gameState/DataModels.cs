using JetBrains.Annotations;
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
    public int saveSlot;
    public bool spellCast;
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
    public FlowerConfig flowerConfig; // Referencing the ScriptableObject for shared properties
    public Vector3 position;
    public int growthStep = 1; // Tracks the growth step for each flower instance
    public Vector3 initialScale = Vector3.one * 0.6f;
    public Vector3 maxScale = Vector3.one * 1.5f;
    public bool needSun = false;
    public bool needWater = false;
    public string scene_name;
}

[System.Serializable]
public class InventoryData
{ // These are sort of like seeds.
    public int fireFlowerCount = 0;
    public int windFlowerCount = 0;
    public int iceFlowerCount = 0;
    public int waterFlowerCount = 0;
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

    //We will add more.
}

[System.Serializable]
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcData;
    public List<FlowerData> allFlowerData;
    public InventoryData inventoryData;
}
}