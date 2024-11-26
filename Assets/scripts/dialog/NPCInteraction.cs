using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueNode startingNode;  // The first dialogue node to start with when the player clicks the NPC

    private DialogueManager dialogueManager;  // Reference to the DialogueManager

    void Start()
    {
        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnMouseDown()
    {
        // Print to see if the mouse click is detected
        Debug.Log("OnMouseDown called");

        if (dialogueManager != null && startingNode != null)
        {
            // Print a message to the console when the NPC is clicked
            Debug.Log("Yeti NPC clicked!");

            // Start the dialogue with the starting node
            dialogueManager.StartDialogue(startingNode);
        }
    }
}
