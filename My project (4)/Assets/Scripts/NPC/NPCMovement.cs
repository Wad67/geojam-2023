using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public List<Sprite> frames;
    public SpriteRenderer npcSprite;
    public float frameCycleSpeed = 0.5f;
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
    
    public float timer;
    private int currentDirectionFrameOffset;
    private int currentDirectionFrameCount;

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
        timer += Time.deltaTime;

        canMove = theDM.canNPCMove;
        if (!canMove)
        {
            myRigidbody2D.velocity = Vector2.zero;
            return;
        }

        if (isWalking)
        {
            walkCounter -= Time.deltaTime;

            switch (walkDirection)
            {	//up
                case 0:
                    myRigidbody2D.velocity = new Vector2(0, moveSpeed);
                    break;
		//right
                case 1:
                    myRigidbody2D.velocity = new Vector2(moveSpeed, 0);
                    break;
		//down
                case 2:
                    myRigidbody2D.velocity = new Vector2(0, -moveSpeed);
                    break;
		//left
                case 3:
                    myRigidbody2D.velocity = new Vector2(-moveSpeed, 0);
                    break;
            }
            
		if (Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Abs(myRigidbody2D.velocity.y))
		{
		    // Horizontal movement
		    currentDirectionFrameOffset = (myRigidbody2D.velocity.x > 0) ? 13 : 9;
		    currentDirectionFrameCount = 4;
		}
		else
		{
		    // Vertical movement
		    currentDirectionFrameOffset = (myRigidbody2D.velocity.y > 0) ? 1 : 5;
		    currentDirectionFrameCount = 4;
		}

            if (walkCounter < 0)
            {
                isWalking = false;
                waitCounter = waitTime;

            }
        }
        else
        {
            // No movement (idle)
            currentDirectionFrameOffset = 0;
            currentDirectionFrameCount = 1;
            waitCounter -= Time.deltaTime;

            myRigidbody2D.velocity = Vector2.zero;

            if (waitCounter < 0)
            {
                ChooseDirection();
            }
        }
        // Calculate the frame index for the current direction
        int frameIndex = currentDirectionFrameOffset + (int)(timer / frameCycleSpeed) % currentDirectionFrameCount;
        npcSprite.sprite = frames[frameIndex];
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 4);
        isWalking = true;

        walkCounter = walkTime;
    }
}

