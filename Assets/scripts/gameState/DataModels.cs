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
public class SaveData
{
    public PlayerData playerData;
    public List<NPCData> npcData;
    public List<FlowerData> flowerData;
}
