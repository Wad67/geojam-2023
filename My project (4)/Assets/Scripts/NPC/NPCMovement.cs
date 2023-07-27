using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D myRigidbody2D;
    public bool isWalking;
    public float walkTime;
    private float walkCounter;
    public float waitTime;
    private float waitCounter;
    private int walkDirection;
    private Animator anim;
    public bool canMove;
    private DialogueManager theDM;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        waitCounter = waitTime;
        walkCounter = walkTime;
        ChooseDirection();
        canMove = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!theDM.dialogueIsPlaying)
        {
            canMove = true;
        }

        if (!canMove)
        {
            myRigidbody2D.velocity = Vector2.zero;
            return;
        }

        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            switch (walkDirection)
            {
                case 0:
                    myRigidbody2D.velocity = new Vector2(0, moveSpeed);
                    anim.SetFloat("MoveY", 1); // Set the "MoveY" parameter in the Animator for up movement
                    break;
                case 1:
                    myRigidbody2D.velocity = new Vector2(moveSpeed, 0);
                    anim.SetFloat("MoveY", 0); // Set the "MoveY" parameter in the Animator for right movement
                    break;
                case 2:
                    myRigidbody2D.velocity = new Vector2(0, -moveSpeed);
                    anim.SetFloat("MoveY", -1); // Set the "MoveY" parameter in the Animator for down movement
                    break;
                case 3:
                    myRigidbody2D.velocity = new Vector2(-moveSpeed, 0);
                    anim.SetFloat("MoveY", 0); // Set the "MoveY" parameter in the Animator for left movement
                    break;
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
                anim.SetBool("isWalking", false); // Set the "isWalking" parameter to false to trigger the idle animation
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;

            myRigidbody2D.velocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
                anim.SetBool("isWalking", true); // Set the "isWalking" parameter to true to trigger the walk animation
            }
        }
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;
        walkCounter = walkTime;
    }
}
