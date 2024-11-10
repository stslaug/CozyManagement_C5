using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int goldCount;
    public int currentDay;
    public string lastTimePlayed;
    public string creationDate;
    public string lastScene;
    public int saveSlot;
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
    public float growthRate = 1;
    public string flowerType;

 
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
    public List<FlowerData> flowerData;
    public InventoryData inventoryData;
}
