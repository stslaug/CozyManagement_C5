using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resumeGame : MonoBehaviour
{
    Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ResumeGameOnClick);

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void ResumeGameOnClick()
    {
        Debug.Log("Resuming Session");
        GameObject gameObjectToClose = GameObject.Find("MenuComponents_Canvas");
        if (gameObjectToClose != null)
        {
            // Close the game object
            gameObjectToClose.SetActive(false);
            Time.timeScale = 1;
        }

    }
}
