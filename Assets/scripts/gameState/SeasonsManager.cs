using UnityEngine;
using System;

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance;

    public Season currentSeason = Season.Spring;

    public event Action<Season> OnSeasonChanged;

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

    public void ChangeSeason(Season newSeason)
    {
        if (currentSeason != newSeason)
        {
            currentSeason = newSeason;
            Debug.Log($"Season changed to: {currentSeason}");
            OnSeasonChanged?.Invoke(currentSeason);
        }
        else
        {
            Debug.Log("Season is already: " + currentSeason);
        }
    }
}
