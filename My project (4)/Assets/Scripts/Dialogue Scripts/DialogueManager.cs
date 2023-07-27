using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System;
using UnityEngine.EventSystems;



public class DialogueManager : MonoBehaviour
{
    [Header("Audio")]
    private AudioSource typingAudioSource;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject DialoguePanel;

    [SerializeField] private TextMeshProUGUI DialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying;

    private PlayerMovement thePlayer;

    private static DialogueManager instance;

    public List<Quest> allQuests = new List<Quest>();

    
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;

    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }


    void Start()
    {
        allQuests.Add(new Quest("Find the Lost Sheep", false));

        thePlayer = FindObjectOfType<PlayerMovement>();

        dialogueIsPlaying = false;
        DialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        thePlayer.canMove = false;
        Debug.Log("The player can not move");

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        DialoguePanel.SetActive(true);

        currentStory.BindExternalFunction("Start_Quest", (string questName) => {
            StartQuest(questName);
        });

        ContinueStory();
    }


    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
            
            
        }

        if (Input.GetKeyDown(KeyCode.Space)|| (Input.GetKeyDown(KeyCode.Mouse0))) //click space to continue dialogue
        {

            ContinueStory();


        }

    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        DialoguePanel.SetActive(false);
        DialogueText.text = "";

        thePlayer.canMove = true;
        Debug.Log("The player can move now");
    }

    private bool skip = false;

    private IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;

            // Play the typing audio clip every time a letter is appended
            if (typingAudioSource && typingAudioSource.clip)
            {
                typingAudioSource.Play();
            }

            // If skip becomes true, break from the loop
            if (skip)
            {
                DialogueText.text = sentence;
                break;
            }
            yield return new WaitForSeconds(0.05f); // adjust this value to change typing speed
        }
        skip = false;
    }


    public void StartQuest(string questName)
    {
        Quest quest = allQuests.Find(q => q.questName == questName);
        if (quest != null)
        {
            quest.isCompleted = false;
            Debug.Log("Quest " + questName + " started.");
        }
        else
        {
            Debug.Log("Quest not found.");
        }
    }


    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            // If a typing effect is ongoing, skip it, else start a new typing effect
            if (DialogueText.text == nextLine)
            {
                skip = true;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(nextLine));
            }

            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }




    private void DisplayChoices()
    {

        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }




}
