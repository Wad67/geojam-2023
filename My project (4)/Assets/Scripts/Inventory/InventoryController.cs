using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject InventoryUI;
   public void Openinventory()
    {
        InventoryUI.SetActive(true);
        Debug.Log("Inventory is now open");
    }

    public void Closeinventory()
    {
        InventoryUI.SetActive(false);
        Debug.Log("Inventory is now closed");
        
    }
}
