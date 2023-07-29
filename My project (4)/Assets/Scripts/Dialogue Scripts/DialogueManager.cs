using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject DialoguePanel;
    [SerializeField] private TextMeshProUGUI DialogueText;

    private PlayerMovement thePlayer;
    private bool isDialogueInProgress = false;
    private int currentNPCID; // The ID of the current NPC being interacted with
    private int ericaStage = 0;
    private bool hasEricaQuestItems = false; // True if the player has handed in the quest items for Erica
    // Add more state variables for other NPCs if needed

    public bool canNPCMove = true;

    // Texts for Erica's dialogue
    [Header("NPC Erica's Dialogues")]
    [TextArea(3, 10)] public string ericaIntroText;
    [TextArea(3, 10)] public string ericaRequirementText;
    [TextArea(3, 10)] public string ericaOutroText;

    // Add more public fields for other NPCs if needed

    void Start()
    {
        DialoguePanel.SetActive(false);
    }

    // Stop player from moving, stop all NPC from moving, show panel
    public void EnterDialogueMode(int npcID, GameObject triggerer)
    {
    	Debug.Log("Starting Convo");
        currentNPCID = npcID;
        canNPCMove = false;
        thePlayer = triggerer.GetComponent<PlayerMovement>();
        thePlayer.canMove = false;
        DialoguePanel.SetActive(true);
        isDialogueInProgress = true;

        ContinueDialogue();
    }

    // Display the specified text on the dialogue panel one newline at a time
    private IEnumerator DisplayText(string text, bool exitAfterDisplay)
    {
        DialogueText.text = ""; // Clear the text first

        string[] lines = text.Split('\n'); // Split the text into individual lines
        for (int i = 0; i < lines.Length; i++)
        {
            // Display the current line
            DialogueText.text = lines[i];

            // Wait for the player to press space or click the mouse
            while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            // Wait for a short duration before displaying the next line (you can adjust this time as needed)
            yield return new WaitForSeconds(0.1f);
        }

        // Wait for the player to press space or click the mouse to continue to the next stage or exit
        while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        if (exitAfterDisplay)
        {
            // Exit the dialogue after displaying the text
            EndDialogue();
        }
        else
        {
            // Progress the dialogue to the next stage
            ContinueDialogue();
        }
    }


    // Progress the dialogue to the next stage
    private void ContinueDialogue()
    {
        if (isDialogueInProgress)
        {
            // Increment the stage for Erica and display the appropriate dialogue
            if (currentNPCID == 0) // Erica's ID is 0
            {
                if (ericaStage == 0)
                {
                    StartCoroutine(DisplayText(ericaIntroText, false));
                    ericaStage++;
                }
                else if (ericaStage == 1)
                {
                    if (GameManager.Instance.HasItems("Sports Drink", 2))
                    {
                        GameManager.Instance.RemoveItems("Sports Drink", 2);
                        // Quest completed, progress to the next stage
                        StartCoroutine(DisplayText(ericaOutroText, false));
                        ericaStage++;
                    }
                    else
                    {
                        // Player hasn't fulfilled the requirement, show the requirement text again
                        StartCoroutine(DisplayText(ericaRequirementText, true));
                    }
                }
                else if (ericaStage == 2)
                {
                    StartCoroutine(DisplayText(ericaOutroText, true));
                }
                // Add more stages for other NPCs if needed
            }
        }
    }


    // End the dialogue and allow the player to move again
    private void EndDialogue()
    {
        DialoguePanel.SetActive(false);
        canNPCMove = true;
        thePlayer.canMove = true;
        isDialogueInProgress = false;
    }
}

