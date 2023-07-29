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
    }

    // Update is called once per frame
    void Update()


    {
	canMove = theDM.canNPCMove;
        if (!canMove)
        {
            myRigidbody2D.velocity = Vector2.zero;
            return;
        }
        if (isWalking)
        {
            walkCounter -= Time.deltaTime;
            
            

            switch(walkDirection)
            {
                case 0:;
                    myRigidbody2D.velocity = new Vector2 ( 0, moveSpeed );
                    break;

                case 1:;
                    myRigidbody2D.velocity = new Vector2(moveSpeed, 0);
                    break;

                case 2:;
                    myRigidbody2D.velocity = new Vector2(0, -moveSpeed);
                    break;

                case 3:;
                    myRigidbody2D.velocity = new Vector2(-moveSpeed, 0);
                    break;
            }

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;

            myRigidbody2D.velocity = Vector2.zero;

            if(waitCounter < 0 )
            {
                ChooseDirection();
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
