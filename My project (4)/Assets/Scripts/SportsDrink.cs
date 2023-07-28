using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SportsDrink : MonoBehaviour, ICollectable
{
    public static event HandleDrinkCollected OnDrinkCollected;
    public delegate void HandleDrinkCollected(ItemData itemData);
    public ItemData DrinkData;
    public void Collect()
    {
       OnDrinkCollected?.Invoke(DrinkData);
       Destroy(gameObject);
    }
    


}
