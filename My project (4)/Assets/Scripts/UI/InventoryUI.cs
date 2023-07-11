using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public enum GameState
    {
        Bag,
        FreeRoam
        // Other states...
    }


    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;

    [SerializeField] Image ItemIcon;
    [SerializeField] TMP_Text itemDescription;

    int selectedItem = 0;

    GameState state;

    List<ItemSlotUI> slotUIList;

    Inventory inventory;
    RectTransform itemListRect;

    PlayerMovement playerMovement;


    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();  // Get RectTransform of itemList, not this GameObject
        // Assuming the PlayerMovement script is attached to a GameObject tagged as "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player object not found");
        }
    }

    // Method for changing state
    public void ChangeState(GameState newState)
    {
        state = newState;
    }

    private void Start()
    {
        UpdateItemList();
    }

    private void Update()
    {
        HandleUpdate(() => {
            // Actions to perform when the player presses 'X'.
            // This could be exiting the inventory or some other action.
            this.gameObject.SetActive(false);  // Assuming this closes the inventory
            state = GameState.FreeRoam;
        });
    }

    void UpdateItemList()
    {
        //check if itemList is null
        if (itemList == null || itemList.transform == null)
        {
            Debug.LogError("itemList or itemList.transform is null");
            return;
        }

        //check if inventory or inventory.Slots is null
        if (inventory == null || inventory.Slots == null)
        {
            Debug.LogError("inventory or inventory.Slots is null");
            return;
        }

        //check if itemSlotUI is null
        if (itemSlotUI == null)
        {
            Debug.LogError("itemSlotUI is null");
            return;
        }

        //clear all existing items
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        slotUIList = new List<ItemSlotUI>();
        foreach (var itemSlot in inventory.Slots)
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            if (slotUIObj == null)
            {
                Debug.LogError("slotUIObj is null");
                return;
            }
            slotUIObj.SetData(itemSlot);
            slotUIList.Add(slotUIObj);
        }
        UpdateItemSelection();
    }



    public void HandleUpdate(Action onBack)
    {
        // If the game state isn't Bag, don't run the method.
        if (state != GameState.Bag)
        {
            return;
        }

        int prevSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.S))
            ++selectedItem;
        else if (Input.GetKeyDown(KeyCode.W))
            --selectedItem;

        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

        if (prevSelection != selectedItem)
            UpdateItemSelection();

        if (state == GameState.Bag && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Closing inventory and enabling player movement");

            // Update game state and player movement before disabling this GameObject
            state = GameState.FreeRoam;
            playerMovement.canMove = true;

            onBack?.Invoke();

            Debug.Log($"Game state changed to {state}");
        }

    }


    void UpdateItemSelection()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem)
                slotUIList[i].NameText.color = Color.blue;
            else
                slotUIList[i].NameText.color = Color.black;
        }

        var slot = inventory.Slots[selectedItem];
        ItemIcon.sprite = slot.Item.Icon; 
        itemDescription.text = slot.Item.Description;

        HandleScrolling();
    }

    void HandleScrolling()
    {
        float itemSlotHeight = slotUIList[0].GetComponent<RectTransform>().sizeDelta.y;  // Or use rect.height
        float scrollPos = selectedItem * itemSlotHeight;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);
    }


}
