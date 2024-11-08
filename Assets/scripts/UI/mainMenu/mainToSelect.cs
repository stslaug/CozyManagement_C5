using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainToSelect : MonoBehaviour
{
    [SerializeField]
    GameObject MainMenuPanel;
    [SerializeField]
    GameObject SaveSelectMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuPanel.SetActive(true);
        SaveSelectMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { }


    public void switchToSelect()
    {
        Debug.Log("Switching to Selection");
        MainMenuPanel.SetActive(false);
        SaveSelectMenuPanel.SetActive(true);
    }
    public void switchToMain()
    {
        Debug.Log("Switching to Mains");
        SaveSelectMenuPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        
    }

}
