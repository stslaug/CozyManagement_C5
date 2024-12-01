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
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcData;
    public List<Flower> allFlowers;

}
}