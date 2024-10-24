using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quitGame : MonoBehaviour
{
    Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(ExitFunction);

    }
        // Update is called once per frame
        void Update()
        {

        }

        void ExitFunction()
        {
            UnityEngine.Debug.Log("User has initated exiting the game");
            Application.Quit();
        }
    }
