using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public DialogueNode grannyNode;   // Dialogue node for Granny
    public DialogueNode yetiNode;     // Dialogue node for Yeti

    public GameObject grannySprite;   // Reference to the Granny sprite/GameObject
    public GameObject yetiSprite;     // Reference to the Yeti sprite/GameObject

    private DialogueManager dialogueManager;  // Reference to the DialogueManager

    void Start()
    {
        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();

        // Find the granny and yeti sprites by name (if they're in the scene)
        grannySprite = GameObject.Find("granny");  // Reference the Granny sprite
        yetiSprite = GameObject.Find("yeti");      // Reference the Yeti sprite

        // Ensure both sprites are hidden initially
        if (grannySprite != null) grannySprite.SetActive(false);
        if (yetiSprite != null) yetiSprite.SetActive(false);

        // Find buttons and add listeners for them
        Button grannyButton = GameObject.Find("GrannyButton").GetComponent<Button>();
        Button yetiButton = GameObject.Find("YetiButton").GetComponent<Button>();

        if (grannyButton != null)
            grannyButton.onClick.AddListener(OnGrannyButtonClick);  // Add listener for Granny button

        if (yetiButton != null)
            yetiButton.onClick.AddListener(OnYetiButtonClick);  // Add listener for Yeti button
    }

    void OnGrannyButtonClick()
    {
        Debug.Log("Granny Button Clicked");  // Add this for debugging

        // Show the Granny sprite and hide Yeti sprite
        if (grannySprite != null) grannySprite.SetActive(true);
        if (yetiSprite != null) yetiSprite.SetActive(false);

        // Start the dialogue for Granny when the button is clicked
        if (dialogueManager != null && grannyNode != null)
        {
            // Ensure the dialogue starts immediately after activating the sprite
            dialogueManager.StartDialogue(grannyNode);
        }
    }

    void OnYetiButtonClick()
    {
        Debug.Log("Yeti Button Clicked");  // Add this for debugging

        // Show the Yeti sprite and hide Granny sprite
        if (grannySprite != null) grannySprite.SetActive(false);
        if (yetiSprite != null) yetiSprite.SetActive(true);

        // Start the dialogue for Yeti when the button is clicked
        if (dialogueManager != null && yetiNode != null)
        {
            // Ensure the dialogue starts immediately after activating the sprite
            dialogueManager.StartDialogue(yetiNode);
        }
    }
}
