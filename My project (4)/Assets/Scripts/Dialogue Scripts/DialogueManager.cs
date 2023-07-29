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
    
    
    
    [SerializeField] public GameObject sportsDrinkTuckshopLady;




    public bool canNPCMove = true;
    //NPCID 0
    [Header("NPC Erica's Dialogues")]
    [TextArea(3, 10)] public string ericaIntroText;
    [TextArea(3, 10)] public string ericaRequirementText;
    [TextArea(3, 10)] public string ericaOutroText;
    public int ericaStage = 0;

    //NPCID 1
    [Header("Tuckshop lady")]
    [TextArea(3, 10)] public string tuckladyIntroText;
    [TextArea(3, 10)] public string tuckladyRequirementText;
    [TextArea(3, 10)] public string tuckladyOutroText;
    public int tucklady = 0;
    
    //NPCID 2
    [Header("Calculator kid")]
    [TextArea(3, 10)] public string calckidIntroText;
    [TextArea(3, 10)] public string calckidRequirementText;
    [TextArea(3, 10)] public string calckidOutroText;
    public int calckid = 0;
    //NPCID 3
    [Header("paintbrush kid")]
    [TextArea(3, 10)] public string paintkidIntroText;
    [TextArea(3, 10)] public string paintkidRequirementText;
    [TextArea(3, 10)] public string paintkidOutroText;
    public int paintkid = 0;
    
    
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

private bool canAdvance = false;

private IEnumerator DisplayText(string text, bool exitAfterDisplay)
{
    DialogueText.text = ""; // Clear the text first

    string[] lines = text.Split(new char[] { '\n', '\r' }); // Split the text into individual lines

    foreach (string line in lines)
    {
        // Display the current line character by character
        yield return StartCoroutine(DisplayLineCharacterByCharacter(line));

        // Wait for player input to continue to the next line
        canAdvance = false;
        while (!canAdvance)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                canAdvance = true;
            }
            yield return null;
        }
    }

    // Text has finished displaying, allow advancing
    canAdvance = true;

    // Wait for the player to press space or click the mouse to continue to the next stage or exit
    while (!canAdvance || (!Input.GetKeyDown(KeyCode.Space) && !Input.GetMouseButtonDown(0)))
    {
        yield return null;
    }

    // Reset canAdvance to false
    canAdvance = false;

    // Check if there is more text to display or if the exitAfterDisplay flag is set
    if (!exitAfterDisplay)
    {
        // Progress the dialogue to the next stage
        ContinueDialogue();
    }
    else
    {
        // Exit the dialogue after displaying the text or if the exitAfterDisplay flag is set
        EndDialogue();
    }
}

// Coroutine to display the text character by character
private IEnumerator DisplayLineCharacterByCharacter(string line)
{
    DialogueText.text = ""; // Clear the text first

    for (int i = 0; i < line.Length; i++)
    {
        // Append the current character to the displayed text
        DialogueText.text += line[i];

        // Wait for a short duration before displaying the next character (you can adjust this time as needed)
        yield return new WaitForSeconds(0.05f); // Adjust the duration to control the speed of text printing
    }

    // Wait for a short duration after displaying the entire line (you can adjust this time as needed)
    yield return new WaitForSeconds(0.1f);

    // Text has finished displaying, allow advancing to the next line
    canAdvance = true;
}

private void ContinueDialogue()
{
    if (isDialogueInProgress)
    {
        if (currentNPCID == 0) // Erica's ID is 0
        {
            // Erica's Dialogue Logic
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
                    StartCoroutine(DisplayText(ericaOutroText, true));
                    GameManager.Instance.questCount--; // Decrement quest count for successful hand-in
                    ericaStage++;
                }
                else
                {
                    // Player hasn't fulfilled the requirement, show the requirement text
                    StartCoroutine(DisplayText(ericaRequirementText, true));
                }
            }
            else if (ericaStage == 2)
            {
                // Erica's quest is completed, show the outro text
                StartCoroutine(DisplayText(ericaOutroText, true));
            }
        }
        else if (currentNPCID == 1) // Tuckshop lady's ID is 1
        {
            // Tuckshop lady's Dialogue Logic
            if (tucklady == 0)
            {
                StartCoroutine(DisplayText(tuckladyIntroText, false));
                tucklady++;
            }
            else if (tucklady == 1)
            {
                if (GameManager.Instance.HasItems("Money", 5))
                {
                    GameManager.Instance.RemoveItems("Money", 5);
                    GameObject sportsDrink = Instantiate(sportsDrinkTuckshopLady);
                    GameManager.Instance.AddItem(sportsDrink);
                    // Quest completed, progress to the next stage
                    StartCoroutine(DisplayText(tuckladyOutroText, true));
                    tucklady++;
                }
                else
                {
                    // Player hasn't fulfilled the requirement, show the requirement text
                    StartCoroutine(DisplayText(tuckladyRequirementText, true));
                }
            }
            else if (tucklady == 2)
            {
                // Tuckshop lady's quest is completed, show the outro text
                StartCoroutine(DisplayText(tuckladyOutroText, true));
            }
        }
        else if (currentNPCID == 2) // Calculator kid's ID is 2
        {
            // Calculator kid's Dialogue Logic
            if (calckid == 0)
            {
                StartCoroutine(DisplayText(calckidIntroText, false));
                calckid++;
            }
            else if (calckid == 1)
            {
                if (GameManager.Instance.HasItems("Calculator", 1))
                {
                    GameManager.Instance.RemoveItems("Calculator", 1);
                    
                    // Quest completed, progress to the next stage
                    StartCoroutine(DisplayText(calckidOutroText, true));
                    GameManager.Instance.questCount--; // Decrement quest count for successful hand-in
                    calckid++;
                }
                else
                {
                    // Player hasn't fulfilled the requirement, show the requirement text
                    StartCoroutine(DisplayText(calckidRequirementText, true));
                }
            }
            else if (calckid == 2)
            {
                // Calculator kid's quest is completed, show the outro text
                StartCoroutine(DisplayText(calckidOutroText, true));
            }
        }
        else if (currentNPCID == 3) // Paintbrush kid's ID is 3
        {
            // Paintbrush kid's Dialogue Logic
            if (paintkid == 0)
            {
                StartCoroutine(DisplayText(paintkidIntroText, false));
                paintkid++;
            }
            else if (paintkid == 1)
            {
                if (GameManager.Instance.HasItems("Paintbrush", 1))
                {
                    GameManager.Instance.RemoveItems("Paintbrush", 1);
                    
                    // Quest completed, progress to the next stage
                    StartCoroutine(DisplayText(paintkidOutroText, true));
                    GameManager.Instance.questCount--; // Decrement quest count for successful hand-in
                    paintkid++;
                }
                else
                {
                    // Player hasn't fulfilled the requirement, show the requirement text
                    StartCoroutine(DisplayText(paintkidRequirementText, true));
                }
            }
            else if (paintkid == 2)
            {
                // Paintbrush kid's quest is completed, show the outro text
                StartCoroutine(DisplayText(paintkidOutroText, true));
            }
        }
        // Add more NPCs' dialogues and stages here if needed
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

