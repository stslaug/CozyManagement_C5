using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public PlayerData()
    {
        this.goldCount = 500;
        this.currentDay = 1;
        this.lastTimePlayed = DateTime.Today.ToString();
        this.creationDate = DateTime.Today.ToString();
        this.spellCast = false;
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
    public string flowerType;
}

[System.Serializable]
public class InventoryData
    { // Primary Seeds
        public int fire_seed = 0;
        public int wind_seed = 0;
        public int water_seed = 0;


        public int fire_extract = 0;
        public int wind_extract = 0;
        public int water_extract = 0;

        public int ice_extract = 0;

        public override bool Equals(object obj)
        {
            return obj is InventoryData data &&
                   fire_seed == data.fire_seed &&
                   wind_seed == data.wind_seed &&
                   water_seed == data.water_seed &&
                   fire_extract == data.fire_extract &&
                   wind_extract == data.wind_extract &&
                   water_extract == data.water_extract &&
                   ice_extract == data.ice_extract;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fire_seed, wind_seed, water_seed, fire_extract, wind_extract, water_extract, ice_extract);
        }



        //We will add more.
    }

[System.Serializable]
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcData;
    public List<FlowerConfig> allFlowers;
    public InventoryData inventoryData;
}
}