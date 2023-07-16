using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Calculator : MonoBehaviour, ICollectable
{
    public static event HandleCalculatorCollected OnCalculatorCollected;
    public delegate void HandleCalculatorCollected(ItemData itemData);
    public ItemData CalculatorData;
    public void Collect()
    {
        OnCalculatorCollected?.Invoke(CalculatorData);
        Destroy(gameObject);
    }
}



