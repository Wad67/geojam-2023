using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
// I could have used the very contrived unity animation system, but I elected not to 
// This just cycles through ranges of frames, based on direction
public class PlayerMovement : MonoBehaviour
{
    public List<Sprite> frames;
    public SpriteRenderer playerSprite;
    public float frameCycleSpeed = 0.5f;
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    private float moveTimer;
    private int currentDirectionFrameOffset;
    private int currentDirectionFrameCount;

    public bool canMove;
    public float fsT = 0;
    public float footStepT = 0.3f;
    private bool isLeftFootstep = true;

    void Start()
    {
        currentDirectionFrameOffset = 0;
        currentDirectionFrameCount = 4; // Assuming 4 frames for each direction
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        
        
        

        if (!canMove)
        {
            moveDirection = Vector2.zero;
            rb.velocity = Vector2.zero;
            return;
        }else{
        
            ProcessInputs();  
            
        
        }

        // Calculate the frame index for the current direction
        int frameIndex = currentDirectionFrameOffset + (int)(moveTimer / frameCycleSpeed) % currentDirectionFrameCount;
        playerSprite.sprite = frames[frameIndex];
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        // Update the current direction frame offset and count
        if (moveDirection == Vector2.zero)
        {
            // No movement (idle)
            currentDirectionFrameOffset = 8;
            currentDirectionFrameCount = 1;
        }
        else if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            // Horizontal movement
            currentDirectionFrameOffset = (moveDirection.x > 0) ? 9 : 4;
            currentDirectionFrameCount = 4;
        }
        else
        {	
            // Vertical movement
            currentDirectionFrameOffset = (moveDirection.y > 0) ? 0 : 13;
            currentDirectionFrameCount = 4;
        }
    }

    void Move()
    {
    	if (moveDirection != Vector2.zero){
    	        fsT += Time.deltaTime;
    	        }
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        if (fsT >= footStepT){
            // Get a random volume between minVolume and maxVolume
            float volume = UnityEngine.Random.Range(0.4f, 1.0f);

            // Play the footstep sound based on the footstep type (left or right)
            int footstepIndex = isLeftFootstep ? 4 : 5;
            GameManager.Instance.gameSoundsSource.PlayOneShot(GameManager.Instance.gameSounds[footstepIndex], volume);

            // Alternate the footstep type for the next step
            isLeftFootstep = !isLeftFootstep;

            // Reset the footstep timer
            fsT = 0;
        }
        
        }
        
    }


