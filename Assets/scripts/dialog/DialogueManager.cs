using TMPro;  
using UnityEngine;  
using UnityEngine.UI;  
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI npcText;             // TextMeshPro UI Text to display dialogue
    public Button[] choiceButtons;              // Buttons for player choices
    public GameObject dialogueBox;              // Dialogue box UI to show or hide
    public float typingSpeed = 0.05f;           // Speed of typing effect

    private DialogueNode currentNode;           // Current dialogue node the player is on
    private bool isTyping = false;              // To prevent skipping the text during typing
    private int currentLineIndex = 0;           // To track the current line being displayed

    void Start()
    {
        dialogueBox.SetActive(false);           // Hide dialogue box initially
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false); // Hide all choice buttons initially
        }
    }

    public void StartDialogue(DialogueNode startingNode)
    {
        currentNode = startingNode;
        dialogueBox.SetActive(true);             // Show dialogue box
        ShowDialogue();
    }

    void Update()
    {
        if (isTyping && Input.anyKeyDown)  // Check for any key press while typing
        {
            StopAllCoroutines();  // Stop the current typing coroutine
            npcText.text = currentNode.dialogueText.Split('\n')[currentLineIndex];  // Show the full line immediately
            isTyping = false;  // Set typing flag to false
        }
        else if (!isTyping && Input.anyKeyDown)
        {
            ShowNextLine();  // Show the next line when a key is pressed
        }
    }

    void ShowDialogue()
    {
        if (currentNode == null)
            return;

        StopAllCoroutines();  // Stop any ongoing coroutine before starting a new one
        currentLineIndex = 0;  // Reset the line index
        StartCoroutine(TypeDialogue(currentNode.dialogueText.Split('\n')[currentLineIndex]));  // Start typing first line

        // Hide all buttons first
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Show choices if they exist
        if (currentNode.choices.Count > 0)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                if (i < choiceButtons.Length)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].choiceText;

                    int choiceIndex = i;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() => OnChoiceMade(choiceIndex));
                }
            }
        }
        else
        {
            // If no choices exist, end the dialogue
            EndDialogue();
        }
    }

    IEnumerator TypeDialogue(string line)
    {
        npcText.text = "";  // Clear existing text
        isTyping = true;    // Set typing flag to true

        foreach (char letter in line.ToCharArray())
        {
            npcText.text += letter;  // Add each character
            yield return new WaitForSeconds(typingSpeed);  // Wait for typing speed
        }

        isTyping = false;  // Typing is complete
    }

    void ShowNextLine()
    {
        if (currentLineIndex < currentNode.dialogueText.Split('\n').Length - 1)
        {
            currentLineIndex++;  // Move to the next line
            StartCoroutine(TypeDialogue(currentNode.dialogueText.Split('\n')[currentLineIndex]));  // Start typing next line
        }
        else
        {
            // No more lines to show, proceed with choices
            ShowChoices();
        }
    }

    void ShowChoices()
    {
        // Show choices after dialogue is complete
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false); // Hide all buttons initially
        }

        if (currentNode.choices.Count > 0)
        {
            for (int i = 0; i < currentNode.choices.Count; i++)
            {
                if (i < choiceButtons.Length)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentNode.choices[i].choiceText;

                    int choiceIndex = i;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() => OnChoiceMade(choiceIndex));
                }
            }
        }
    }

    void OnChoiceMade(int choiceIndex)
    {
        Debug.Log("Choice made: " + choiceIndex);  // Log which choice was made

        // Ensure the choiceIndex is within valid range
        if (choiceIndex >= 0 && choiceIndex < currentNode.choices.Count)
        {
            DialogueNode nextNode = currentNode.choices[choiceIndex].nextNode;

            if (nextNode != null)
            {
                Debug.Log("Next Node: " + nextNode.name);  // Log the next node
                currentNode = nextNode;  // Move to the next node
                ShowDialogue();  // Show the next part of the dialogue
            }
            else
            {
                // If no nextNode, end the dialogue
                Debug.Log("No next node, ending dialogue.");
                EndDialogue();
            }
        }
        else
        {
            Debug.LogError("Choice index out of bounds! Index: " + choiceIndex);
            EndDialogue();  // End dialogue in case of invalid choice
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);  // Hide the dialogue box
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);  // Hide all buttons
        }

        // Disable the Yeti NPC (or any other NPC)
        GameObject yeti = GameObject.Find("yeti");  // Replace "Yeti" with your NPC's GameObject name
        if (yeti != null)
        {
            yeti.SetActive(false);  // Disable the Yeti NPC
        }

        GameObject granny = GameObject.Find("granny");  // Replace "Yeti" with your NPC's GameObject name
        if (granny != null)
        {
            granny.SetActive(false);  // Disable the Yeti NPC
        }

        Debug.Log("Dialogue ended");
        currentNode = null;  // Reset currentNode to ensure no further dialogue is processed
    }
}
