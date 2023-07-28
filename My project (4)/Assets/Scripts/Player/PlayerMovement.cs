
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour

{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public bool canMove;

    void Update()
    {
        ProcessInputs();  

        if(!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }


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
        }

        void Move()

        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }





    
}

