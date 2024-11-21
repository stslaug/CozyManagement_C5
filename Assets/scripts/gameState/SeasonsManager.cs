using UnityEngine;
using System;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;

    //public event Action<Season> OnSeasonChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //public void ChangeSeason(Season newSeason)
    //{
    //    if (GameManager.Instance.saveData.playerData.rooftopSeason != newSeason)
    //    {
    //        GameManager.Instance.saveData.playerData.rooftopSeason = newSeason;
    //        Debug.Log($"Season changed to: {GameManager.Instance.saveData.playerData.rooftopSeason}");
    //        OnSeasonChanged?.Invoke(GameManager.Instance.saveData.playerData.rooftopSeason);
    //    }
    //    else
    //    {
    //        Debug.Log("Season is already: " + GameManager.Instance.saveData.playerData.rooftopSeason);
    //    }
    //}
}
