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
        npcText.text = currentNode.dialogueText;  // Show the full dialogue text immediately
        isTyping = false;  // Set typing flag to false
    }
}
void ShowDialogue()
{
    if (currentNode == null)
        return;

    StopAllCoroutines();  // Stop any ongoing coroutine before starting a new one
    StartCoroutine(TypeDialogue(currentNode.dialogueText));  // Start typing dialogue

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
}


IEnumerator TypeDialogue(string dialogue)
{
    npcText.text = "";  // Clear existing text
    isTyping = true;    // Set typing flag to true

    string[] lines = dialogue.Split('\n');  // Split the dialogue into lines

    foreach (string line in lines)
    {
        // Type out each line character by character
        foreach (char letter in line.ToCharArray())
        {
            if (!isTyping)  // If typing was skipped, break immediately
                break;

            npcText.text += letter;  // Add each character
            yield return new WaitForSeconds(typingSpeed);  // Wait for typing speed
        }

        if (isTyping)  // Pause at the end of each line
        {
            npcText.text += "\n";  // Add a newline character after each line
            yield return new WaitForSeconds(0.5f);  // Small pause after each line
        }
    }

    isTyping = false;  // Typing is complete
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
    Debug.Log("Dialogue ended");
    currentNode = null;  // Reset currentNode to ensure no further dialogue is processed
}

}

