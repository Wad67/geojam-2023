
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour

{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    InventoryController InventoryController;

    InventoryUI.GameState state;

    public bool canMove;


    [SerializeField] InventoryUI inventoryUI;
    private void Start()
    {
        canMove = true;
        state = InventoryUI.GameState.FreeRoam;
    }
    private void Awake()
    {
        InventoryController = GetComponent<InventoryController>();
        InventoryController.Closeinventory();
    }


    private void Update()
    {
        Debug.Log(state);
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        ProcessInputs();

        if (state == InventoryUI.GameState.Bag)
        {
            Action onBack = () =>
            {
                inventoryUI.gameObject.SetActive(false);
                state = InventoryUI.GameState.FreeRoam;
                //unfreeze player movement
                canMove = true;
            };


            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == InventoryUI.GameState.FreeRoam)
        {

            
            if (Input.GetKeyDown(KeyCode.B))
            {
                InventoryController.Openinventory();
                inventoryUI.ChangeState(InventoryUI.GameState.Bag);

                //freeze player movement
                canMove = false;
            }
        }

    }

    private void FixedUpdate()
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

