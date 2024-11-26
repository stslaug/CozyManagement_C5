using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string dialogueText;  // The text for the NPC's dialogue
    public List<DialogueChoice> choices = new List<DialogueChoice>();  // Choices the player can make
}


[System.Serializable]
public class DialogueChoice
{
    public string choiceText;      // The player's choice text
    public DialogueNode nextNode;  // The next node to go to based on the choice
}


