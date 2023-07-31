using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

	

    private bool playerInRange;



    private DialogueManager manager;
    private PlayerMovement playermovement;
    
    
    private GameObject playerObject;
    
    public int npcID = 0;

    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
        playermovement = FindObjectOfType<PlayerMovement>();
        playerInRange = false;
        visualCue.SetActive(false);

    }

    private void Update()
    {
        if (playerInRange ) 
        {
            visualCue.SetActive(true);
            if ( playermovement.canMove){
            if (Input.GetKeyDown(KeyCode.E ))
            {
                manager.EnterDialogueMode(npcID,playerObject);

                if(transform.parent.GetComponent<NPCMovement>() != null)
                {
                    transform.parent.GetComponent<NPCMovement>().canMove = false;
                }
            }
        }}
        else
        {
            visualCue.SetActive(false);


        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
       	    playerObject = collider.gameObject;
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }


    }
}
