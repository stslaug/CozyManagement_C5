using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class PlayerDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines; // Shop owner’s response line
    public float textSpeed;

    public bool finishedDialogue = false;

    private bool isTriggered = false;


    private int index;


    void Update()
    {
        if (isTriggered && Input.GetMouseButtonDown(0))
        {
             if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    //start dialogue
    public void TriggerResponse()
    {

        textComponent.text = string.Empty;
        isTriggered = true;
        StartPlayerDialogue();
    }

    void StartPlayerDialogue()
    {   
        index = 0;
        gameObject.SetActive(true);
        StartCoroutine(TypePlayerLines());
    }

    IEnumerator TypePlayerLines()
    {
       foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypePlayerLines());
        }
        else
        {
           // gameObject.SetActive(false); //hide shop owner dialgoue    
            finishedDialogue = true;
            LoadNextScene();
        }
    }
    void LoadNextScene()
    {
        // Replace "NextScene" with the name of your next scene
        SceneManager.LoadScene("temp_shop"); 
    }
}