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
    public float positionX;
    public float positionY;
    public string scene_name;
    public int growthStep = 1;

 
    public GameObject flowerPrefab; 
    public FlowerData(float x, float y, GameObject prefab)
    {
        positionX = x;
        positionY = y;
        flowerPrefab = prefab;
    }
}

[System.Serializable]
public class SaveData
{
    public PlayerData playerData = new PlayerData();
    public List<NPCData> npcData = new List<NPCData>();
    public List<FlowerData> flowerData = new List<FlowerData>();
    public int saveSlot;
}
